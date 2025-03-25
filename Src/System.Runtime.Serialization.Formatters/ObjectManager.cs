// Decompiled with JetBrains decompiler
// Type: System.Runtime.Serialization.ObjectManager
// Assembly: System.Runtime.Serialization.Formatters, Version=4.0.3.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 59FEC222-A9B8-4506-99C2-FAFFD4E25D28
// Assembly location: C:\Users\Admin\Desktop\RE\1\System.Runtime.Serialization.Formatters.dll

using System.Globalization;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;


namespace System.Runtime.Serialization
{
  public class ObjectManager
  {
    private const int DefaultInitialSize = 16;
    private const int MaxArraySize = 1048576;
    private const int ArrayMask = 1048575;
    private const int MaxReferenceDepth = 100;
    private DeserializationEventHandler _onDeserializationHandler;
    private SerializationEventHandler _onDeserializedHandler;
    internal ObjectHolder[] _objects;
    internal object _topObject;
    internal ObjectHolderList _specialFixupObjects;
    internal long _fixupCount;
    internal readonly ISurrogateSelector _selector;
    internal readonly StreamingContext _context;
    private readonly bool _isCrossAppDomain;

    public ObjectManager(ISurrogateSelector selector, StreamingContext context)
      : this(selector, context, true, false)
    {
    }

    internal ObjectManager(
      ISurrogateSelector selector,
      StreamingContext context,
      bool checkSecurity,
      bool isCrossAppDomain)
    {
      this._objects = new ObjectHolder[16];
      this._selector = selector;
      this._context = context;
      this._isCrossAppDomain = isCrossAppDomain;
    }

    private bool CanCallGetType(object obj) => true;

    internal object TopObject
    {
      set => this._topObject = value;
      get => this._topObject;
    }

    internal ObjectHolderList SpecialFixupObjects
    {
      get => this._specialFixupObjects ?? (this._specialFixupObjects = new ObjectHolderList());
    }

    internal ObjectHolder FindObjectHolder(long objectID)
    {
      int index = (int) (objectID & 1048575L);
      if (index >= this._objects.Length)
        return (ObjectHolder) null;
      ObjectHolder next = this._objects[index];
      while (next != null && next._id != objectID)
        next = next._next;
      return next;
    }

    internal ObjectHolder FindOrCreateObjectHolder(long objectID)
    {
      ObjectHolder holder = this.FindObjectHolder(objectID);
      if (holder == null)
      {
        holder = new ObjectHolder(objectID);
        this.AddObjectHolder(holder);
      }
      return holder;
    }

    private void AddObjectHolder(ObjectHolder holder)
    {
      if (holder._id >= (long) this._objects.Length && this._objects.Length != 1048576)
      {
        int length = 1048576;
        if (holder._id < 524288L)
        {
          length = this._objects.Length * 2;
          while ((long) length <= holder._id && length < 1048576)
            length *= 2;
          if (length > 1048576)
            length = 1048576;
        }
        ObjectHolder[] destinationArray = new ObjectHolder[length];
        Array.Copy((Array) this._objects, 0, (Array) destinationArray, 0, this._objects.Length);
        this._objects = destinationArray;
      }
      int index = (int) (holder._id & 1048575L);
      ObjectHolder objectHolder = this._objects[index];
      holder._next = objectHolder;
      this._objects[index] = holder;
    }

    private bool GetCompletionInfo(
      FixupHolder fixup,
      out ObjectHolder holder,
      out object member,
      bool bThrowIfMissing)
    {
      member = fixup._fixupInfo;
      holder = this.FindObjectHolder(fixup._id);
      if (!holder.CompletelyFixed && holder.ObjectValue != null && holder.ObjectValue is ValueType)
      {
        this.SpecialFixupObjects.Add(holder);
        return false;
      }
      if (holder != null && !holder.CanObjectValueChange && holder.ObjectValue != null)
        return true;
      if (!bThrowIfMissing)
        return false;
      if (holder == null)
        throw new SerializationException(SR.Format(SR.Serialization_NeverSeen, (object) fixup._id));
      if (holder.IsIncompleteObjectReference)
        throw new SerializationException(SR.Format(SR.Serialization_IORIncomplete, (object) fixup._id));
      throw new SerializationException(SR.Format(SR.Serialization_ObjectNotSupplied, (object) fixup._id));
    }

