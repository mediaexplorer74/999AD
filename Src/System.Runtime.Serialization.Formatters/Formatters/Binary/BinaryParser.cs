// Decompiled with JetBrains decompiler
// Type: System.Runtime.Serialization.Formatters.Binary.BinaryParser
// Assembly: System.Runtime.Serialization.Formatters, Version=4.0.3.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 59FEC222-A9B8-4506-99C2-FAFFD4E25D28
// Assembly location: C:\Users\Admin\Desktop\RE\1\System.Runtime.Serialization.Formatters.dll

using System.Globalization;
using System.IO;
using System.Text;


namespace System.Runtime.Serialization.Formatters.Binary
{
  internal sealed class BinaryParser
  {
    private const int ChunkSize = 4096;
    private static readonly Encoding s_encoding = (Encoding) new UTF8Encoding(false, true);
    internal ObjectReader _objectReader;
    internal Stream _input;
    internal long _topId;
    internal long _headerId;
    internal SizedArray _objectMapIdTable;
    internal SizedArray _assemIdToAssemblyTable;
    internal SerStack _stack = new SerStack("ObjectProgressStack");
    internal BinaryTypeEnum _expectedType = BinaryTypeEnum.ObjectUrt;
    internal object _expectedTypeInformation;
    internal ParseRecord _prs;
    private BinaryAssemblyInfo _systemAssemblyInfo;
    private BinaryReader _dataReader;
    private SerStack _opPool;
    private BinaryObject _binaryObject;
    private BinaryObjectWithMap _bowm;
    private BinaryObjectWithMapTyped _bowmt;
    internal BinaryObjectString _objectString;
    internal BinaryCrossAppDomainString _crossAppDomainString;
    internal MemberPrimitiveTyped _memberPrimitiveTyped;
    private byte[] _byteBuffer;
    internal MemberPrimitiveUnTyped memberPrimitiveUnTyped;
    internal MemberReference _memberReference;
    internal ObjectNull _objectNull;
    internal static volatile MessageEnd _messageEnd;

    internal BinaryParser(Stream stream, ObjectReader objectReader)
    {
      this._input = stream;
      this._objectReader = objectReader;
      this._dataReader = new BinaryReader(this._input, BinaryParser.s_encoding);
    }

    internal BinaryAssemblyInfo SystemAssemblyInfo
    {
      get
      {
        return this._systemAssemblyInfo ?? (this._systemAssemblyInfo = new BinaryAssemblyInfo(Converter.s_urtAssemblyString, Converter.s_urtAssembly));
      }
    }

    internal SizedArray ObjectMapIdTable
    {
      get => this._objectMapIdTable ?? (this._objectMapIdTable = new SizedArray());
    }

    internal SizedArray AssemIdToAssemblyTable
    {
      get => this._assemIdToAssemblyTable ?? (this._assemIdToAssemblyTable = new SizedArray(2));
    }

    internal ParseRecord PRs => this._prs ?? (this._prs = new ParseRecord());

