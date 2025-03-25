// Decompiled with JetBrains decompiler
// Type: System.Runtime.Serialization.ObjectHolder
// Assembly: System.Runtime.Serialization.Formatters, Version=4.0.3.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 59FEC222-A9B8-4506-99C2-FAFFD4E25D28
// Assembly location: C:\Users\Admin\Desktop\RE\1\System.Runtime.Serialization.Formatters.dll

using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;


namespace System.Runtime.Serialization
{
  internal sealed class ObjectHolder
  {
    internal const int IncompleteObjectReference = 1;
    internal const int HAS_ISERIALIZABLE = 2;
    internal const int HAS_SURROGATE = 4;
    internal const int REQUIRES_VALUETYPE_FIXUP = 8;
    internal const int REQUIRES_DELAYED_FIXUP = 7;
    internal const int SER_INFO_FIXED = 16384;
    internal const int VALUETYPE_FIXUP_PERFORMED = 32768;
    private object _object;
    internal readonly long _id;
    private int _missingElementsRemaining;
    private int _missingDecendents;
    internal SerializationInfo _serInfo;
    internal ISerializationSurrogate _surrogate;
    internal FixupHolderList _missingElements;
    internal LongList _dependentObjects;
    internal ObjectHolder _next;
    internal int _flags;
    private bool _markForFixupWhenAvailable;
    private ValueTypeFixupInfo _valueFixup;
    private TypeLoadExceptionHolder _typeLoad;
    private bool _reachable;

    internal ObjectHolder(long objID)
      : this((string) null, objID, (SerializationInfo) null, (ISerializationSurrogate) null, 0L, (FieldInfo) null, (int[]) null)
    {
    }

    internal ObjectHolder(
      object obj,
      long objID,
      SerializationInfo info,
      ISerializationSurrogate surrogate,
      long idOfContainingObj,
      FieldInfo field,
      int[] arrayIndex)
    {
      this._object = obj;
      this._id = objID;
      this._flags = 0;
      this._missingElementsRemaining = 0;
      this._missingDecendents = 0;
      if (idOfContainingObj != 0L && (field != null && field.FieldType.GetTypeInfo().IsValueType
                || arrayIndex != null))
      this._dependentObjects = (LongList) null;
      this._next = (ObjectHolder) null;
      this._serInfo = info;
      this._surrogate = surrogate;
      this._markForFixupWhenAvailable = false;
      if (obj is TypeLoadExceptionHolder)
        this._typeLoad = (TypeLoadExceptionHolder) obj;
            if (idOfContainingObj != 0L && (field != null && field.FieldType.GetTypeInfo().IsValueType 
                || arrayIndex != null))
      {
        if (idOfContainingObj == objID)
          throw new SerializationException(SR.Serialization_ParentChildIdentical);
        this._valueFixup = new ValueTypeFixupInfo(idOfContainingObj, field, arrayIndex);
      }
      this.SetFlags();
    }

    internal ObjectHolder(
      string obj,
      long objID,
      SerializationInfo info,
      ISerializationSurrogate surrogate,
      long idOfContainingObj,
      FieldInfo field,
      int[] arrayIndex)
    {
      this._object = (object) obj;
      this._id = objID;
      this._flags = 0;
      this._missingElementsRemaining = 0;
      this._missingDecendents = 0;
      this._dependentObjects = (LongList) null;
      this._next = (ObjectHolder) null;
      this._serInfo = info;
      this._surrogate = surrogate;
      this._markForFixupWhenAvailable = false;
      if (idOfContainingObj != 0L && arrayIndex != null)
        this._valueFixup = new ValueTypeFixupInfo(idOfContainingObj, field, arrayIndex);
      if (this._valueFixup == null)
        return;
      this._flags |= 8;
    }

    private void IncrementDescendentFixups(int amount) => this._missingDecendents += amount;

    internal void DecrementFixupsRemaining(ObjectManager manager)
    {
      --this._missingElementsRemaining;
      if (!this.RequiresValueTypeFixup)
        return;
      this.UpdateDescendentDependencyChain(-1, manager);
    }

    internal void RemoveDependency(long id) => this._dependentObjects.RemoveElement(id);

    internal void AddFixup(FixupHolder fixup, ObjectManager manager)
    {
      if (this._missingElements == null)
        this._missingElements = new FixupHolderList();
      this._missingElements.Add(fixup);
      ++this._missingElementsRemaining;
      if (!this.RequiresValueTypeFixup)
        return;
      this.UpdateDescendentDependencyChain(1, manager);
    }

    private void UpdateDescendentDependencyChain(int amount, ObjectManager manager)
    {
      ObjectHolder objectHolder = this;
      do
      {
        objectHolder = manager.FindOrCreateObjectHolder(objectHolder.ContainerID);
        objectHolder.IncrementDescendentFixups(amount);
      }
      while (objectHolder.RequiresValueTypeFixup);
    }

