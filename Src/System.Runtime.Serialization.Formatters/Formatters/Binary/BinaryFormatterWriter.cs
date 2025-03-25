// Decompiled with JetBrains decompiler
// Type: System.Runtime.Serialization.Formatters.Binary.BinaryFormatterWriter
// Assembly: System.Runtime.Serialization.Formatters, Version=4.0.3.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 59FEC222-A9B8-4506-99C2-FAFFD4E25D28
// Assembly location: C:\Users\Admin\Desktop\RE\1\System.Runtime.Serialization.Formatters.dll

using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;


namespace System.Runtime.Serialization.Formatters.Binary
{
  internal sealed class BinaryFormatterWriter
  {
    private const int ChunkSize = 4096;
    private readonly Stream _outputStream;
    private readonly FormatterTypeStyle _formatterTypeStyle;
    private readonly ObjectWriter _objectWriter;
    private readonly BinaryWriter _dataWriter;
    private int _consecutiveNullArrayEntryCount;
    private Dictionary<string, BinaryFormatterWriter.ObjectMapInfo> _objectMapTable;
    private BinaryObject _binaryObject;
    private BinaryObjectWithMap _binaryObjectWithMap;
    private BinaryObjectWithMapTyped _binaryObjectWithMapTyped;
    private BinaryObjectString _binaryObjectString;
    private BinaryArray _binaryArray;
    private byte[] _byteBuffer;
    private MemberPrimitiveUnTyped _memberPrimitiveUnTyped;
    private MemberPrimitiveTyped _memberPrimitiveTyped;
    private ObjectNull _objectNull;
    private MemberReference _memberReference;
    private BinaryAssembly _binaryAssembly;

    internal BinaryFormatterWriter(
      Stream outputStream,
      ObjectWriter objectWriter,
      FormatterTypeStyle formatterTypeStyle)
    {
      this._outputStream = outputStream;
      this._formatterTypeStyle = formatterTypeStyle;
      this._objectWriter = objectWriter;
      this._dataWriter = new BinaryWriter(outputStream, Encoding.UTF8);
    }

    internal void WriteBegin()
    {
    }

    internal void WriteEnd() => this._dataWriter.Flush();

    internal void WriteBoolean(bool value) => this._dataWriter.Write(value);

    internal void WriteByte(byte value) => this._dataWriter.Write(value);

    private void WriteBytes(byte[] value) => this._dataWriter.Write(value);

    private void WriteBytes(byte[] byteA, int offset, int size)
    {
      this._dataWriter.Write(byteA, offset, size);
    }

    internal void WriteChar(char value) => this._dataWriter.Write(value);

    internal void WriteChars(char[] value) => this._dataWriter.Write(value);

    internal void WriteDecimal(Decimal value)
    {
      this.WriteString(value.ToString((IFormatProvider) CultureInfo.InvariantCulture));
    }

    internal void WriteSingle(float value) => this._dataWriter.Write(value);

    internal void WriteDouble(double value) => this._dataWriter.Write(value);

    internal void WriteInt16(short value) => this._dataWriter.Write(value);

    internal void WriteInt32(int value) => this._dataWriter.Write(value);

    internal void WriteInt64(long value) => this._dataWriter.Write(value);

    internal void WriteSByte(sbyte value) => this.WriteByte((byte) value);

    internal void WriteString(string value) => this._dataWriter.Write(value);

    internal void WriteTimeSpan(TimeSpan value) => this.WriteInt64(value.Ticks);

    internal void WriteDateTime(DateTime value) => this.WriteInt64(value.Ticks);

    internal void WriteUInt16(ushort value) => this._dataWriter.Write(value);

    internal void WriteUInt32(uint value) => this._dataWriter.Write(value);

    internal void WriteUInt64(ulong value) => this._dataWriter.Write(value);

    internal void WriteObjectEnd(NameInfo memberNameInfo, NameInfo typeNameInfo)
    {
    }

    internal void WriteSerializationHeaderEnd() => new MessageEnd().Write(this);

    internal void WriteSerializationHeader(
      int topId,
      int headerId,
      int minorVersion,
      int majorVersion)
    {
      new SerializationHeaderRecord(BinaryHeaderEnum.SerializedStreamHeader, topId, headerId, minorVersion, majorVersion).Write(this);
    }