    internal void Run()
    {
      try
      {
        bool flag1 = true;
        this.ReadBegin();
        this.ReadSerializationHeaderRecord();
        while (flag1)
        {
          BinaryHeaderEnum binaryHeaderEnum = BinaryHeaderEnum.Object;
          switch (this._expectedType)
          {
            case BinaryTypeEnum.Primitive:
              this.ReadMemberPrimitiveUnTyped();
              break;
            case BinaryTypeEnum.String:
            case BinaryTypeEnum.Object:
            case BinaryTypeEnum.ObjectUrt:
            case BinaryTypeEnum.ObjectUser:
            case BinaryTypeEnum.ObjectArray:
            case BinaryTypeEnum.StringArray:
            case BinaryTypeEnum.PrimitiveArray:
              byte p1 = this._dataReader.ReadByte();
              binaryHeaderEnum = (BinaryHeaderEnum) p1;
              switch (binaryHeaderEnum)
              {
                case BinaryHeaderEnum.Object:
                  this.ReadObject();
                  break;
                case BinaryHeaderEnum.ObjectWithMap:
                case BinaryHeaderEnum.ObjectWithMapAssemId:
                  this.ReadObjectWithMap(binaryHeaderEnum);
                  break;
                case BinaryHeaderEnum.ObjectWithMapTyped:
                case BinaryHeaderEnum.ObjectWithMapTypedAssemId:
                  this.ReadObjectWithMapTyped(binaryHeaderEnum);
                  break;
                case BinaryHeaderEnum.ObjectString:
                case BinaryHeaderEnum.CrossAppDomainString:
                  this.ReadObjectString(binaryHeaderEnum);
                  break;
                case BinaryHeaderEnum.Array:
                case BinaryHeaderEnum.ArraySinglePrimitive:
                case BinaryHeaderEnum.ArraySingleObject:
                case BinaryHeaderEnum.ArraySingleString:
                  this.ReadArray(binaryHeaderEnum);
                  break;
                case BinaryHeaderEnum.MemberPrimitiveTyped:
                  this.ReadMemberPrimitiveTyped();
                  break;
                case BinaryHeaderEnum.MemberReference:
                  this.ReadMemberReference();
                  break;
                case BinaryHeaderEnum.ObjectNull:
                case BinaryHeaderEnum.ObjectNullMultiple256:
                case BinaryHeaderEnum.ObjectNullMultiple:
                  this.ReadObjectNull(binaryHeaderEnum);
                  break;
                case BinaryHeaderEnum.MessageEnd:
                  flag1 = false;
                  this.ReadMessageEnd();
                  this.ReadEnd();
                  break;
                case BinaryHeaderEnum.Assembly:
                case BinaryHeaderEnum.CrossAppDomainAssembly:
                  this.ReadAssembly(binaryHeaderEnum);
                  break;
                case BinaryHeaderEnum.CrossAppDomainMap:
                  this.ReadCrossAppDomainMap();
                  break;
                default:
                  throw new SerializationException(SR.Format(SR.Serialization_BinaryHeader, (object) p1));
              }
              break;
            default:
              throw new SerializationException(SR.Serialization_TypeExpected);
          }
          if (binaryHeaderEnum != BinaryHeaderEnum.Assembly)
          {
            bool flag2 = false;
            while (!flag2)
            {
              ObjectProgress op = (ObjectProgress) this._stack.Peek();
              if (op == null)
              {
                this._expectedType = BinaryTypeEnum.ObjectUrt;
                this._expectedTypeInformation = (object) null;
                flag2 = true;
              }
              else
              {
                flag2 = op.GetNext(out op._expectedType, out op._expectedTypeInformation);
                this._expectedType = op._expectedType;
                this._expectedTypeInformation = op._expectedTypeInformation;
                if (!flag2)
                {
                  this.PRs.Init();
                  if (op._memberValueEnum == InternalMemberValueE.Nested)
                  {
                    this.PRs._parseTypeEnum = InternalParseTypeE.MemberEnd;
                    this.PRs._memberTypeEnum = op._memberTypeEnum;
                    this.PRs._memberValueEnum = op._memberValueEnum;
                    this._objectReader.Parse(this.PRs);
                  }
                  else
                  {
                    this.PRs._parseTypeEnum = InternalParseTypeE.ObjectEnd;
                    this.PRs._memberTypeEnum = op._memberTypeEnum;
                    this.PRs._memberValueEnum = op._memberValueEnum;
                    this._objectReader.Parse(this.PRs);
                  }
                  this._stack.Pop();
                  this.PutOp(op);
                }
              }
            }
          }
        }
      }
      catch (EndOfStreamException ex)
      {
        throw new SerializationException(SR.Serialization_StreamEnd);
      }
    }

    internal void ReadBegin()
    {
    }

    internal void ReadEnd()
    {
    }

    internal bool ReadBoolean() => this._dataReader.ReadBoolean();

    internal byte ReadByte() => this._dataReader.ReadByte();

    internal byte[] ReadBytes(int length) => this._dataReader.ReadBytes(length);

    internal void ReadBytes(byte[] byteA, int offset, int size)
    {
      int num;
      for (; size > 0; size -= num)
      {
        num = this._dataReader.Read(byteA, offset, size);
        if (num == 0)
          throw new EndOfStreamException(SR.IO_EOF_ReadBeyondEOF);
        offset += num;
      }
    }

    internal char ReadChar() => this._dataReader.ReadChar();

    internal char[] ReadChars(int length) => this._dataReader.ReadChars(length);

    internal Decimal ReadDecimal()
    {
      return Decimal.Parse(this._dataReader.ReadString(), (IFormatProvider) CultureInfo.InvariantCulture);
    }

    internal float ReadSingle() => this._dataReader.ReadSingle();

    internal double ReadDouble() => this._dataReader.ReadDouble();

    internal short ReadInt16() => this._dataReader.ReadInt16();

    internal int ReadInt32() => this._dataReader.ReadInt32();

    internal long ReadInt64() => this._dataReader.ReadInt64();

    internal sbyte ReadSByte() => (sbyte) this.ReadByte();

    internal string ReadString() => this._dataReader.ReadString();

    internal TimeSpan ReadTimeSpan() => new TimeSpan(this.ReadInt64());

    internal DateTime ReadDateTime() => BinaryParser.FromBinaryRaw(this.ReadInt64());

    private static DateTime FromBinaryRaw(long dateData)
    {
      return new DateTime(dateData & 4611686018427387903L);
    }

    internal ushort ReadUInt16() => this._dataReader.ReadUInt16();

    internal uint ReadUInt32() => this._dataReader.ReadUInt32();

    internal ulong ReadUInt64() => this._dataReader.ReadUInt64();