    internal void AddDependency(long dependentObject)
    {
      if (this._dependentObjects == null)
        this._dependentObjects = new LongList();
      this._dependentObjects.Add(dependentObject);
    }

    internal void UpdateData(
      object obj,
      SerializationInfo info,
      ISerializationSurrogate surrogate,
      long idOfContainer,
      FieldInfo field,
      int[] arrayIndex,
      ObjectManager manager)
    {
      this.SetObjectValue(obj, manager);
      this._serInfo = info;
      this._surrogate = surrogate;
      if (idOfContainer != 0L && (field != null && field.FieldType.GetTypeInfo().IsValueType || arrayIndex != null))
      {
        if (idOfContainer == this._id)
          throw new SerializationException(SR.Serialization_ParentChildIdentical);
        this._valueFixup = new ValueTypeFixupInfo(idOfContainer, field, arrayIndex);
      }
      this.SetFlags();
      if (!this.RequiresValueTypeFixup)
        return;
      this.UpdateDescendentDependencyChain(this._missingElementsRemaining, manager);
    }

    internal void MarkForCompletionWhenAvailable() => this._markForFixupWhenAvailable = true;

    internal void SetFlags()
    {
      if (this._object is IObjectReference)
        this._flags |= 1;
      this._flags &= -7;
      if (this._surrogate != null)
        this._flags |= 4;
      else if (this._object is ISerializable)
        this._flags |= 2;
      if (this._valueFixup == null)
        return;
      this._flags |= 8;
    }

    internal bool IsIncompleteObjectReference
    {
      get => (this._flags & 1) != 0;
      set
      {
        if (value)
          this._flags |= 1;
        else
          this._flags &= -2;
      }
    }

    internal bool RequiresDelayedFixup => (this._flags & 7) != 0;

    internal bool RequiresValueTypeFixup => (this._flags & 8) != 0;

    internal bool ValueTypeFixupPerformed
    {
      get
      {
        if ((this._flags & 32768) != 0)
          return true;
        if (this._object == null)
          return false;
        return this._dependentObjects == null || this._dependentObjects.Count == 0;
      }
      set
      {
        if (!value)
          return;
        this._flags |= 32768;
      }
    }

    internal bool HasISerializable => (this._flags & 2) != 0;

    internal bool HasSurrogate => (this._flags & 4) != 0;

    internal bool CanSurrogatedObjectValueChange
    {
      get
      {
        return this._surrogate == null || this._surrogate.GetType() != typeof (SurrogateForCyclicalReference);
      }
    }

    internal bool CanObjectValueChange
    {
      get
      {
        if (this.IsIncompleteObjectReference)
          return true;
        return this.HasSurrogate && this.CanSurrogatedObjectValueChange;
      }
    }

    internal int DirectlyDependentObjects => this._missingElementsRemaining;

    internal int TotalDependentObjects => this._missingElementsRemaining + this._missingDecendents;

    internal bool Reachable
    {
      get => this._reachable;
      set => this._reachable = value;
    }

    internal bool TypeLoadExceptionReachable => this._typeLoad != null;

    internal TypeLoadExceptionHolder TypeLoadException
    {
      get => this._typeLoad;
      set => this._typeLoad = value;
    }

    internal object ObjectValue => this._object;

    internal void SetObjectValue(object obj, ObjectManager manager)
    {
      this._object = obj;
      if (obj == manager.TopObject)
        this._reachable = true;
      if (obj is TypeLoadExceptionHolder)
        this._typeLoad = (TypeLoadExceptionHolder) obj;
      if (!this._markForFixupWhenAvailable)
        return;
      manager.CompleteObject(this, true);
    }

    internal SerializationInfo SerializationInfo
    {
      get => this._serInfo;
      set => this._serInfo = value;
    }

    internal ISerializationSurrogate Surrogate => this._surrogate;

    internal LongList DependentObjects
    {
      get => this._dependentObjects;
      set => this._dependentObjects = value;
    }

    internal bool RequiresSerInfoFixup
    {
      get => ((this._flags & 4) != 0 || (this._flags & 2) != 0) && (this._flags & 16384) == 0;
      set
      {
        if (!value)
          this._flags |= 16384;
        else
          this._flags &= -16385;
      }
    }

    internal ValueTypeFixupInfo ValueFixup => this._valueFixup;

    internal bool CompletelyFixed
    {
      get => !this.RequiresSerInfoFixup && !this.IsIncompleteObjectReference;
    }

    internal long ContainerID => this._valueFixup == null ? 0L : this._valueFixup.ContainerID;
  }
}
