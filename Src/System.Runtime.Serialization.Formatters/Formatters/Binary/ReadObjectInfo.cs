// Decompiled with JetBrains decompiler
// Type: System.Runtime.Serialization.Formatters.Binary.ReadObjectInfo
// Assembly: System.Runtime.Serialization.Formatters, Version=4.0.3.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 59FEC222-A9B8-4506-99C2-FAFFD4E25D28
// Assembly location: C:\Users\Admin\Desktop\RE\1\System.Runtime.Serialization.Formatters.dll

using System.Collections.Generic;
using System.Reflection;
using System.Threading;


namespace System.Runtime.Serialization.Formatters.Binary
{
  internal sealed class ReadObjectInfo
  {
    internal int _objectInfoId;
    internal static int _readObjectInfoCounter;
    internal Type _objectType;
    internal ObjectManager _objectManager;
    internal int _count;
    internal bool _isSi;
    internal bool _isTyped;
    internal bool _isSimpleAssembly;
    internal SerObjectInfoCache _cache;
    internal string[] _wireMemberNames;
    internal Type[] _wireMemberTypes;
    private int _lastPosition;
    internal ISerializationSurrogate _serializationSurrogate;
    internal StreamingContext _context;
    internal List<Type> _memberTypesList;
    internal SerObjectInfoInit _serObjectInfoInit;
    internal IFormatterConverter _formatterConverter;

    internal ReadObjectInfo()
    {
    }

    internal void ObjectEnd()
    {
    }

    internal void PrepareForReuse() => this._lastPosition = 0;

    internal static ReadObjectInfo Create(
      Type objectType,
      ISurrogateSelector surrogateSelector,
      StreamingContext context,
      ObjectManager objectManager,
      SerObjectInfoInit serObjectInfoInit,
      IFormatterConverter converter,
      bool bSimpleAssembly)
    {
      ReadObjectInfo objectInfo = ReadObjectInfo.GetObjectInfo(serObjectInfoInit);
      objectInfo.Init(objectType, surrogateSelector, context, objectManager, serObjectInfoInit, converter, bSimpleAssembly);
      return objectInfo;
    }

    internal void Init(
      Type objectType,
      ISurrogateSelector surrogateSelector,
      StreamingContext context,
      ObjectManager objectManager,
      SerObjectInfoInit serObjectInfoInit,
      IFormatterConverter converter,
      bool bSimpleAssembly)
    {
      this._objectType = objectType;
      this._objectManager = objectManager;
      this._context = context;
      this._serObjectInfoInit = serObjectInfoInit;
      this._formatterConverter = converter;
      this._isSimpleAssembly = bSimpleAssembly;
      this.InitReadConstructor(objectType, surrogateSelector, context);
    }

    internal static ReadObjectInfo Create(
      Type objectType,
      string[] memberNames,
      Type[] memberTypes,
      ISurrogateSelector surrogateSelector,
      StreamingContext context,
      ObjectManager objectManager,
      SerObjectInfoInit serObjectInfoInit,
      IFormatterConverter converter,
      bool bSimpleAssembly)
    {
      ReadObjectInfo objectInfo = ReadObjectInfo.GetObjectInfo(serObjectInfoInit);
      objectInfo.Init(objectType, memberNames, memberTypes, surrogateSelector, context, objectManager, serObjectInfoInit, converter, bSimpleAssembly);
      return objectInfo;
    }

    internal void Init(
      Type objectType,
      string[] memberNames,
      Type[] memberTypes,
      ISurrogateSelector surrogateSelector,
      StreamingContext context,
      ObjectManager objectManager,
      SerObjectInfoInit serObjectInfoInit,
      IFormatterConverter converter,
      bool bSimpleAssembly)
    {
      this._objectType = objectType;
      this._objectManager = objectManager;
      this._wireMemberNames = memberNames;
      this._wireMemberTypes = memberTypes;
      this._context = context;
      this._serObjectInfoInit = serObjectInfoInit;
      this._formatterConverter = converter;
      this._isSimpleAssembly = bSimpleAssembly;
      if (memberTypes != null)
        this._isTyped = true;
      if (!(objectType != (Type) null))
        return;
      this.InitReadConstructor(objectType, surrogateSelector, context);
    }

    private void InitReadConstructor(
      Type objectType,
      ISurrogateSelector surrogateSelector,
      StreamingContext context)
    {
      if (objectType.IsArray)
      {
        this.InitNoMembers();
      }
      else
      {
        ISurrogateSelector selector = (ISurrogateSelector) null;
        if (surrogateSelector != null)
          this._serializationSurrogate = surrogateSelector.GetSurrogate(objectType, context, out selector);
        if (this._serializationSurrogate != null)
          this._isSi = true;
        else if ((object) objectType != (object) Converter.s_typeofObject && Converter.s_typeofISerializable.IsAssignableFrom(objectType))
          this._isSi = true;
        if (this._isSi)
          this.InitSiRead();
        else
          this.InitMemberInfo();
      }
    }

    private void InitSiRead()
    {
      if (this._memberTypesList == null)
        return;
      this._memberTypesList = new List<Type>(20);
    }

    private void InitNoMembers() => this._cache = new SerObjectInfoCache(this._objectType);

    private void InitMemberInfo()
    {
      this._cache = new SerObjectInfoCache(this._objectType);
      this._cache._memberInfos = FormatterServices.GetSerializableMembers(this._objectType, this._context);
      this._count = this._cache._memberInfos.Length;
      this._cache._memberNames = new string[this._count];
      this._cache._memberTypes = new Type[this._count];
      for (int index = 0; index < this._count; ++index)
      {
        this._cache._memberNames[index] = this._cache._memberInfos[index].Name;
        this._cache._memberTypes[index] = this.GetMemberType(this._cache._memberInfos[index]);
      }
      this._isTyped = true;
    }