    internal void WriteObject(
      NameInfo nameInfo,
      NameInfo typeNameInfo,
      int numMembers,
      string[] memberNames,
      Type[] memberTypes,
      WriteObjectInfo[] memberObjectInfos)
    {
      this.InternalWriteItemNull();
      int objectId = (int) nameInfo._objectId;
      string str1;
      string str2;
      if (objectId >= 0)
        str2 = str1 = nameInfo.NIname;
      else
        str1 = str2 = typeNameInfo.NIname;
      string str3 = str2;
      if (this._objectMapTable == null)
        this._objectMapTable = new Dictionary<string, BinaryFormatterWriter.ObjectMapInfo>();
      BinaryFormatterWriter.ObjectMapInfo objectMapInfo;
      if (this._objectMapTable.TryGetValue(str3, out objectMapInfo) && objectMapInfo.IsCompatible(numMembers, memberNames, memberTypes))
      {
        if (this._binaryObject == null)
          this._binaryObject = new BinaryObject();
        this._binaryObject.Set(objectId, objectMapInfo._objectId);
        this._binaryObject.Write(this);
      }
      else if (!typeNameInfo._transmitTypeOnObject)
      {
        if (this._binaryObjectWithMap == null)
          this._binaryObjectWithMap = new BinaryObjectWithMap();
        int assemId = (int) typeNameInfo._assemId;
        this._binaryObjectWithMap.Set(objectId, str3, numMembers, memberNames, assemId);
        this._binaryObjectWithMap.Write(this);
        if (objectMapInfo != null)
          return;
        this._objectMapTable.Add(str3, new BinaryFormatterWriter.ObjectMapInfo(objectId, numMembers, memberNames, memberTypes));
      }
      else
      {
        BinaryTypeEnum[] binaryTypeEnumA = new BinaryTypeEnum[numMembers];
        object[] typeInformationA = new object[numMembers];
        int[] memberAssemIds = new int[numMembers];
        for (int index = 0; index < numMembers; ++index)
        {
          object typeInformation = (object) null;
          int assemId;
          binaryTypeEnumA[index] = BinaryTypeConverter.GetBinaryTypeInfo(memberTypes[index], memberObjectInfos[index], (string) null, this._objectWriter, out typeInformation, out assemId);
          typeInformationA[index] = typeInformation;
          memberAssemIds[index] = assemId;
        }
        if (this._binaryObjectWithMapTyped == null)
          this._binaryObjectWithMapTyped = new BinaryObjectWithMapTyped();
        int assemId1 = (int) typeNameInfo._assemId;
        this._binaryObjectWithMapTyped.Set(objectId, str3, numMembers, memberNames, binaryTypeEnumA, typeInformationA, memberAssemIds, assemId1);
        this._binaryObjectWithMapTyped.Write(this);
        if (objectMapInfo != null)
          return;
        this._objectMapTable.Add(str3, new BinaryFormatterWriter.ObjectMapInfo(objectId, numMembers, memberNames, memberTypes));
      }
    }

    internal void WriteObjectString(int objectId, string value)
    {
      this.InternalWriteItemNull();
      if (this._binaryObjectString == null)
        this._binaryObjectString = new BinaryObjectString();
      this._binaryObjectString.Set(objectId, value);
      this._binaryObjectString.Write(this);
    }