    internal void ReadSerializationHeaderRecord()
    {
      SerializationHeaderRecord serializationHeaderRecord = new SerializationHeaderRecord();
      serializationHeaderRecord.Read(this);
      this._topId = serializationHeaderRecord._topId > 0 ? this._objectReader.GetId((long) serializationHeaderRecord._topId) : (long) serializationHeaderRecord._topId;
      this._headerId = serializationHeaderRecord._headerId > 0 ? this._objectReader.GetId((long) serializationHeaderRecord._headerId) : (long) serializationHeaderRecord._headerId;
    }

    internal void ReadAssembly(BinaryHeaderEnum binaryHeaderEnum)
    {
      BinaryAssembly binaryAssembly = new BinaryAssembly();
      if (binaryHeaderEnum == BinaryHeaderEnum.CrossAppDomainAssembly)
      {
        BinaryCrossAppDomainAssembly appDomainAssembly = new BinaryCrossAppDomainAssembly();
        appDomainAssembly.Read(this);
        binaryAssembly._assemId = appDomainAssembly._assemId;
        binaryAssembly._assemblyString = this._objectReader.CrossAppDomainArray(appDomainAssembly._assemblyIndex) as string;
        if (binaryAssembly._assemblyString == null)
          throw new SerializationException(SR.Format(SR.Serialization_CrossAppDomainError, (object) "String", (object) appDomainAssembly._assemblyIndex));
      }
      else
        binaryAssembly.Read(this);
      this.AssemIdToAssemblyTable[binaryAssembly._assemId] = (object) new BinaryAssemblyInfo(binaryAssembly._assemblyString);
    }

    private void ReadObject()
    {
      if (this._binaryObject == null)
        this._binaryObject = new BinaryObject();
      this._binaryObject.Read(this);
      ObjectMap objectMap = (ObjectMap) this.ObjectMapIdTable[this._binaryObject._mapId];
      if (objectMap == null)
        throw new SerializationException(SR.Format(SR.Serialization_Map, (object) this._binaryObject._mapId));
      ObjectProgress op = this.GetOp();
      ParseRecord pr = op._pr;
      this._stack.Push((object) op);
      op._objectTypeEnum = InternalObjectTypeE.Object;
      op._binaryTypeEnumA = objectMap._binaryTypeEnumA;
      op._memberNames = objectMap._memberNames;
      op._memberTypes = objectMap._memberTypes;
      op._typeInformationA = objectMap._typeInformationA;
      op._memberLength = op._binaryTypeEnumA.Length;
      ObjectProgress objectProgress = (ObjectProgress) this._stack.PeekPeek();
      if (objectProgress == null || objectProgress._isInitial)
      {
        op._name = objectMap._objectName;
        pr._parseTypeEnum = InternalParseTypeE.Object;
        op._memberValueEnum = InternalMemberValueE.Empty;
      }
      else
      {
        pr._parseTypeEnum = InternalParseTypeE.Member;
        pr._memberValueEnum = InternalMemberValueE.Nested;
        op._memberValueEnum = InternalMemberValueE.Nested;
        switch (objectProgress._objectTypeEnum)
        {
          case InternalObjectTypeE.Object:
            pr._name = objectProgress._name;
            pr._memberTypeEnum = InternalMemberTypeE.Field;
            op._memberTypeEnum = InternalMemberTypeE.Field;
            break;
          case InternalObjectTypeE.Array:
            pr._memberTypeEnum = InternalMemberTypeE.Item;
            op._memberTypeEnum = InternalMemberTypeE.Item;
            break;
          default:
            throw new SerializationException(SR.Format(SR.Serialization_Map, (object) objectProgress._objectTypeEnum.ToString()));
        }
      }
      pr._objectId = this._objectReader.GetId((long) this._binaryObject._objectId);
      pr._objectInfo = objectMap.CreateObjectInfo(ref pr._si, ref pr._memberData);
      if (pr._objectId == this._topId)
        pr._objectPositionEnum = InternalObjectPositionE.Top;
      pr._objectTypeEnum = InternalObjectTypeE.Object;
      pr._keyDt = objectMap._objectName;
      pr._dtType = objectMap._objectType;
      pr._dtTypeCode = InternalPrimitiveTypeE.Invalid;
      this._objectReader.Parse(pr);
    }

    internal void ReadCrossAppDomainMap()
    {
      BinaryCrossAppDomainMap crossAppDomainMap = new BinaryCrossAppDomainMap();
      crossAppDomainMap.Read(this);
      object p2 = this._objectReader.CrossAppDomainArray(crossAppDomainMap._crossAppDomainArrayIndex);
      switch (p2)
      {
        case BinaryObjectWithMap record1:
          this.ReadObjectWithMap(record1);
          break;
        case BinaryObjectWithMapTyped record2:
          this.ReadObjectWithMapTyped(record2);
          break;
        default:
          throw new SerializationException(SR.Format(SR.Serialization_CrossAppDomainError, (object) "BinaryObjectMap", p2));
      }
    }

