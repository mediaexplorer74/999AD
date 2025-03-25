// Decompiled with JetBrains decompiler
// Type: System.Runtime.Serialization.Formatters.Binary.ParseRecord
// Assembly: System.Runtime.Serialization.Formatters, Version=4.0.3.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 59FEC222-A9B8-4506-99C2-FAFFD4E25D28
// Assembly location: C:\Users\Admin\Desktop\RE\1\System.Runtime.Serialization.Formatters.dll


namespace System.Runtime.Serialization.Formatters.Binary
{
  internal sealed class ParseRecord
  {
    internal InternalParseTypeE _parseTypeEnum;
    internal InternalObjectTypeE _objectTypeEnum;
    internal InternalArrayTypeE _arrayTypeEnum;
    internal InternalMemberTypeE _memberTypeEnum;
    internal InternalMemberValueE _memberValueEnum;
    internal InternalObjectPositionE _objectPositionEnum;
    internal string _name;
    internal string _value;
    internal object _varValue;
    internal string _keyDt;
    internal Type _dtType;
    internal InternalPrimitiveTypeE _dtTypeCode;
    internal long _objectId;
    internal long _idRef;
    internal string _arrayElementTypeString;
    internal Type _arrayElementType;
    internal bool _isArrayVariant;
    internal InternalPrimitiveTypeE _arrayElementTypeCode;
    internal int _rank;
    internal int[] _lengthA;
    internal int[] _lowerBoundA;
    internal int[] _indexMap;
    internal int _memberIndex;
    internal int _linearlength;
    internal int[] _rectangularMap;
    internal bool _isLowerBound;
    internal ReadObjectInfo _objectInfo;
    internal bool _isValueTypeFixup;
    internal object _newObj;
    internal object[] _objectA;
    internal PrimitiveArray _primitiveArray;
    internal bool _isRegistered;
    internal object[] _memberData;
    internal SerializationInfo _si;
    internal int _consecutiveNullArrayEntryCount;

    internal ParseRecord()
    {
    }

    internal void Init()
    {
      this._parseTypeEnum = InternalParseTypeE.Empty;
      this._objectTypeEnum = InternalObjectTypeE.Empty;
      this._arrayTypeEnum = InternalArrayTypeE.Empty;
      this._memberTypeEnum = InternalMemberTypeE.Empty;
      this._memberValueEnum = InternalMemberValueE.Empty;
      this._objectPositionEnum = InternalObjectPositionE.Empty;
      this._name = (string) null;
      this._value = (string) null;
      this._keyDt = (string) null;
      this._dtType = (Type) null;
      this._dtTypeCode = InternalPrimitiveTypeE.Invalid;
      this._objectId = 0L;
      this._idRef = 0L;
      this._arrayElementTypeString = (string) null;
      this._arrayElementType = (Type) null;
      this._isArrayVariant = false;
      this._arrayElementTypeCode = InternalPrimitiveTypeE.Invalid;
      this._rank = 0;
      this._lengthA = (int[]) null;
      this._lowerBoundA = (int[]) null;
      this._indexMap = (int[]) null;
      this._memberIndex = 0;
      this._linearlength = 0;
      this._rectangularMap = (int[]) null;
      this._isLowerBound = false;
      this._isValueTypeFixup = false;
      this._newObj = (object) null;
      this._objectA = (object[]) null;
      this._primitiveArray = (PrimitiveArray) null;
      this._objectInfo = (ReadObjectInfo) null;
      this._isRegistered = false;
      this._memberData = (object[]) null;
      this._si = (SerializationInfo) null;
      this._consecutiveNullArrayEntryCount = 0;
    }
  }
}