    internal void WriteSingleArray(
      NameInfo memberNameInfo,
      NameInfo arrayNameInfo,
      WriteObjectInfo objectInfo,
      NameInfo arrayElemTypeNameInfo,
      int length,
      int lowerBound,
      Array array)
    {
      this.InternalWriteItemNull();
      int[] lengthA = new int[1]{ length };
      int[] lowerBoundA = (int[]) null;
      object typeInformation = (object) null;
      BinaryArrayTypeEnum binaryArrayTypeEnum;
      if (lowerBound == 0)
      {
        binaryArrayTypeEnum = BinaryArrayTypeEnum.Single;
      }
      else
      {
        binaryArrayTypeEnum = BinaryArrayTypeEnum.SingleOffset;
        lowerBoundA = new int[1]{ lowerBound };
      }
      int assemId;
      BinaryTypeEnum binaryTypeInfo = BinaryTypeConverter.GetBinaryTypeInfo(arrayElemTypeNameInfo._type, objectInfo, arrayElemTypeNameInfo.NIname, this._objectWriter, out typeInformation, out assemId);
      if (this._binaryArray == null)
        this._binaryArray = new BinaryArray();
      this._binaryArray.Set((int) arrayNameInfo._objectId, 1, lengthA, lowerBoundA, binaryTypeInfo, typeInformation, binaryArrayTypeEnum, assemId);
      this._binaryArray.Write(this);
      if (!Converter.IsWriteAsByteArray(arrayElemTypeNameInfo._primitiveTypeEnum) || lowerBound != 0)
        return;
      if (arrayElemTypeNameInfo._primitiveTypeEnum == InternalPrimitiveTypeE.Byte)
        this.WriteBytes((byte[]) array);
      else if (arrayElemTypeNameInfo._primitiveTypeEnum == InternalPrimitiveTypeE.Char)
        this.WriteChars((char[]) array);
      else
        this.WriteArrayAsBytes(array, Converter.TypeLength(arrayElemTypeNameInfo._primitiveTypeEnum));
    }

    private void WriteArrayAsBytes(Array array, int typeLength)
    {
      this.InternalWriteItemNull();
      int num1 = array.Length * typeLength;
      int num2 = 0;
      if (this._byteBuffer == null)
        this._byteBuffer = new byte[4096];
      int num3;
      for (; num2 < array.Length; num2 += num3)
      {
        num3 = Math.Min(4096 / typeLength, array.Length - num2);
        int num4 = num3 * typeLength;
        Buffer.BlockCopy(array, num2 * typeLength, (Array) this._byteBuffer, 0, num4);
        if (!BitConverter.IsLittleEndian)
        {
          for (int index1 = 0; index1 < num4; index1 += typeLength)
          {
            for (int index2 = 0; index2 < typeLength / 2; ++index2)
            {
              byte num5 = this._byteBuffer[index1 + index2];
              this._byteBuffer[index1 + index2] = this._byteBuffer[index1 + typeLength - 1 - index2];
              this._byteBuffer[index1 + typeLength - 1 - index2] = num5;
            }
          }
        }
        this.WriteBytes(this._byteBuffer, 0, num4);
      }
    }

    internal void WriteJaggedArray(
      NameInfo memberNameInfo,
      NameInfo arrayNameInfo,
      WriteObjectInfo objectInfo,
      NameInfo arrayElemTypeNameInfo,
      int length,
      int lowerBound)
    {
      this.InternalWriteItemNull();
      int[] lengthA = new int[1]{ length };
      int[] lowerBoundA = (int[]) null;
      object typeInformation = (object) null;
      int assemId = 0;
      BinaryArrayTypeEnum binaryArrayTypeEnum;
      if (lowerBound == 0)
      {
        binaryArrayTypeEnum = BinaryArrayTypeEnum.Jagged;
      }
      else
      {
        binaryArrayTypeEnum = BinaryArrayTypeEnum.JaggedOffset;
        lowerBoundA = new int[1]{ lowerBound };
      }
      BinaryTypeEnum binaryTypeInfo = BinaryTypeConverter.GetBinaryTypeInfo(arrayElemTypeNameInfo._type, objectInfo, arrayElemTypeNameInfo.NIname, this._objectWriter, out typeInformation, out assemId);
      if (this._binaryArray == null)
        this._binaryArray = new BinaryArray();
      this._binaryArray.Set((int) arrayNameInfo._objectId, 1, lengthA, lowerBoundA, binaryTypeInfo, typeInformation, binaryArrayTypeEnum, assemId);
      this._binaryArray.Write(this);
    }