    internal void ReadObjectWithMap(BinaryHeaderEnum binaryHeaderEnum)
    {
      if (this._bowm == null)
        this._bowm = new BinaryObjectWithMap(binaryHeaderEnum);
      else
        this._bowm._binaryHeaderEnum = binaryHeaderEnum;
      this._bowm.Read(this);
      this.ReadObjectWithMap(this._bowm);
    }

    private void ReadObjectWithMap(BinaryObjectWithMap record)
    {
      BinaryAssemblyInfo assemblyInfo = (BinaryAssemblyInfo) null;
      ObjectProgress op = this.GetOp();
      ParseRecord pr = op._pr;
      this._stack.Push((object) op);
      if (record._binaryHeaderEnum == BinaryHeaderEnum.ObjectWithMapAssemId)
      {
        if (record._assemId < 1)
          throw new SerializationException(SR.Format(SR.Serialization_Assembly, (object) record._name));
        assemblyInfo = (BinaryAssemblyInfo) this.AssemIdToAssemblyTable[record._assemId];
        if (assemblyInfo == null)
          throw new SerializationException(SR.Format(SR.Serialization_Assembly, (object) (record._assemId.ToString() + " " + record._name)));
      }
      else if (record._binaryHeaderEnum == BinaryHeaderEnum.ObjectWithMap)
        assemblyInfo = this.SystemAssemblyInfo;
      Type type = this._objectReader.GetType(assemblyInfo, record._name);
      ObjectMap objectMap = ObjectMap.Create(record._name, type, record._memberNames, this._objectReader, record._objectId, assemblyInfo);
      this.ObjectMapIdTable[record._objectId] = (object) objectMap;
      op._objectTypeEnum = InternalObjectTypeE.Object;
      op._binaryTypeEnumA = objectMap._binaryTypeEnumA;
      op._typeInformationA = objectMap._typeInformationA;
      op._memberLength = op._binaryTypeEnumA.Length;
      op._memberNames = objectMap._memberNames;
      op._memberTypes = objectMap._memberTypes;
      ObjectProgress objectProgress = (ObjectProgress) this._stack.PeekPeek();
      if (objectProgress == null || objectProgress._isInitial)
      {
        op._name = record._name;
        pr._parseTypeEnum = InternalParseTypeE.Object;
        op._memberValueEnum = InternalMemberValueE.Empty;
      }
      else
      {
        pr._parseTypeEnum = InternalParseTypeE.Member;
        pr._memberValueEnum = InternalMemberValueE.Nested;
        op._memberValueEnum = InternalMemberValueE.Nested;
        switch (objectProgress._objectTypeEnum)
        {
          case InternalObjectTypeE.Object:
            pr._name = objectProgress._name;
            pr._memberTypeEnum = InternalMemberTypeE.Field;
            op._memberTypeEnum = InternalMemberTypeE.Field;
            break;
          case InternalObjectTypeE.Array:
            pr._memberTypeEnum = InternalMemberTypeE.Item;
            op._memberTypeEnum = InternalMemberTypeE.Field;
            break;
          default:
            throw new SerializationException(SR.Format(SR.Serialization_ObjectTypeEnum, (object) objectProgress._objectTypeEnum.ToString()));
        }
      }
      pr._objectTypeEnum = InternalObjectTypeE.Object;
      pr._objectId = this._objectReader.GetId((long) record._objectId);
      pr._objectInfo = objectMap.CreateObjectInfo(ref pr._si, ref pr._memberData);
      if (pr._objectId == this._topId)
        pr._objectPositionEnum = InternalObjectPositionE.Top;
      pr._keyDt = record._name;
      pr._dtType = objectMap._objectType;
      pr._dtTypeCode = InternalPrimitiveTypeE.Invalid;
      this._objectReader.Parse(pr);
    }

    internal void ReadObjectWithMapTyped(BinaryHeaderEnum binaryHeaderEnum)
    {
      if (this._bowmt == null)
        this._bowmt = new BinaryObjectWithMapTyped(binaryHeaderEnum);
      else
        this._bowmt._binaryHeaderEnum = binaryHeaderEnum;
      this._bowmt.Read(this);
      this.ReadObjectWithMapTyped(this._bowmt);
    }