    private void FixupSpecialObject(ObjectHolder holder)
    {
      ISurrogateSelector selector = (ISurrogateSelector) null;
      if (holder.HasSurrogate)
      {
        ISerializationSurrogate surrogate = holder.Surrogate;
        object obj = surrogate.SetObjectData(holder.ObjectValue, holder.SerializationInfo, this._context, selector);
        if (obj != null)
        {
          if (!holder.CanSurrogatedObjectValueChange && obj != holder.ObjectValue)
            throw new SerializationException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, SR.Serialization_NotCyclicallyReferenceableSurrogate, (object) surrogate.GetType().FullName));
          holder.SetObjectValue(obj, this);
        }
        holder._surrogate = (ISerializationSurrogate) null;
        holder.SetFlags();
      }
      else
        this.CompleteISerializableObject(holder.ObjectValue, holder.SerializationInfo, this._context);
      holder.SerializationInfo = (SerializationInfo) null;
      holder.RequiresSerInfoFixup = false;
      if (holder.RequiresValueTypeFixup && holder.ValueTypeFixupPerformed)
        this.DoValueTypeFixup((FieldInfo) null, holder, holder.ObjectValue);
      this.DoNewlyRegisteredObjectFixups(holder);
    }

    private bool ResolveObjectReference(ObjectHolder holder)
    {
      int num = 0;
      try
      {
        object objectValue;
        do
        {
          objectValue = holder.ObjectValue;
          holder.SetObjectValue(((IObjectReference) holder.ObjectValue).GetRealObject(this._context), this);
          if (holder.ObjectValue == null)
          {
            holder.SetObjectValue(objectValue, this);
            return false;
          }
          if (num++ == 100)
            throw new SerializationException(SR.Serialization_TooManyReferences);
          if (!(holder.ObjectValue is IObjectReference))
            break;
        }
        while (objectValue != holder.ObjectValue);
      }
      catch (NullReferenceException ex)
      {
        return false;
      }
      holder.IsIncompleteObjectReference = false;
      this.DoNewlyRegisteredObjectFixups(holder);
      return true;
    }

    private bool DoValueTypeFixup(FieldInfo memberToFix, ObjectHolder holder, object value)
    {
      FieldInfo[] sourceArray = new FieldInfo[4];
      int length = 0;
      int[] numArray = (int[]) null;
      object objectValue = holder.ObjectValue;
      while (holder.RequiresValueTypeFixup)
      {
        if (length + 1 >= sourceArray.Length)
        {
          FieldInfo[] destinationArray = new FieldInfo[sourceArray.Length * 2];
          Array.Copy((Array) sourceArray, 0, (Array) destinationArray, 0, sourceArray.Length);
          sourceArray = destinationArray;
        }
        ValueTypeFixupInfo valueFixup = holder.ValueFixup;
        objectValue = holder.ObjectValue;
        if (valueFixup.ParentField != (FieldInfo)null)//if (FieldInfo.op_Inequality(valueFixup.ParentField, (FieldInfo) null))
        {
          FieldInfo parentField = valueFixup.ParentField;
          ObjectHolder objectHolder = this.FindObjectHolder(valueFixup.ContainerID);
          if (objectHolder.ObjectValue != null)
          {
            if (Nullable.GetUnderlyingType(parentField.FieldType) != (Type) null)
            {
              sourceArray[length] = parentField.FieldType.GetField(nameof (value), (BindingFlags) 36);
              ++length;
            }
            sourceArray[length] = parentField;
            holder = objectHolder;
            ++length;
          }
          else
            break;
        }
        else
        {
          holder = this.FindObjectHolder(valueFixup.ContainerID);
          numArray = valueFixup.ParentIndex;
          break;
        }
      }
      if (!(holder.ObjectValue is Array) && holder.ObjectValue != null)
        objectValue = holder.ObjectValue;
      if (length != 0)
      {
        FieldInfo[] fieldInfoArray = new FieldInfo[length];
        for (int index = 0; index < length; ++index)
        {
          FieldInfo fieldInfo = sourceArray[length - 1 - index];
          //SerializationFieldInfo serializationFieldInfo = fieldInfo as SerializationFieldInfo;
          //fieldInfoArray[index] = FieldInfo.Equals((FieldInfo) serializationFieldInfo, (FieldInfo) null) ? fieldInfo : serializationFieldInfo.FieldInfo;
        }
        TypedReference typedReference = TypedReference.MakeTypedReference(objectValue, fieldInfoArray);


        if (memberToFix != (FieldInfo)null)
        {
            //RnD
            //memberToFix.SetValueDirect(typedReference, value);
        }
        else
            TypedReference.SetTypedReference(typedReference, value);
      }
      else if (memberToFix != (FieldInfo)null)
        FormatterServices.SerializationSetValue((MemberInfo) memberToFix, objectValue, value);
      if (numArray != null && holder.ObjectValue != null)
        ((Array) holder.ObjectValue).SetValue(objectValue, numArray);
      return true;
    }

    internal void CompleteObject(ObjectHolder holder, bool bObjectFullyComplete)
    {
      FixupHolderList missingElements = holder._missingElements;
      object member = (object) null;
      ObjectHolder holder1 = (ObjectHolder) null;
      int num = 0;
      if (holder.ObjectValue == null)
        throw new SerializationException(SR.Format(SR.Serialization_MissingObject, (object) holder._id));
      if (missingElements == null)
        return;
      if (holder.HasSurrogate || holder.HasISerializable)
      {
        SerializationInfo serInfo = holder._serInfo;
        if (serInfo == null)
          throw new SerializationException(SR.Serialization_InvalidFixupDiscovered);
        if (missingElements != null)
        {
          for (int index = 0; index < missingElements._count; ++index)
          {
            if (missingElements._values[index] != null && this.GetCompletionInfo(missingElements._values[index], out holder1, out member, bObjectFullyComplete))
            {
              object objectValue = holder1.ObjectValue;
            //RnD
            if (this.CanCallGetType(objectValue))
            {
                //serInfo.UpdateValue((string)member, objectValue, objectValue.GetType());
            }
            else
            {
                //serInfo.UpdateValue((string)member, objectValue, typeof(MarshalByRefObject));
            }
              ++num;
              missingElements._values[index] = (FixupHolder) null;
              if (!bObjectFullyComplete)
              {
                holder.DecrementFixupsRemaining(this);
                holder1.RemoveDependency(holder._id);
              }
            }
          }
        }
      }
      else
      {
        for (int index = 0; index < missingElements._count; ++index)
        {
          FixupHolder fixup = missingElements._values[index];
          if (fixup != null && this.GetCompletionInfo(fixup, out holder1, out member, bObjectFullyComplete))
          {
            if (holder1.TypeLoadExceptionReachable)
            {
              holder.TypeLoadException = holder1.TypeLoadException;
              if (holder.Reachable)
                throw new SerializationException(SR.Format(SR.Serialization_TypeLoadFailure, 
                    (object) holder.TypeLoadException.TypeName));
            }
            if (holder.Reachable)
              holder1.Reachable = true;
            switch (fixup._fixupType)
            {
              case 1:
                if (holder.RequiresValueTypeFixup)
                  throw new SerializationException(SR.Serialization_ValueTypeFixup);
                ((Array) holder.ObjectValue).SetValue(holder1.ObjectValue, (int[]) member);
                break;
              case 2:
                MemberInfo memberInfo = (MemberInfo) member;
                if (!(memberInfo is FieldInfo))
                  throw new SerializationException(SR.Serialization_UnableToFixup);
                if (holder.RequiresValueTypeFixup && holder.ValueTypeFixupPerformed)
                {
                  if (!this.DoValueTypeFixup((FieldInfo) memberInfo, holder, holder1.ObjectValue))
                    throw new SerializationException(SR.Serialization_PartialValueTypeFixup);
                }
                else
                  FormatterServices.SerializationSetValue(memberInfo, holder.ObjectValue, holder1.ObjectValue);
                if (holder1.RequiresValueTypeFixup)
                {
                  holder1.ValueTypeFixupPerformed = true;
                  break;
                }
                break;
              default:
                throw new SerializationException(SR.Serialization_UnableToFixup);
            }
            ++num;
            missingElements._values[index] = (FixupHolder) null;
            if (!bObjectFullyComplete)
            {
              holder.DecrementFixupsRemaining(this);
              holder1.RemoveDependency(holder._id);
            }
          }
        }
      }
      this._fixupCount -= (long) num;
      if (missingElements._count != num)
        return;
      holder._missingElements = (FixupHolderList) null;
    }

    private void DoNewlyRegisteredObjectFixups(ObjectHolder holder)
    {
      if (holder.CanObjectValueChange)
        return;
      LongList dependentObjects = holder.DependentObjects;
      if (dependentObjects == null)
        return;
      dependentObjects.StartEnumeration();
      while (dependentObjects.MoveNext())
      {
        ObjectHolder objectHolder = this.FindObjectHolder(dependentObjects.Current);
        objectHolder.DecrementFixupsRemaining(this);
        if (objectHolder.DirectlyDependentObjects == 0)
        {
          if (objectHolder.ObjectValue != null)
            this.CompleteObject(objectHolder, true);
          else
            objectHolder.MarkForCompletionWhenAvailable();
        }
      }
    }

    public virtual object GetObject(long objectID)
    {
      ObjectHolder objectHolder = objectID > 0L ? this.FindObjectHolder(objectID) : throw new ArgumentOutOfRangeException(nameof (objectID), SR.ArgumentOutOfRange_ObjectID);
      return objectHolder == null || objectHolder.CanObjectValueChange ? (object) null : objectHolder.ObjectValue;
    }

    public virtual void RegisterObject(object obj, long objectID)
    {
      this.RegisterObject(obj, objectID, (SerializationInfo) null, 0L, (MemberInfo) null);
    }

    public void RegisterObject(object obj, long objectID, SerializationInfo info)
    {
      this.RegisterObject(obj, objectID, info, 0L, (MemberInfo) null);
    }

    public void RegisterObject(
      object obj,
      long objectID,
      SerializationInfo info,
      long idOfContainingObj,
      MemberInfo member)
    {
      this.RegisterObject(obj, objectID, info, idOfContainingObj, member, (int[]) null);
    }

    internal void RegisterString(
      string obj,
      long objectID,
      SerializationInfo info,
      long idOfContainingObj,
      MemberInfo member)
    {
      this.AddObjectHolder(new ObjectHolder(obj, objectID, info, (ISerializationSurrogate) null, idOfContainingObj, (FieldInfo) member, (int[]) null));
    }

    public void RegisterObject(
      object obj,
      long objectID,
      SerializationInfo info,
      long idOfContainingObj,
      MemberInfo member,
      int[] arrayIndex)
    {
      if (obj == null)
        throw new ArgumentNullException(nameof (obj));
      if (objectID <= 0L)
        throw new ArgumentOutOfRangeException(nameof (objectID), SR.ArgumentOutOfRange_ObjectID);
      if (member != (MemberInfo) null && !(member is FieldInfo))
        throw new SerializationException(SR.Serialization_UnknownMemberInfo);
      ISerializationSurrogate surrogate = (ISerializationSurrogate) null;
      if (this._selector != null)
        surrogate = this._selector.GetSurrogate(this.CanCallGetType(obj) ? obj.GetType() : typeof (MarshalByRefObject),
            this._context, out ISurrogateSelector _);
      if (obj is IDeserializationCallback)
        this.AddOnDeserialization(new DeserializationEventHandler(((IDeserializationCallback) obj).OnDeserialization));
      if (arrayIndex != null)
        arrayIndex = (int[]) arrayIndex.Clone();
      ObjectHolder objectHolder = this.FindObjectHolder(objectID);
      if (objectHolder == null)
      {
        ObjectHolder holder = new ObjectHolder(obj, objectID, info, surrogate, idOfContainingObj,
            (FieldInfo) member, arrayIndex);
        this.AddObjectHolder(holder);
        if (holder.RequiresDelayedFixup)
          this.SpecialFixupObjects.Add(holder);
        this.AddOnDeserialized(obj);
      }
      else
      {
        if (objectHolder.ObjectValue != null)
          throw new SerializationException(SR.Serialization_RegisterTwice);
        objectHolder.UpdateData(obj, info, surrogate, idOfContainingObj, (FieldInfo) member, arrayIndex, this);
        if (objectHolder.DirectlyDependentObjects > 0)
          this.CompleteObject(objectHolder, false);
        if (objectHolder.RequiresDelayedFixup)
          this.SpecialFixupObjects.Add(objectHolder);
        if (objectHolder.CompletelyFixed)
        {
          this.DoNewlyRegisteredObjectFixups(objectHolder);
          objectHolder.DependentObjects = (LongList) null;
        }
        if (objectHolder.TotalDependentObjects > 0)
          this.AddOnDeserialized(obj);
        else
          this.RaiseOnDeserializedEvent(obj);
      }
    }

    internal void CompleteISerializableObject(
      object obj,
      SerializationInfo info,
      StreamingContext context)
    {
      if (obj == null)
        throw new ArgumentNullException(nameof (obj));
      Type type = obj is ISerializable ? obj.GetType() : throw new ArgumentException(SR.Serialization_NotISer);
      ConstructorInfo deserializationConstructor;
      try
      {
        deserializationConstructor = ObjectManager.GetDeserializationConstructor(type);
      }
      catch (Exception ex)
      {
        throw new SerializationException(SR.Format(SR.Serialization_ConstructorNotFound, (object) type), ex);
      }
      ((MethodBase) deserializationConstructor).Invoke(obj, new object[2]
      {
        (object) info,
        (object) context
      });
    }

    internal static ConstructorInfo GetDeserializationConstructor(Type t)
    {
      foreach (ConstructorInfo constructor in t.GetConstructors((BindingFlags) 54))
      {
        ParameterInfo[] parameters = ((MethodBase) constructor).GetParameters();
        if (parameters.Length == 2 && parameters[0].ParameterType == typeof (SerializationInfo)
                    && parameters[1].ParameterType == typeof (StreamingContext))
          return constructor;
      }
      throw new SerializationException(SR.Format(SR.Serialization_ConstructorNotFound, (object) t.FullName));
    }

    public virtual void DoFixups()
    {
      int num = -1;
      while (num != 0)
      {
        num = 0;
        ObjectHolderListEnumerator fixupEnumerator = this.SpecialFixupObjects.GetFixupEnumerator();
        while (fixupEnumerator.MoveNext())
        {
          ObjectHolder current = fixupEnumerator.Current;
          if (current.ObjectValue == null)
            throw new SerializationException(SR.Format(SR.Serialization_ObjectNotSupplied, (object) current._id));
          if (current.TotalDependentObjects == 0)
          {
            if (current.RequiresSerInfoFixup)
            {
              this.FixupSpecialObject(current);
              ++num;
            }
            else if (!current.IsIncompleteObjectReference)
              this.CompleteObject(current, true);
            if (current.IsIncompleteObjectReference && this.ResolveObjectReference(current))
              ++num;
          }
        }
      }
      if (this._fixupCount == 0L)
      {
        if (this.TopObject is TypeLoadExceptionHolder)
          throw new SerializationException(SR.Format(SR.Serialization_TypeLoadFailure, (object) ((TypeLoadExceptionHolder) this.TopObject).TypeName));
      }
      else
      {
        for (int index = 0; index < this._objects.Length; ++index)
        {
          for (ObjectHolder next = this._objects[index]; next != null; next = next._next)
          {
            if (next.TotalDependentObjects > 0)
              this.CompleteObject(next, true);
          }
          if (this._fixupCount == 0L)
            return;
        }
        throw new SerializationException(SR.Serialization_IncorrectNumberOfFixups);
      }
    }

    private void RegisterFixup(FixupHolder fixup, long objectToBeFixed, long objectRequired)
    {
      ObjectHolder createObjectHolder = this.FindOrCreateObjectHolder(objectToBeFixed);
      if (createObjectHolder.RequiresSerInfoFixup && fixup._fixupType == 2)
        throw new SerializationException(SR.Serialization_InvalidFixupType);
      createObjectHolder.AddFixup(fixup, this);
      this.FindOrCreateObjectHolder(objectRequired).AddDependency(objectToBeFixed);
      ++this._fixupCount;
    }

    public virtual void RecordFixup(long objectToBeFixed, MemberInfo member, long objectRequired)
    {
      if (objectToBeFixed <= 0L || objectRequired <= 0L)
        throw new ArgumentOutOfRangeException(objectToBeFixed <= 0L ? nameof (objectToBeFixed) : nameof (objectRequired), SR.Serialization_IdTooSmall);
      if (member != (MemberInfo)null)
        throw new ArgumentNullException(nameof (member));
      if (!(member is FieldInfo))
        throw new SerializationException(SR.Format(SR.Serialization_InvalidType, (object) member.GetType().ToString()));
      this.RegisterFixup(new FixupHolder(objectRequired, (object) member, 2), objectToBeFixed, objectRequired);
    }

    public virtual void RecordDelayedFixup(
      long objectToBeFixed,
      string memberName,
      long objectRequired)
    {
      if (objectToBeFixed <= 0L || objectRequired <= 0L)
        throw new ArgumentOutOfRangeException(objectToBeFixed <= 0L ? nameof (objectToBeFixed) : nameof (objectRequired), SR.Serialization_IdTooSmall);
      if (memberName == null)
        throw new ArgumentNullException(nameof (memberName));
      this.RegisterFixup(new FixupHolder(objectRequired, (object) memberName, 4), objectToBeFixed, objectRequired);
    }

    public virtual void RecordArrayElementFixup(
      long arrayToBeFixed,
      int index,
      long objectRequired)
    {
      int[] indices = new int[1]{ index };
      this.RecordArrayElementFixup(arrayToBeFixed, indices, objectRequired);
    }

    public virtual void RecordArrayElementFixup(
      long arrayToBeFixed,
      int[] indices,
      long objectRequired)
    {
      if (arrayToBeFixed <= 0L || objectRequired <= 0L)
        throw new ArgumentOutOfRangeException(arrayToBeFixed <= 0L ? nameof (arrayToBeFixed) : nameof (objectRequired), SR.Serialization_IdTooSmall);
      if (indices == null)
        throw new ArgumentNullException(nameof (indices));
      this.RegisterFixup(new FixupHolder(objectRequired, (object) indices, 1), arrayToBeFixed, objectRequired);
    }

    public virtual void RaiseDeserializationEvent()
    {
      SerializationEventHandler deserializedHandler = this._onDeserializedHandler;
      if (deserializedHandler != null)
        deserializedHandler(this._context);
      DeserializationEventHandler deserializationHandler = this._onDeserializationHandler;
      if (deserializationHandler == null)
        return;
      deserializationHandler((object) null);
    }

    internal virtual void AddOnDeserialization(DeserializationEventHandler handler)
    {
      this._onDeserializationHandler += handler;
    }

    internal virtual void AddOnDeserialized(object obj)
    {
      this._onDeserializedHandler = SerializationEventsCache.GetSerializationEventsForType(obj.GetType()).AddOnDeserialized(obj, this._onDeserializedHandler);
    }

    internal virtual void RaiseOnDeserializedEvent(object obj)
    {
      SerializationEventsCache.GetSerializationEventsForType(obj.GetType()).InvokeOnDeserialized(obj, this._context);
    }

    public void RaiseOnDeserializingEvent(object obj)
    {
      SerializationEventsCache.GetSerializationEventsForType(obj.GetType()).InvokeOnDeserializing(obj, this._context);
    }
            
  }
}