    internal void WriteRectangleArray(
      NameInfo memberNameInfo,
      NameInfo arrayNameInfo,
      WriteObjectInfo objectInfo,
      NameInfo arrayElemTypeNameInfo,
      int rank,
      int[] lengthA,
      int[] lowerBoundA)
    {
      this.InternalWriteItemNull();
      BinaryArrayTypeEnum binaryArrayTypeEnum = BinaryArrayTypeEnum.Rectangular;
      object typeInformation = (object) null;
      int assemId = 0;
      BinaryTypeEnum binaryTypeInfo = BinaryTypeConverter.GetBinaryTypeInfo(arrayElemTypeNameInfo._type, objectInfo, arrayElemTypeNameInfo.NIname, this._objectWriter, out typeInformation, out assemId);
      if (this._binaryArray == null)
        this._binaryArray = new BinaryArray();
      for (int index = 0; index < rank; ++index)
      {
        if (lowerBoundA[index] != 0)
        {
          binaryArrayTypeEnum = BinaryArrayTypeEnum.RectangularOffset;
          break;
        }
      }
      this._binaryArray.Set((int) arrayNameInfo._objectId, rank, lengthA, lowerBoundA, binaryTypeInfo, typeInformation, binaryArrayTypeEnum, assemId);
      this._binaryArray.Write(this);
    }

    internal void WriteObjectByteArray(
      NameInfo memberNameInfo,
      NameInfo arrayNameInfo,
      WriteObjectInfo objectInfo,
      NameInfo arrayElemTypeNameInfo,
      int length,
      int lowerBound,
      byte[] byteA)
    {
      this.InternalWriteItemNull();
      this.WriteSingleArray(memberNameInfo, arrayNameInfo, objectInfo, arrayElemTypeNameInfo, length, lowerBound, (Array) byteA);
    }

    internal void WriteMember(NameInfo memberNameInfo, NameInfo typeNameInfo, object value)
    {
      this.InternalWriteItemNull();
      InternalPrimitiveTypeE primitiveTypeEnum = typeNameInfo._primitiveTypeEnum;
      if (memberNameInfo._transmitTypeOnMember)
      {
        if (this._memberPrimitiveTyped == null)
          this._memberPrimitiveTyped = new MemberPrimitiveTyped();
        this._memberPrimitiveTyped.Set(primitiveTypeEnum, value);
        this._memberPrimitiveTyped.Write(this);
      }
      else
      {
        if (this._memberPrimitiveUnTyped == null)
          this._memberPrimitiveUnTyped = new MemberPrimitiveUnTyped();
        this._memberPrimitiveUnTyped.Set(primitiveTypeEnum, value);
        this._memberPrimitiveUnTyped.Write(this);
      }
    }

    internal void WriteNullMember(NameInfo memberNameInfo, NameInfo typeNameInfo)
    {
      this.InternalWriteItemNull();
      if (this._objectNull == null)
        this._objectNull = new ObjectNull();
      if (memberNameInfo._isArrayItem)
        return;
      this._objectNull.SetNullCount(1);
      this._objectNull.Write(this);
      this._consecutiveNullArrayEntryCount = 0;
    }

    internal void WriteMemberObjectRef(NameInfo memberNameInfo, int idRef)
    {
      this.InternalWriteItemNull();
      if (this._memberReference == null)
        this._memberReference = new MemberReference();
      this._memberReference.Set(idRef);
      this._memberReference.Write(this);
    }

    internal void WriteMemberNested(NameInfo memberNameInfo) => this.InternalWriteItemNull();

    internal void WriteMemberString(NameInfo memberNameInfo, NameInfo typeNameInfo, string value)
    {
      this.InternalWriteItemNull();
      this.WriteObjectString((int) typeNameInfo._objectId, value);
    }

    internal void WriteItem(NameInfo itemNameInfo, NameInfo typeNameInfo, object value)
    {
      this.InternalWriteItemNull();
      this.WriteMember(itemNameInfo, typeNameInfo, value);
    }

    internal void WriteNullItem(NameInfo itemNameInfo, NameInfo typeNameInfo)
    {
      ++this._consecutiveNullArrayEntryCount;
      this.InternalWriteItemNull();
    }

    internal void WriteDelayedNullItem() => ++this._consecutiveNullArrayEntryCount;

    internal void WriteItemEnd() => this.InternalWriteItemNull();

    private void InternalWriteItemNull()
    {
      if (this._consecutiveNullArrayEntryCount <= 0)
        return;
      if (this._objectNull == null)
        this._objectNull = new ObjectNull();
      this._objectNull.SetNullCount(this._consecutiveNullArrayEntryCount);
      this._objectNull.Write(this);
      this._consecutiveNullArrayEntryCount = 0;
    }

    internal void WriteItemObjectRef(NameInfo nameInfo, int idRef)
    {
      this.InternalWriteItemNull();
      this.WriteMemberObjectRef(nameInfo, idRef);
    }