    private void ReadObjectWithMapTyped(BinaryObjectWithMapTyped record)
    {
      BinaryAssemblyInfo assemblyInfo = (BinaryAssemblyInfo) null;
      ObjectProgress op = this.GetOp();
      ParseRecord pr = op._pr;
      this._stack.Push((object) op);
      if (record._binaryHeaderEnum == BinaryHeaderEnum.ObjectWithMapTypedAssemId)
      {
        if (record._assemId < 1)
          throw new SerializationException(SR.Format(SR.Serialization_AssemblyId, (object) record._name));
        assemblyInfo = (BinaryAssemblyInfo) this.AssemIdToAssemblyTable[record._assemId];
        if (assemblyInfo == null)
          throw new SerializationException(SR.Format(SR.Serialization_AssemblyId, (object) (record._assemId.ToString() + " " + record._name)));
      }
      else if (record._binaryHeaderEnum == BinaryHeaderEnum.ObjectWithMapTyped)
        assemblyInfo = this.SystemAssemblyInfo;
      ObjectMap objectMap = ObjectMap.Create(record._name, record._memberNames, record._binaryTypeEnumA, record._typeInformationA, record._memberAssemIds, this._objectReader, record._objectId, assemblyInfo, this.AssemIdToAssemblyTable);
      this.ObjectMapIdTable[record._objectId] = (object) objectMap;
      op._objectTypeEnum = InternalObjectTypeE.Object;
      op._binaryTypeEnumA = objectMap._binaryTypeEnumA;
      op._typeInformationA = objectMap._typeInformationA;
      op._memberLength = op._binaryTypeEnumA.Length;
      op._memberNames = objectMap._memberNames;
      op._memberTypes = objectMap._memberTypes;
      ObjectProgress objectProgress = (ObjectProgress) this._stack.PeekPeek();
      if (objectProgress == null || objectProgress._isInitial)
      {
        op._name = record._name;
        pr._parseTypeEnum = InternalParseTypeE.Object;
        op._memberValueEnum = InternalMemberValueE.Empty;
      }
      else
      {
        pr._parseTypeEnum = InternalParseTypeE.Member;
        pr._memberValueEnum = InternalMemberValueE.Nested;
        op._memberValueEnum = InternalMemberValueE.Nested;
        switch (objectProgress._objectTypeEnum)
        {
          case InternalObjectTypeE.Object:
            pr._name = objectProgress._name;
            pr._memberTypeEnum = InternalMemberTypeE.Field;
            op._memberTypeEnum = InternalMemberTypeE.Field;
            break;
          case InternalObjectTypeE.Array:
            pr._memberTypeEnum = InternalMemberTypeE.Item;
            op._memberTypeEnum = InternalMemberTypeE.Item;
            break;
          default:
            throw new SerializationException(SR.Format(SR.Serialization_ObjectTypeEnum, (object) objectProgress._objectTypeEnum.ToString()));
        }
      }
      pr._objectTypeEnum = InternalObjectTypeE.Object;
      pr._objectInfo = objectMap.CreateObjectInfo(ref pr._si, ref pr._memberData);
      pr._objectId = this._objectReader.GetId((long) record._objectId);
      if (pr._objectId == this._topId)
        pr._objectPositionEnum = InternalObjectPositionE.Top;
      pr._keyDt = record._name;
      pr._dtType = objectMap._objectType;
      pr._dtTypeCode = InternalPrimitiveTypeE.Invalid;
      this._objectReader.Parse(pr);
    }

    private void ReadObjectString(BinaryHeaderEnum binaryHeaderEnum)
    {
      if (this._objectString == null)
        this._objectString = new BinaryObjectString();
      if (binaryHeaderEnum == BinaryHeaderEnum.ObjectString)
      {
        this._objectString.Read(this);
      }
      else
      {
        if (this._crossAppDomainString == null)
          this._crossAppDomainString = new BinaryCrossAppDomainString();
        this._crossAppDomainString.Read(this);
        this._objectString._value = this._objectReader.CrossAppDomainArray(this._crossAppDomainString._value) as string;
        if (this._objectString._value == null)
          throw new SerializationException(SR.Format(SR.Serialization_CrossAppDomainError, (object) "String", (object) this._crossAppDomainString._value));
        this._objectString._objectId = this._crossAppDomainString._objectId;
      }
      this.PRs.Init();
      this.PRs._parseTypeEnum = InternalParseTypeE.Object;
      this.PRs._objectId = this._objectReader.GetId((long) this._objectString._objectId);
      if (this.PRs._objectId == this._topId)
        this.PRs._objectPositionEnum = InternalObjectPositionE.Top;
      this.PRs._objectTypeEnum = InternalObjectTypeE.Object;
      ObjectProgress objectProgress = (ObjectProgress) this._stack.Peek();
      this.PRs._value = this._objectString._value;
      this.PRs._keyDt = "System.String";
      this.PRs._dtType = Converter.s_typeofString;
      this.PRs._dtTypeCode = InternalPrimitiveTypeE.Invalid;
      this.PRs._varValue = (object) this._objectString._value;
      if (objectProgress == null)
      {
        this.PRs._parseTypeEnum = InternalParseTypeE.Object;
        this.PRs._name = "System.String";
      }
      else
      {
        this.PRs._parseTypeEnum = InternalParseTypeE.Member;
        this.PRs._memberValueEnum = InternalMemberValueE.InlineValue;
        switch (objectProgress._objectTypeEnum)
        {
          case InternalObjectTypeE.Object:
            this.PRs._name = objectProgress._name;
            this.PRs._memberTypeEnum = InternalMemberTypeE.Field;
            break;
          case InternalObjectTypeE.Array:
            this.PRs._memberTypeEnum = InternalMemberTypeE.Item;
            break;
          default:
            throw new SerializationException(SR.Format(SR.Serialization_ObjectTypeEnum, (object) objectProgress._objectTypeEnum.ToString()));
        }
      }
      this._objectReader.Parse(this.PRs);
    }