    internal MemberInfo GetMemberInfo(string name)
    {
      if (this._cache == null)
        return (MemberInfo) null;
      if (this._isSi)
        throw new SerializationException(SR.Format(SR.Serialization_MemberInfo, (object) (this._objectType.ToString() + " " + name)));
      if (this._cache._memberInfos == null)
        throw new SerializationException(SR.Format(SR.Serialization_NoMemberInfo, (object) (this._objectType.ToString() + " " + name)));
      int index = this.Position(name);
      return index == -1 ? (MemberInfo) null : this._cache._memberInfos[index];
    }

    internal Type GetType(string name)
    {
      int index = this.Position(name);
      if (index == -1)
        return (Type) null;
      Type type = this._isTyped ? this._cache._memberTypes[index] : this._memberTypesList[index];
      return !(type == (Type) null) ? type : throw new SerializationException(SR.Format(SR.Serialization_ISerializableTypes, (object) (this._objectType.ToString() + " " + name)));
    }

    internal void AddValue(
      string name,
      object value,
      ref SerializationInfo si,
      ref object[] memberData)
    {
      if (this._isSi)
      {
        si.AddValue(name, value);
      }
      else
      {
        int index = this.Position(name);
        if (index == -1)
          return;
        memberData[index] = value;
      }
    }

    internal void InitDataStore(ref SerializationInfo si, ref object[] memberData)
    {
      if (this._isSi)
      {
        if (si != null)
          return;
        si = new SerializationInfo(this._objectType, this._formatterConverter);
      }
      else
      {
        if (memberData != null || this._cache == null)
          return;
        memberData = new object[this._cache._memberNames.Length];
      }
    }

    internal void RecordFixup(long objectId, string name, long idRef)
    {
      if (this._isSi)
      {
        this._objectManager.RecordDelayedFixup(objectId, name, idRef);
      }
      else
      {
        int index = this.Position(name);
        if (index == -1)
          return;
        this._objectManager.RecordFixup(objectId, this._cache._memberInfos[index], idRef);
      }
    }

    internal void PopulateObjectMembers(object obj, object[] memberData)
    {
      if (this._isSi || memberData == null)
        return;
      FormatterServices.PopulateObjectMembers(obj, this._cache._memberInfos, memberData);
    }

    private int Position(string name)
    {
      if (this._cache == null)
        return -1;
      if (this._cache._memberNames.Length != 0 && this._cache._memberNames[this._lastPosition].Equals(name) || ++this._lastPosition < this._cache._memberNames.Length && this._cache._memberNames[this._lastPosition].Equals(name))
        return this._lastPosition;
      for (int index = 0; index < this._cache._memberNames.Length; ++index)
      {
        if (this._cache._memberNames[index].Equals(name))
        {
          this._lastPosition = index;
          return this._lastPosition;
        }
      }
      this._lastPosition = 0;
      return -1;
    }

    internal Type[] GetMemberTypes(string[] inMemberNames, Type objectType)
    {
      if (this._isSi)
        throw new SerializationException(SR.Format(SR.Serialization_ISerializableTypes, (object) objectType));
      if (this._cache == null)
        return (Type[]) null;
      if (this._cache._memberTypes == null)
      {
        this._cache._memberTypes = new Type[this._count];
        for (int index = 0; index < this._count; ++index)
          this._cache._memberTypes[index] = this.GetMemberType(this._cache._memberInfos[index]);
      }
      bool flag1 = false;
      if (inMemberNames.Length < this._cache._memberInfos.Length)
        flag1 = true;
      Type[] memberTypes = new Type[this._cache._memberInfos.Length];
      for (int index1 = 0; index1 < this._cache._memberInfos.Length; ++index1)
      {
        if (!flag1 && inMemberNames[index1].Equals(this._cache._memberInfos[index1].Name))
        {
          memberTypes[index1] = this._cache._memberTypes[index1];
        }
        else
        {
          bool flag2 = false;
          for (int index2 = 0; index2 < inMemberNames.Length; ++index2)
          {
            if (this._cache._memberInfos[index1].Name.Equals(inMemberNames[index2]))
            {
              memberTypes[index1] = this._cache._memberTypes[index1];
              flag2 = true;
              break;
            }
          }
          if (!flag2 && !this._isSimpleAssembly 
                        && CustomAttributeExtensions.GetCustomAttribute(this._cache._memberInfos[index1],
                        typeof (OptionalFieldAttribute), false) == null)
            throw new SerializationException(SR.Format(SR.Serialization_MissingMember,
                (object) this._cache._memberNames[index1], (object) objectType, 
                (object) typeof (OptionalFieldAttribute).FullName));
        }
      }
      return memberTypes;
    }

    internal Type GetMemberType(MemberInfo objMember)
    {
      return objMember is FieldInfo ? ((FieldInfo) objMember).FieldType : throw new SerializationException(SR.Format(SR.Serialization_SerMemberInfo, (object) objMember.GetType()));
    }

    private static ReadObjectInfo GetObjectInfo(SerObjectInfoInit serObjectInfoInit)
    {
      return new ReadObjectInfo()
      {
        _objectInfoId = Interlocked.Increment(ref ReadObjectInfo._readObjectInfoCounter)
      };
    }
  }
}