    internal void WriteAssembly(Type type, string assemblyString, int assemId, bool isNew)
    {
      this.InternalWriteItemNull();
      if (assemblyString == null)
        assemblyString = string.Empty;
      if (!isNew)
        return;
      if (this._binaryAssembly == null)
        this._binaryAssembly = new BinaryAssembly();
      this._binaryAssembly.Set(assemId, assemblyString);
      this._binaryAssembly.Write(this);
    }

    internal void WriteValue(InternalPrimitiveTypeE code, object value)
    {
      switch (code)
      {
        case InternalPrimitiveTypeE.Boolean:
          this.WriteBoolean(Convert.ToBoolean(value, (IFormatProvider) CultureInfo.InvariantCulture));
          break;
        case InternalPrimitiveTypeE.Byte:
          this.WriteByte(Convert.ToByte(value, (IFormatProvider) CultureInfo.InvariantCulture));
          break;
        case InternalPrimitiveTypeE.Char:
          this.WriteChar(Convert.ToChar(value, (IFormatProvider) CultureInfo.InvariantCulture));
          break;
        case InternalPrimitiveTypeE.Decimal:
          this.WriteDecimal(Convert.ToDecimal(value, (IFormatProvider) CultureInfo.InvariantCulture));
          break;
        case InternalPrimitiveTypeE.Double:
          this.WriteDouble(Convert.ToDouble(value, (IFormatProvider) CultureInfo.InvariantCulture));
          break;
        case InternalPrimitiveTypeE.Int16:
          this.WriteInt16(Convert.ToInt16(value, (IFormatProvider) CultureInfo.InvariantCulture));
          break;
        case InternalPrimitiveTypeE.Int32:
          this.WriteInt32(Convert.ToInt32(value, (IFormatProvider) CultureInfo.InvariantCulture));
          break;
        case InternalPrimitiveTypeE.Int64:
          this.WriteInt64(Convert.ToInt64(value, (IFormatProvider) CultureInfo.InvariantCulture));
          break;
        case InternalPrimitiveTypeE.SByte:
          this.WriteSByte(Convert.ToSByte(value, (IFormatProvider) CultureInfo.InvariantCulture));
          break;
        case InternalPrimitiveTypeE.Single:
          this.WriteSingle(Convert.ToSingle(value, (IFormatProvider) CultureInfo.InvariantCulture));
          break;
        case InternalPrimitiveTypeE.TimeSpan:
          this.WriteTimeSpan((TimeSpan) value);
          break;
        case InternalPrimitiveTypeE.DateTime:
          this.WriteDateTime((DateTime) value);
          break;
        case InternalPrimitiveTypeE.UInt16:
          this.WriteUInt16(Convert.ToUInt16(value, (IFormatProvider) CultureInfo.InvariantCulture));
          break;
        case InternalPrimitiveTypeE.UInt32:
          this.WriteUInt32(Convert.ToUInt32(value, (IFormatProvider) CultureInfo.InvariantCulture));
          break;
        case InternalPrimitiveTypeE.UInt64:
          this.WriteUInt64(Convert.ToUInt64(value, (IFormatProvider) CultureInfo.InvariantCulture));
          break;
        default:
          throw new SerializationException(SR.Format(SR.Serialization_TypeCode, (object) code.ToString()));
      }
    }

    private sealed class ObjectMapInfo
    {
      internal readonly int _objectId;
      private readonly int _numMembers;
      private readonly string[] _memberNames;
      private readonly Type[] _memberTypes;

      internal ObjectMapInfo(
        int objectId,
        int numMembers,
        string[] memberNames,
        Type[] memberTypes)
      {
        this._objectId = objectId;
        this._numMembers = numMembers;
        this._memberNames = memberNames;
        this._memberTypes = memberTypes;
      }

      internal bool IsCompatible(int numMembers, string[] memberNames, Type[] memberTypes)
      {
        if (this._numMembers != numMembers)
          return false;
        for (int index = 0; index < numMembers; ++index)
        {
          if (!this._memberNames[index].Equals(memberNames[index]) || memberTypes != null && this._memberTypes[index] != memberTypes[index])
            return false;
        }
        return true;
      }
    }
  }
}