    private void ReadMemberPrimitiveTyped()
    {
      if (this._memberPrimitiveTyped == null)
        this._memberPrimitiveTyped = new MemberPrimitiveTyped();
      this._memberPrimitiveTyped.Read(this);
      this.PRs._objectTypeEnum = InternalObjectTypeE.Object;
      ObjectProgress objectProgress = (ObjectProgress) this._stack.Peek();
      this.PRs.Init();
      this.PRs._varValue = this._memberPrimitiveTyped._value;
      this.PRs._keyDt = Converter.ToComType(this._memberPrimitiveTyped._primitiveTypeEnum);
      this.PRs._dtType = Converter.ToType(this._memberPrimitiveTyped._primitiveTypeEnum);
      this.PRs._dtTypeCode = this._memberPrimitiveTyped._primitiveTypeEnum;
      if (objectProgress == null)
      {
        this.PRs._parseTypeEnum = InternalParseTypeE.Object;
        this.PRs._name = "System.Variant";
      }
      else
      {
        this.PRs._parseTypeEnum = InternalParseTypeE.Member;
        this.PRs._memberValueEnum = InternalMemberValueE.InlineValue;
        switch (objectProgress._objectTypeEnum)
        {
          case InternalObjectTypeE.Object:
            this.PRs._name = objectProgress._name;
            this.PRs._memberTypeEnum = InternalMemberTypeE.Field;
            break;
          case InternalObjectTypeE.Array:
            this.PRs._memberTypeEnum = InternalMemberTypeE.Item;
            break;
          default:
            throw new SerializationException(SR.Format(SR.Serialization_ObjectTypeEnum, (object) objectProgress._objectTypeEnum.ToString()));
        }
      }
      this._objectReader.Parse(this.PRs);
    }

    private void ReadArray(BinaryHeaderEnum binaryHeaderEnum)
    {
      BinaryArray binaryArray = new BinaryArray(binaryHeaderEnum);
      binaryArray.Read(this);
      BinaryAssemblyInfo systemAssemblyInfo;
      if (binaryArray._binaryTypeEnum == BinaryTypeEnum.ObjectUser)
      {
        if (binaryArray._assemId < 1)
          throw new SerializationException(SR.Format(SR.Serialization_AssemblyId, binaryArray._typeInformation));
        systemAssemblyInfo = (BinaryAssemblyInfo) this.AssemIdToAssemblyTable[binaryArray._assemId];
      }
      else
        systemAssemblyInfo = this.SystemAssemblyInfo;
      ObjectProgress op = this.GetOp();
      ParseRecord pr = op._pr;
      op._objectTypeEnum = InternalObjectTypeE.Array;
      op._binaryTypeEnum = binaryArray._binaryTypeEnum;
      op._typeInformation = binaryArray._typeInformation;
      ObjectProgress objectProgress = (ObjectProgress) this._stack.PeekPeek();
      if (objectProgress == null || binaryArray._objectId > 0)
      {
        op._name = "System.Array";
        pr._parseTypeEnum = InternalParseTypeE.Object;
        op._memberValueEnum = InternalMemberValueE.Empty;
      }
      else
      {
        pr._parseTypeEnum = InternalParseTypeE.Member;
        pr._memberValueEnum = InternalMemberValueE.Nested;
        op._memberValueEnum = InternalMemberValueE.Nested;
        switch (objectProgress._objectTypeEnum)
        {
          case InternalObjectTypeE.Object:
            pr._name = objectProgress._name;
            pr._memberTypeEnum = InternalMemberTypeE.Field;
            op._memberTypeEnum = InternalMemberTypeE.Field;
            pr._keyDt = objectProgress._name;
            pr._dtType = objectProgress._dtType;
            break;
          case InternalObjectTypeE.Array:
            pr._memberTypeEnum = InternalMemberTypeE.Item;
            op._memberTypeEnum = InternalMemberTypeE.Item;
            break;
          default:
            throw new SerializationException(SR.Format(SR.Serialization_ObjectTypeEnum, (object) objectProgress._objectTypeEnum.ToString()));
        }
      }
      pr._objectId = this._objectReader.GetId((long) binaryArray._objectId);
      pr._objectPositionEnum = pr._objectId != this._topId ? (this._headerId <= 0L || pr._objectId != this._headerId ? InternalObjectPositionE.Child : InternalObjectPositionE.Headers) : InternalObjectPositionE.Top;
      pr._objectTypeEnum = InternalObjectTypeE.Array;
      BinaryTypeConverter.TypeFromInfo(binaryArray._binaryTypeEnum, binaryArray._typeInformation, this._objectReader, systemAssemblyInfo, out pr._arrayElementTypeCode, out pr._arrayElementTypeString, out pr._arrayElementType, out pr._isArrayVariant);
      pr._dtTypeCode = InternalPrimitiveTypeE.Invalid;
      pr._rank = binaryArray._rank;
      pr._lengthA = binaryArray._lengthA;
      pr._lowerBoundA = binaryArray._lowerBoundA;
      bool flag = false;
      switch (binaryArray._binaryArrayTypeEnum)
      {
        case BinaryArrayTypeEnum.Single:
        case BinaryArrayTypeEnum.SingleOffset:
          op._numItems = binaryArray._lengthA[0];
          pr._arrayTypeEnum = InternalArrayTypeE.Single;
          if (Converter.IsWriteAsByteArray(pr._arrayElementTypeCode) && binaryArray._lowerBoundA[0] == 0)
          {
            flag = true;
            this.ReadArrayAsBytes(pr);
            break;
          }
          break;
        case BinaryArrayTypeEnum.Jagged:
        case BinaryArrayTypeEnum.JaggedOffset:
          op._numItems = binaryArray._lengthA[0];
          pr._arrayTypeEnum = InternalArrayTypeE.Jagged;
          break;
        case BinaryArrayTypeEnum.Rectangular:
        case BinaryArrayTypeEnum.RectangularOffset:
          int num = 1;
          for (int index = 0; index < binaryArray._rank; ++index)
            num *= binaryArray._lengthA[index];
          op._numItems = num;
          pr._arrayTypeEnum = InternalArrayTypeE.Rectangular;
          break;
        default:
          throw new SerializationException(SR.Format(SR.Serialization_ArrayType, (object) binaryArray._binaryArrayTypeEnum.ToString()));
      }
      if (!flag)
        this._stack.Push((object) op);
      else
        this.PutOp(op);
      this._objectReader.Parse(pr);
      if (!flag)
        return;
      pr._parseTypeEnum = InternalParseTypeE.ObjectEnd;
      this._objectReader.Parse(pr);
    }

