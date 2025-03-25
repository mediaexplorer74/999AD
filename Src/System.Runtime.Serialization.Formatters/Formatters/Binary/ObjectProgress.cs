// Decompiled with JetBrains decompiler
// Type: System.Runtime.Serialization.Formatters.Binary.ObjectProgress
// Assembly: System.Runtime.Serialization.Formatters, Version=4.0.3.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 59FEC222-A9B8-4506-99C2-FAFFD4E25D28
// Assembly location: C:\Users\Admin\Desktop\RE\1\System.Runtime.Serialization.Formatters.dll


namespace System.Runtime.Serialization.Formatters.Binary
{
  internal sealed class ObjectProgress
  {
    internal bool _isInitial;
    internal int _count;
    internal BinaryTypeEnum _expectedType = BinaryTypeEnum.ObjectUrt;
    internal object _expectedTypeInformation;
    internal string _name;
    internal InternalObjectTypeE _objectTypeEnum;
    internal InternalMemberTypeE _memberTypeEnum;
    internal InternalMemberValueE _memberValueEnum;
    internal Type _dtType;
    internal int _numItems;
    internal BinaryTypeEnum _binaryTypeEnum;
    internal object _typeInformation;
    internal int _memberLength;
    internal BinaryTypeEnum[] _binaryTypeEnumA;
    internal object[] _typeInformationA;
    internal string[] _memberNames;
    internal Type[] _memberTypes;
    internal ParseRecord _pr = new ParseRecord();

    internal ObjectProgress()
    {
    }

    internal void Init()
    {
      this._isInitial = false;
      this._count = 0;
      this._expectedType = BinaryTypeEnum.ObjectUrt;
      this._expectedTypeInformation = (object) null;
      this._name = (string) null;
      this._objectTypeEnum = InternalObjectTypeE.Empty;
      this._memberTypeEnum = InternalMemberTypeE.Empty;
      this._memberValueEnum = InternalMemberValueE.Empty;
      this._dtType = (Type) null;
      this._numItems = 0;
      this._typeInformation = (object) null;
      this._memberLength = 0;
      this._binaryTypeEnumA = (BinaryTypeEnum[]) null;
      this._typeInformationA = (object[]) null;
      this._memberNames = (string[]) null;
      this._memberTypes = (Type[]) null;
      this._pr.Init();
    }

    internal void ArrayCountIncrement(int value) => this._count += value;

    internal bool GetNext(out BinaryTypeEnum outBinaryTypeEnum, out object outTypeInformation)
    {
      outBinaryTypeEnum = BinaryTypeEnum.Primitive;
      outTypeInformation = (object) null;
      if (this._objectTypeEnum == InternalObjectTypeE.Array)
      {
        if (this._count == this._numItems)
          return false;
        outBinaryTypeEnum = this._binaryTypeEnum;
        outTypeInformation = this._typeInformation;
        if (this._count == 0)
          this._isInitial = false;
        ++this._count;
        return true;
      }
      if (this._count == this._memberLength && !this._isInitial)
        return false;
      outBinaryTypeEnum = this._binaryTypeEnumA[this._count];
      outTypeInformation = this._typeInformationA[this._count];
      if (this._count == 0)
        this._isInitial = false;
      this._name = this._memberNames[this._count];
      this._dtType = this._memberTypes[this._count];
      ++this._count;
      return true;
    }
  }
}
