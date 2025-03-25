// Decompiled with JetBrains decompiler
// Type: System.Runtime.Serialization.Formatters.Binary.ObjectMap
// Assembly: System.Runtime.Serialization.Formatters, Version=4.0.3.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 59FEC222-A9B8-4506-99C2-FAFFD4E25D28
// Assembly location: C:\Users\Admin\Desktop\RE\1\System.Runtime.Serialization.Formatters.dll


namespace System.Runtime.Serialization.Formatters.Binary
{
  internal sealed class ObjectMap
  {
    internal string _objectName;
    internal Type _objectType;
    internal BinaryTypeEnum[] _binaryTypeEnumA;
    internal object[] _typeInformationA;
    internal Type[] _memberTypes;
    internal string[] _memberNames;
    internal ReadObjectInfo _objectInfo;
    internal bool _isInitObjectInfo = true;
    internal ObjectReader _objectReader;
    internal int _objectId;
    internal BinaryAssemblyInfo _assemblyInfo;

    internal ObjectMap(
      string objectName,
      Type objectType,
      string[] memberNames,
      ObjectReader objectReader,
      int objectId,
      BinaryAssemblyInfo assemblyInfo)
    {
      this._objectName = objectName;
      this._objectType = objectType;
      this._memberNames = memberNames;
      this._objectReader = objectReader;
      this._objectId = objectId;
      this._assemblyInfo = assemblyInfo;
      this._objectInfo = objectReader.CreateReadObjectInfo(objectType);
      this._memberTypes = this._objectInfo.GetMemberTypes(memberNames, objectType);
      this._binaryTypeEnumA = new BinaryTypeEnum[this._memberTypes.Length];
      this._typeInformationA = new object[this._memberTypes.Length];
      for (int index = 0; index < this._memberTypes.Length; ++index)
      {
        object typeInformation = (object) null;
        BinaryTypeEnum parserBinaryTypeInfo = BinaryTypeConverter.GetParserBinaryTypeInfo(this._memberTypes[index], out typeInformation);
        this._binaryTypeEnumA[index] = parserBinaryTypeInfo;
        this._typeInformationA[index] = typeInformation;
      }
    }

    internal ObjectMap(
      string objectName,
      string[] memberNames,
      BinaryTypeEnum[] binaryTypeEnumA,
      object[] typeInformationA,
      int[] memberAssemIds,
      ObjectReader objectReader,
      int objectId,
      BinaryAssemblyInfo assemblyInfo,
      SizedArray assemIdToAssemblyTable)
    {
      this._objectName = objectName;
      this._memberNames = memberNames;
      this._binaryTypeEnumA = binaryTypeEnumA;
      this._typeInformationA = typeInformationA;
      this._objectReader = objectReader;
      this._objectId = objectId;
      this._assemblyInfo = assemblyInfo;
      if (assemblyInfo == null)
        throw new SerializationException(SR.Format(SR.Serialization_Assembly, (object) objectName));
      this._objectType = objectReader.GetType(assemblyInfo, objectName);
      this._memberTypes = new Type[memberNames.Length];
      for (int index = 0; index < memberNames.Length; ++index)
      {
        Type type;
        BinaryTypeConverter.TypeFromInfo(binaryTypeEnumA[index], typeInformationA[index], objectReader, (BinaryAssemblyInfo) assemIdToAssemblyTable[memberAssemIds[index]], out InternalPrimitiveTypeE _, out string _, out type, out bool _);
        this._memberTypes[index] = type;
      }
      this._objectInfo = objectReader.CreateReadObjectInfo(this._objectType, memberNames, (Type[]) null);
      if (this._objectInfo._isSi)
        return;
      this._objectInfo.GetMemberTypes(memberNames, this._objectInfo._objectType);
    }

    internal ReadObjectInfo CreateObjectInfo(ref SerializationInfo si, ref object[] memberData)
    {
      if (this._isInitObjectInfo)
      {
        this._isInitObjectInfo = false;
        this._objectInfo.InitDataStore(ref si, ref memberData);
        return this._objectInfo;
      }
      this._objectInfo.PrepareForReuse();
      this._objectInfo.InitDataStore(ref si, ref memberData);
      return this._objectInfo;
    }

    internal static ObjectMap Create(
      string name,
      Type objectType,
      string[] memberNames,
      ObjectReader objectReader,
      int objectId,
      BinaryAssemblyInfo assemblyInfo)
    {
      return new ObjectMap(name, objectType, memberNames, objectReader, objectId, assemblyInfo);
    }

    internal static ObjectMap Create(
      string name,
      string[] memberNames,
      BinaryTypeEnum[] binaryTypeEnumA,
      object[] typeInformationA,
      int[] memberAssemIds,
      ObjectReader objectReader,
      int objectId,
      BinaryAssemblyInfo assemblyInfo,
      SizedArray assemIdToAssemblyTable)
    {
      return new ObjectMap(name, memberNames, binaryTypeEnumA, typeInformationA, memberAssemIds, objectReader, objectId, assemblyInfo, assemIdToAssemblyTable);
    }
  }
}