    private void ReadArrayAsBytes(ParseRecord pr)
    {
      if (pr._arrayElementTypeCode == InternalPrimitiveTypeE.Byte)
        pr._newObj = (object) this.ReadBytes(pr._lengthA[0]);
      else if (pr._arrayElementTypeCode == InternalPrimitiveTypeE.Char)
      {
        pr._newObj = (object) this.ReadChars(pr._lengthA[0]);
      }
      else
      {
        int num1 = Converter.TypeLength(pr._arrayElementTypeCode);
        pr._newObj = (object) Converter.CreatePrimitiveArray(pr._arrayElementTypeCode, pr._lengthA[0]);
        Array newObj = (Array) pr._newObj;
        int num2 = 0;
        if (this._byteBuffer == null)
          this._byteBuffer = new byte[4096];
        int num3;
        for (; num2 < newObj.Length; num2 += num3)
        {
          num3 = Math.Min(4096 / num1, newObj.Length - num2);
          int num4 = num3 * num1;
          this.ReadBytes(this._byteBuffer, 0, num4);
          if (!BitConverter.IsLittleEndian)
          {
            for (int index1 = 0; index1 < num4; index1 += num1)
            {
              for (int index2 = 0; index2 < num1 / 2; ++index2)
              {
                byte num5 = this._byteBuffer[index1 + index2];
                this._byteBuffer[index1 + index2] = this._byteBuffer[index1 + num1 - 1 - index2];
                this._byteBuffer[index1 + num1 - 1 - index2] = num5;
              }
            }
          }
          Buffer.BlockCopy((Array) this._byteBuffer, 0, newObj, num2 * num1, num4);
        }
      }
    }

    private void ReadMemberPrimitiveUnTyped()
    {
      ObjectProgress objectProgress = (ObjectProgress) this._stack.Peek();
      if (this.memberPrimitiveUnTyped == null)
        this.memberPrimitiveUnTyped = new MemberPrimitiveUnTyped();
      this.memberPrimitiveUnTyped.Set((InternalPrimitiveTypeE) this._expectedTypeInformation);
      this.memberPrimitiveUnTyped.Read(this);
      this.PRs.Init();
      this.PRs._varValue = this.memberPrimitiveUnTyped._value;
      this.PRs._dtTypeCode = (InternalPrimitiveTypeE) this._expectedTypeInformation;
      this.PRs._dtType = Converter.ToType(this.PRs._dtTypeCode);
      this.PRs._parseTypeEnum = InternalParseTypeE.Member;
      this.PRs._memberValueEnum = InternalMemberValueE.InlineValue;
      if (objectProgress._objectTypeEnum == InternalObjectTypeE.Object)
      {
        this.PRs._memberTypeEnum = InternalMemberTypeE.Field;
        this.PRs._name = objectProgress._name;
      }
      else
        this.PRs._memberTypeEnum = InternalMemberTypeE.Item;
      this._objectReader.Parse(this.PRs);
    }

    private void ReadMemberReference()
    {
      if (this._memberReference == null)
        this._memberReference = new MemberReference();
      this._memberReference.Read(this);
      ObjectProgress objectProgress = (ObjectProgress) this._stack.Peek();
      this.PRs.Init();
      this.PRs._idRef = this._objectReader.GetId((long) this._memberReference._idRef);
      this.PRs._parseTypeEnum = InternalParseTypeE.Member;
      this.PRs._memberValueEnum = InternalMemberValueE.Reference;
      if (objectProgress._objectTypeEnum == InternalObjectTypeE.Object)
      {
        this.PRs._memberTypeEnum = InternalMemberTypeE.Field;
        this.PRs._name = objectProgress._name;
        this.PRs._dtType = objectProgress._dtType;
      }
      else
        this.PRs._memberTypeEnum = InternalMemberTypeE.Item;
      this._objectReader.Parse(this.PRs);
    }

    private void ReadObjectNull(BinaryHeaderEnum binaryHeaderEnum)
    {
      if (this._objectNull == null)
        this._objectNull = new ObjectNull();
      this._objectNull.Read(this, binaryHeaderEnum);
      ObjectProgress objectProgress = (ObjectProgress) this._stack.Peek();
      this.PRs.Init();
      this.PRs._parseTypeEnum = InternalParseTypeE.Member;
      this.PRs._memberValueEnum = InternalMemberValueE.Null;
      if (objectProgress._objectTypeEnum == InternalObjectTypeE.Object)
      {
        this.PRs._memberTypeEnum = InternalMemberTypeE.Field;
        this.PRs._name = objectProgress._name;
        this.PRs._dtType = objectProgress._dtType;
      }
      else
      {
        this.PRs._memberTypeEnum = InternalMemberTypeE.Item;
        this.PRs._consecutiveNullArrayEntryCount = this._objectNull._nullCount;
        objectProgress.ArrayCountIncrement(this._objectNull._nullCount - 1);
      }
      this._objectReader.Parse(this.PRs);
    }

    private void ReadMessageEnd()
    {
      if (BinaryParser._messageEnd == null)
        BinaryParser._messageEnd = new MessageEnd();
      BinaryParser._messageEnd.Read(this);
      if (!this._stack.IsEmpty())
        throw new SerializationException(SR.Serialization_StreamEnd);
    }

    internal object ReadValue(InternalPrimitiveTypeE code)
    {
      switch (code)
      {
        case InternalPrimitiveTypeE.Boolean:
          return (object) this.ReadBoolean();
        case InternalPrimitiveTypeE.Byte:
          return (object) this.ReadByte();
        case InternalPrimitiveTypeE.Char:
          return (object) this.ReadChar();
        case InternalPrimitiveTypeE.Decimal:
          return (object) this.ReadDecimal();
        case InternalPrimitiveTypeE.Double:
          return (object) this.ReadDouble();
        case InternalPrimitiveTypeE.Int16:
          return (object) this.ReadInt16();
        case InternalPrimitiveTypeE.Int32:
          return (object) this.ReadInt32();
        case InternalPrimitiveTypeE.Int64:
          return (object) this.ReadInt64();
        case InternalPrimitiveTypeE.SByte:
          return (object) this.ReadSByte();
        case InternalPrimitiveTypeE.Single:
          return (object) this.ReadSingle();
        case InternalPrimitiveTypeE.TimeSpan:
          return (object) this.ReadTimeSpan();
        case InternalPrimitiveTypeE.DateTime:
          return (object) this.ReadDateTime();
        case InternalPrimitiveTypeE.UInt16:
          return (object) this.ReadUInt16();
        case InternalPrimitiveTypeE.UInt32:
          return (object) this.ReadUInt32();
        case InternalPrimitiveTypeE.UInt64:
          return (object) this.ReadUInt64();
        default:
          throw new SerializationException(SR.Format(SR.Serialization_TypeCode, (object) code.ToString()));
      }
    }

    private ObjectProgress GetOp()
    {
      ObjectProgress op;
      if (this._opPool != null && !this._opPool.IsEmpty())
      {
        op = (ObjectProgress) this._opPool.Pop();
        op.Init();
      }
      else
        op = new ObjectProgress();
      return op;
    }

    private void PutOp(ObjectProgress op)
    {
      if (this._opPool == null)
        this._opPool = new SerStack("opPool");
      this._opPool.Push((object) op);
    }
  }
}
