// Decompiled with JetBrains decompiler
// Type: System.Runtime.Serialization.Formatters.Binary.ObjectWriter
// Assembly: System.Runtime.Serialization.Formatters, Version=4.0.3.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 59FEC222-A9B8-4506-99C2-FAFFD4E25D28
// Assembly location: C:\Users\Admin\Desktop\RE\1\System.Runtime.Serialization.Formatters.dll

using System.Collections.Generic;
using System.Reflection;


namespace System.Runtime.Serialization.Formatters.Binary
{
  internal sealed class ObjectWriter
  {
    private Queue<object> _objectQueue;
    private ObjectIDGenerator _idGenerator;
    private int _currentId;
    private ISurrogateSelector _surrogates;
    private StreamingContext _context;
    private BinaryFormatterWriter _serWriter;
    private SerializationObjectManager _objectManager;
    private long _topId;
    private string _topName;
    private InternalFE _formatterEnums;
    private SerializationBinder _binder;
    private SerObjectInfoInit _serObjectInfoInit;
    private IFormatterConverter _formatterConverter;
    internal object[] _crossAppDomainArray;
    internal List<object> _internalCrossAppDomainArray;
    private object _previousObj;
    private long _previousId;
    private Type _previousType;
    private InternalPrimitiveTypeE _previousCode;
    private Dictionary<string, long> _assemblyToIdTable;
    private SerStack _niPool = new SerStack("NameInfo Pool");

    internal ObjectWriter(
      ISurrogateSelector selector,
      StreamingContext context,
      InternalFE formatterEnums,
      SerializationBinder binder)
    {
      this._currentId = 1;
      this._surrogates = selector;
      this._context = context;
      this._binder = binder;
      this._formatterEnums = formatterEnums;
      this._objectManager = new SerializationObjectManager(context);
    }

    internal void Serialize(object graph, BinaryFormatterWriter serWriter, bool fCheck)
    {
      if (graph == null)
        throw new ArgumentNullException(nameof (graph));
      this._serWriter = serWriter != null ? serWriter : throw new ArgumentNullException(nameof (serWriter));
      serWriter.WriteBegin();
      this._idGenerator = new ObjectIDGenerator();
      this._objectQueue = new Queue<object>();
      this._formatterConverter = (IFormatterConverter) new FormatterConverter();
      this._serObjectInfoInit = new SerObjectInfoInit();
      this._topId = this.InternalGetId(graph, false, (Type) null, out bool _);
      this.WriteSerializedStreamHeader(this._topId, -1L);
      this._objectQueue.Enqueue(graph);
      object next;
      long objID;
      while ((next = this.GetNext(out objID)) != null)
      {
        WriteObjectInfo objectInfo;
        if (next is WriteObjectInfo)
        {
          objectInfo = (WriteObjectInfo) next;
        }
        else
        {
          objectInfo = WriteObjectInfo.Serialize(next, this._surrogates, this._context, this._serObjectInfoInit, this._formatterConverter, this, this._binder);
          objectInfo._assemId = this.GetAssemblyId(objectInfo);
        }
        objectInfo._objectId = objID;
        NameInfo nameInfo = this.TypeToNameInfo(objectInfo);
        this.Write(objectInfo, nameInfo, nameInfo);
        this.PutNameInfo(nameInfo);
        objectInfo.ObjectEnd();
      }
      serWriter.WriteSerializationHeaderEnd();
      serWriter.WriteEnd();
      this._objectManager.RaiseOnSerializedEvent();
    }

    internal SerializationObjectManager ObjectManager => this._objectManager;

    private void Write(WriteObjectInfo objectInfo, NameInfo memberNameInfo, NameInfo typeNameInfo)
    {
      object obj = objectInfo._obj;
      if (obj == null)
        throw new ArgumentNullException("objectInfo._obj");
      Type objectType = objectInfo._objectType;
      long objectId = objectInfo._objectId;
      if ((object) objectType == (object) Converter.s_typeofString)
      {
        memberNameInfo._objectId = objectId;
        this._serWriter.WriteObjectString((int) objectId, obj.ToString());
      }
      else if (objectInfo._isArray)
      {
        this.WriteArray(objectInfo, memberNameInfo, (WriteObjectInfo) null);
      }
      else
      {
        string[] outMemberNames;
        Type[] outMemberTypes;
        object[] outMemberData;
        objectInfo.GetMemberInfo(out outMemberNames, out outMemberTypes, out outMemberData);
        if (objectInfo._isSi || this.CheckTypeFormat(this._formatterEnums._typeFormat, FormatterTypeStyle.TypesAlways))
        {
          memberNameInfo._transmitTypeOnObject = true;
          memberNameInfo._isParentTypeOnObject = true;
          typeNameInfo._transmitTypeOnObject = true;
          typeNameInfo._isParentTypeOnObject = true;
        }
        WriteObjectInfo[] memberObjectInfos = new WriteObjectInfo[outMemberNames.Length];
        for (int index = 0; index < outMemberTypes.Length; ++index)
        {
          Type type = outMemberTypes[index] != (Type) null ? outMemberTypes[index] : (outMemberData[index] != null ? this.GetType(outMemberData[index]) : Converter.s_typeofObject);
          if (this.ToCode(type) == InternalPrimitiveTypeE.Invalid && (object) type != (object) Converter.s_typeofString)
          {
            if (outMemberData[index] != null)
            {
              memberObjectInfos[index] = WriteObjectInfo.Serialize(outMemberData[index], this._surrogates, this._context, this._serObjectInfoInit, this._formatterConverter, this, this._binder);
              memberObjectInfos[index]._assemId = this.GetAssemblyId(memberObjectInfos[index]);
            }
            else
            {
              memberObjectInfos[index] = WriteObjectInfo.Serialize(outMemberTypes[index], this._surrogates, this._context, this._serObjectInfoInit, this._formatterConverter, this._binder);
              memberObjectInfos[index]._assemId = this.GetAssemblyId(memberObjectInfos[index]);
            }
          }
        }
        this.Write(objectInfo, memberNameInfo, typeNameInfo, outMemberNames, outMemberTypes, outMemberData, memberObjectInfos);
      }
    }

    private void Write(
      WriteObjectInfo objectInfo,
      NameInfo memberNameInfo,
      NameInfo typeNameInfo,
      string[] memberNames,
      Type[] memberTypes,
      object[] memberData,
      WriteObjectInfo[] memberObjectInfos)
    {
      int length = memberNames.Length;
      NameInfo nameInfo = (NameInfo) null;
      if (memberNameInfo != null)
      {
        memberNameInfo._objectId = objectInfo._objectId;
        this._serWriter.WriteObject(memberNameInfo, typeNameInfo, length, memberNames, memberTypes, memberObjectInfos);
      }
      else if (objectInfo._objectId == this._topId && this._topName != null)
      {
        nameInfo = this.MemberToNameInfo(this._topName);
        nameInfo._objectId = objectInfo._objectId;
        this._serWriter.WriteObject(nameInfo, typeNameInfo, length, memberNames, memberTypes, memberObjectInfos);
      }
      else if ((object) objectInfo._objectType != (object) Converter.s_typeofString)
      {
        typeNameInfo._objectId = objectInfo._objectId;
        this._serWriter.WriteObject(typeNameInfo, (NameInfo) null, length, memberNames, memberTypes, memberObjectInfos);
      }
      if (memberNameInfo._isParentTypeOnObject)
      {
        memberNameInfo._transmitTypeOnObject = true;
        memberNameInfo._isParentTypeOnObject = false;
      }
      else
        memberNameInfo._transmitTypeOnObject = false;
      for (int index = 0; index < length; ++index)
        this.WriteMemberSetup(objectInfo, memberNameInfo, typeNameInfo, memberNames[index], memberTypes[index], memberData[index], memberObjectInfos[index]);
      if (memberNameInfo != null)
      {
        memberNameInfo._objectId = objectInfo._objectId;
        this._serWriter.WriteObjectEnd(memberNameInfo, typeNameInfo);
      }
      else if (objectInfo._objectId == this._topId && this._topName != null)
      {
        this._serWriter.WriteObjectEnd(nameInfo, typeNameInfo);
        this.PutNameInfo(nameInfo);
      }
      else
      {
        if ((object) objectInfo._objectType == (object) Converter.s_typeofString)
          return;
        this._serWriter.WriteObjectEnd(typeNameInfo, typeNameInfo);
      }
    }

    private void WriteMemberSetup(
      WriteObjectInfo objectInfo,
      NameInfo memberNameInfo,
      NameInfo typeNameInfo,
      string memberName,
      Type memberType,
      object memberData,
      WriteObjectInfo memberObjectInfo)
    {
      NameInfo nameInfo1 = this.MemberToNameInfo(memberName);
      if (memberObjectInfo != null)
        nameInfo1._assemId = memberObjectInfo._assemId;
      nameInfo1._type = memberType;
      NameInfo nameInfo2 = memberObjectInfo != null ? this.TypeToNameInfo(memberObjectInfo) : this.TypeToNameInfo(memberType);
      nameInfo1._transmitTypeOnObject = memberNameInfo._transmitTypeOnObject;
      nameInfo1._isParentTypeOnObject = memberNameInfo._isParentTypeOnObject;
      this.WriteMembers(nameInfo1, nameInfo2, memberData, objectInfo, typeNameInfo, memberObjectInfo);
      this.PutNameInfo(nameInfo1);
      this.PutNameInfo(nameInfo2);
    }

    private void WriteMembers(
      NameInfo memberNameInfo,
      NameInfo memberTypeNameInfo,
      object memberData,
      WriteObjectInfo objectInfo,
      NameInfo typeNameInfo,
      WriteObjectInfo memberObjectInfo)
    {
      Type type1 = memberNameInfo._type;
      bool assignUniqueIdToValueType = false;
      if ((object) type1 == (object) Converter.s_typeofObject || Nullable.GetUnderlyingType(type1) != (Type) null)
      {
        memberTypeNameInfo._transmitTypeOnMember = true;
        memberNameInfo._transmitTypeOnMember = true;
      }
      if (this.CheckTypeFormat(this._formatterEnums._typeFormat, FormatterTypeStyle.TypesAlways) || objectInfo._isSi)
      {
        memberTypeNameInfo._transmitTypeOnObject = true;
        memberNameInfo._transmitTypeOnObject = true;
        memberNameInfo._isParentTypeOnObject = true;
      }
      if (this.CheckForNull(objectInfo, memberNameInfo, memberTypeNameInfo, memberData))
        return;
      object obj = memberData;
      Type type2 = (Type) null;
      if (memberTypeNameInfo._primitiveTypeEnum == InternalPrimitiveTypeE.Invalid)
      {
        type2 = this.GetType(obj);
        if ((object) type1 != (object) type2)
        {
          memberTypeNameInfo._transmitTypeOnMember = true;
          memberNameInfo._transmitTypeOnMember = true;
        }
      }
      if ((object) type1 == (object) Converter.s_typeofObject)
      {
        assignUniqueIdToValueType = true;
        Type type3 = this.GetType(memberData);
        if (memberObjectInfo == null)
          this.TypeToNameInfo(type3, memberTypeNameInfo);
        else
          this.TypeToNameInfo(memberObjectInfo, memberTypeNameInfo);
      }
      if (memberObjectInfo != null && memberObjectInfo._isArray)
      {
        if (type2 == (Type) null)
          this.GetType(obj);
        long objectId = this.Schedule(obj, false, (Type) null, memberObjectInfo);
        if (objectId > 0L)
        {
          memberNameInfo._objectId = objectId;
          this.WriteObjectRef(memberNameInfo, objectId);
        }
        else
        {
          this._serWriter.WriteMemberNested(memberNameInfo);
          memberObjectInfo._objectId = objectId;
          memberNameInfo._objectId = objectId;
          this.WriteArray(memberObjectInfo, memberNameInfo, memberObjectInfo);
          objectInfo.ObjectEnd();
        }
      }
      else
      {
        if (this.WriteKnownValueClass(memberNameInfo, memberTypeNameInfo, memberData))
          return;
        if (type2 == (Type) null)
          type2 = this.GetType(obj);
        long objectId = this.Schedule(obj, assignUniqueIdToValueType, type2, memberObjectInfo);
        if (objectId < 0L)
        {
          memberObjectInfo._objectId = objectId;
          NameInfo nameInfo = this.TypeToNameInfo(memberObjectInfo);
          nameInfo._objectId = objectId;
          this.Write(memberObjectInfo, memberNameInfo, nameInfo);
          this.PutNameInfo(nameInfo);
          memberObjectInfo.ObjectEnd();
        }
        else
        {
          memberNameInfo._objectId = objectId;
          this.WriteObjectRef(memberNameInfo, objectId);
        }
      }
    }

    private void WriteArray(
      WriteObjectInfo objectInfo,
      NameInfo memberNameInfo,
      WriteObjectInfo memberObjectInfo)
    {
      bool flag1 = false;
      if (memberNameInfo == null)
      {
        memberNameInfo = this.TypeToNameInfo(objectInfo);
        flag1 = true;
      }
      memberNameInfo._isArray = true;
      long objectId = objectInfo._objectId;
      memberNameInfo._objectId = objectInfo._objectId;
      Array array1 = (Array) objectInfo._obj;
      Type elementType = objectInfo._objectType.GetElementType();
      WriteObjectInfo objectInfo1 = (WriteObjectInfo) null;
      if (!elementType.GetTypeInfo().IsPrimitive)
      {
        objectInfo1 = WriteObjectInfo.Serialize(elementType, this._surrogates, this._context, 
            this._serObjectInfoInit, this._formatterConverter, this._binder);
        objectInfo1._assemId = this.GetAssemblyId(objectInfo1);
      }
      NameInfo nameInfo1 = objectInfo1 == null ? this.TypeToNameInfo(elementType) : this.TypeToNameInfo(objectInfo1);
      nameInfo1._isArray = nameInfo1._type.IsArray;
      NameInfo nameInfo2 = memberNameInfo;
      nameInfo2._objectId = objectId;
      nameInfo2._isArray = true;
      nameInfo1._objectId = objectId;
      nameInfo1._transmitTypeOnMember = memberNameInfo._transmitTypeOnMember;
      nameInfo1._transmitTypeOnObject = memberNameInfo._transmitTypeOnObject;
      nameInfo1._isParentTypeOnObject = memberNameInfo._isParentTypeOnObject;
      int rank = array1.Rank;
      int[] numArray1 = new int[rank];
      int[] lowerBoundA = new int[rank];
      int[] numArray2 = new int[rank];
      for (int dimension = 0; dimension < rank; ++dimension)
      {
        numArray1[dimension] = array1.GetLength(dimension);
        lowerBoundA[dimension] = array1.GetLowerBound(dimension);
        numArray2[dimension] = array1.GetUpperBound(dimension);
      }
      InternalArrayTypeE internalArrayTypeE = !nameInfo1._isArray ? (rank != 1 ? InternalArrayTypeE.Rectangular : InternalArrayTypeE.Single) : (rank == 1 ? InternalArrayTypeE.Jagged : InternalArrayTypeE.Rectangular);
      nameInfo1._arrayEnum = internalArrayTypeE;
      if ((object) elementType == (object) Converter.s_typeofByte && rank == 1 && lowerBoundA[0] == 0)
      {
        this._serWriter.WriteObjectByteArray(memberNameInfo, nameInfo2, objectInfo1, nameInfo1, numArray1[0], lowerBoundA[0], (byte[]) array1);
      }
      else
      {
        if ((object) elementType == (object) Converter.s_typeofObject || Nullable.GetUnderlyingType(elementType) != (Type) null)
        {
          memberNameInfo._transmitTypeOnMember = true;
          nameInfo1._transmitTypeOnMember = true;
        }
        if (this.CheckTypeFormat(this._formatterEnums._typeFormat, FormatterTypeStyle.TypesAlways))
        {
          memberNameInfo._transmitTypeOnObject = true;
          nameInfo1._transmitTypeOnObject = true;
        }
        switch (internalArrayTypeE)
        {
          case InternalArrayTypeE.Single:
            this._serWriter.WriteSingleArray(memberNameInfo, nameInfo2, objectInfo1, nameInfo1, numArray1[0], lowerBoundA[0], array1);
            if (!Converter.IsWriteAsByteArray(nameInfo1._primitiveTypeEnum) || lowerBoundA[0] != 0)
            {
              object[] objArray = (object[]) null;
              if (!elementType.GetTypeInfo().IsValueType)
                objArray = (object[]) array1;
              int num = numArray2[0] + 1;
              for (int index = lowerBoundA[0]; index < num; ++index)
              {
                if (objArray == null)
                  this.WriteArrayMember(objectInfo, nameInfo1, array1.GetValue(index));
                else
                  this.WriteArrayMember(objectInfo, nameInfo1, objArray[index]);
              }
              this._serWriter.WriteItemEnd();
              break;
            }
            break;
          case InternalArrayTypeE.Jagged:
            nameInfo2._objectId = objectId;
            this._serWriter.WriteJaggedArray(memberNameInfo, nameInfo2, objectInfo1, nameInfo1, numArray1[0], lowerBoundA[0]);
            Array array2 = array1;
            for (int index = lowerBoundA[0]; index < numArray2[0] + 1; ++index)
              this.WriteArrayMember(objectInfo, nameInfo1, array2.GetValue(index));
            this._serWriter.WriteItemEnd();
            break;
          default:
            nameInfo2._objectId = objectId;
            this._serWriter.WriteRectangleArray(memberNameInfo, nameInfo2, objectInfo1, nameInfo1, rank, numArray1, lowerBoundA);
            bool flag2 = false;
            for (int index = 0; index < rank; ++index)
            {
              if (numArray1[index] == 0)
              {
                flag2 = true;
                break;
              }
            }
            if (!flag2)
              this.WriteRectangle(objectInfo, rank, numArray1, array1, nameInfo1, lowerBoundA);
            this._serWriter.WriteItemEnd();
            break;
        }
        this._serWriter.WriteObjectEnd(memberNameInfo, nameInfo2);
        this.PutNameInfo(nameInfo1);
        if (!flag1)
          return;
        this.PutNameInfo(memberNameInfo);
      }
    }

    private void WriteArrayMember(
      WriteObjectInfo objectInfo,
      NameInfo arrayElemTypeNameInfo,
      object data)
    {
      arrayElemTypeNameInfo._isArrayItem = true;
      if (this.CheckForNull(objectInfo, arrayElemTypeNameInfo, arrayElemTypeNameInfo, data))
        return;
      Type type = (Type) null;
      bool flag = false;
      if (arrayElemTypeNameInfo._transmitTypeOnMember)
        flag = true;
      if (!flag && !arrayElemTypeNameInfo.IsSealed)
      {
        type = this.GetType(data);
        if ((object) arrayElemTypeNameInfo._type != (object) type)
          flag = true;
      }
      NameInfo nameInfo1;
      if (flag)
      {
        if (type == (Type) null)
          type = this.GetType(data);
        nameInfo1 = this.TypeToNameInfo(type);
        nameInfo1._transmitTypeOnMember = true;
        nameInfo1._objectId = arrayElemTypeNameInfo._objectId;
        nameInfo1._assemId = arrayElemTypeNameInfo._assemId;
        nameInfo1._isArrayItem = true;
      }
      else
      {
        nameInfo1 = arrayElemTypeNameInfo;
        nameInfo1._isArrayItem = true;
      }
      if (!this.WriteKnownValueClass(arrayElemTypeNameInfo, nameInfo1, data))
      {
        object obj = data;
        bool assignUniqueIdToValueType = false;
        if ((object) arrayElemTypeNameInfo._type == (object) Converter.s_typeofObject)
          assignUniqueIdToValueType = true;
        long idRef = this.Schedule(obj, assignUniqueIdToValueType, nameInfo1._type);
        arrayElemTypeNameInfo._objectId = idRef;
        nameInfo1._objectId = idRef;
        if (idRef < 1L)
        {
          WriteObjectInfo objectInfo1 = WriteObjectInfo.Serialize(obj, this._surrogates, this._context, this._serObjectInfoInit, this._formatterConverter, this, this._binder);
          objectInfo1._objectId = idRef;
          objectInfo1._assemId = (object) arrayElemTypeNameInfo._type == (object) Converter.s_typeofObject || !(Nullable.GetUnderlyingType(arrayElemTypeNameInfo._type) == (Type) null) ? this.GetAssemblyId(objectInfo1) : nameInfo1._assemId;
          NameInfo nameInfo2 = this.TypeToNameInfo(objectInfo1);
          nameInfo2._objectId = idRef;
          objectInfo1._objectId = idRef;
          this.Write(objectInfo1, nameInfo1, nameInfo2);
          objectInfo1.ObjectEnd();
        }
        else
          this._serWriter.WriteItemObjectRef(arrayElemTypeNameInfo, (int) idRef);
      }
      if (!arrayElemTypeNameInfo._transmitTypeOnMember)
        return;
      this.PutNameInfo(nameInfo1);
    }

    private void WriteRectangle(
      WriteObjectInfo objectInfo,
      int rank,
      int[] maxA,
      Array array,
      NameInfo arrayElemNameTypeInfo,
      int[] lowerBoundA)
    {
      int[] numArray1 = new int[rank];
      int[] numArray2 = (int[]) null;
      bool flag1 = false;
      if (lowerBoundA != null)
      {
        for (int index = 0; index < rank; ++index)
        {
          if (lowerBoundA[index] != 0)
            flag1 = true;
        }
      }
      if (flag1)
        numArray2 = new int[rank];
      bool flag2 = true;
      while (flag2)
      {
        flag2 = false;
        if (flag1)
        {
          for (int index = 0; index < rank; ++index)
            numArray2[index] = numArray1[index] + lowerBoundA[index];
          this.WriteArrayMember(objectInfo, arrayElemNameTypeInfo, array.GetValue(numArray2));
        }
        else
          this.WriteArrayMember(objectInfo, arrayElemNameTypeInfo, array.GetValue(numArray1));
        for (int index1 = rank - 1; index1 > -1; --index1)
        {
          if (numArray1[index1] < maxA[index1] - 1)
          {
            ++numArray1[index1];
            if (index1 < rank - 1)
            {
              for (int index2 = index1 + 1; index2 < rank; ++index2)
                numArray1[index2] = 0;
            }
            flag2 = true;
            break;
          }
        }
      }
    }

    private object GetNext(out long objID)
    {
      if (this._objectQueue.Count == 0)
      {
        objID = 0L;
        return (object) null;
      }
      object next = this._objectQueue.Dequeue();
      object p1 = next is WriteObjectInfo ? ((WriteObjectInfo) next)._obj : next;
      bool firstTime;
      objID = this._idGenerator.HasId(p1, out firstTime);
      if (firstTime)
        throw new SerializationException(SR.Format(SR.Serialization_ObjNoID, p1));
      return next;
    }

    private long InternalGetId(
      object obj,
      bool assignUniqueIdToValueType,
      Type type,
      out bool isNew)
    {
      if (obj == this._previousObj)
      {
        isNew = false;
        return this._previousId;
      }
      this._idGenerator._currentCount = this._currentId;
      if (type != (Type) null && type.GetTypeInfo().IsValueType && !assignUniqueIdToValueType)
      {
        isNew = false;
        return (long) (-1 * this._currentId++);
      }
      ++this._currentId;
      long id = this._idGenerator.GetId(obj, out isNew);
      this._previousObj = obj;
      this._previousId = id;
      return id;
    }

    private long Schedule(object obj, bool assignUniqueIdToValueType, Type type)
    {
      return this.Schedule(obj, assignUniqueIdToValueType, type, (WriteObjectInfo) null);
    }

    private long Schedule(
      object obj,
      bool assignUniqueIdToValueType,
      Type type,
      WriteObjectInfo objectInfo)
    {
      long num = 0;
      if (obj != null)
      {
        bool isNew;
        num = this.InternalGetId(obj, assignUniqueIdToValueType, type, out isNew);
        if (isNew && num > 0L)
          this._objectQueue.Enqueue((object) objectInfo ?? obj);
      }
      return num;
    }

    private bool WriteKnownValueClass(NameInfo memberNameInfo, NameInfo typeNameInfo, object data)
    {
      if ((object) typeNameInfo._type == (object) Converter.s_typeofString)
      {
        this.WriteString(memberNameInfo, typeNameInfo, data);
      }
      else
      {
        if (typeNameInfo._primitiveTypeEnum == InternalPrimitiveTypeE.Invalid)
          return false;
        if (typeNameInfo._isArray)
          this._serWriter.WriteItem(memberNameInfo, typeNameInfo, data);
        else
          this._serWriter.WriteMember(memberNameInfo, typeNameInfo, data);
      }
      return true;
    }

    private void WriteObjectRef(NameInfo nameInfo, long objectId)
    {
      this._serWriter.WriteMemberObjectRef(nameInfo, (int) objectId);
    }

    private void WriteString(NameInfo memberNameInfo, NameInfo typeNameInfo, object stringObject)
    {
      bool isNew = true;
      long objectId = -1;
      if (!this.CheckTypeFormat(this._formatterEnums._typeFormat, FormatterTypeStyle.XsdString))
        objectId = this.InternalGetId(stringObject, false, (Type) null, out isNew);
      typeNameInfo._objectId = objectId;
      if (isNew || objectId < 0L)
        this._serWriter.WriteMemberString(memberNameInfo, typeNameInfo, (string) stringObject);
      else
        this.WriteObjectRef(memberNameInfo, objectId);
    }

    private bool CheckForNull(
      WriteObjectInfo objectInfo,
      NameInfo memberNameInfo,
      NameInfo typeNameInfo,
      object data)
    {
      bool flag = data == null;
      if (flag && (this._formatterEnums._serializerTypeEnum == InternalSerializerTypeE.Binary || memberNameInfo._isArrayItem || memberNameInfo._transmitTypeOnObject || memberNameInfo._transmitTypeOnMember || objectInfo._isSi || this.CheckTypeFormat(this._formatterEnums._typeFormat, FormatterTypeStyle.TypesAlways)))
      {
        if (typeNameInfo._isArrayItem)
        {
          if (typeNameInfo._arrayEnum == InternalArrayTypeE.Single)
            this._serWriter.WriteDelayedNullItem();
          else
            this._serWriter.WriteNullItem(memberNameInfo, typeNameInfo);
        }
        else
          this._serWriter.WriteNullMember(memberNameInfo, typeNameInfo);
      }
      return flag;
    }

    private void WriteSerializedStreamHeader(long topId, long headerId)
    {
      this._serWriter.WriteSerializationHeader((int) topId, (int) headerId, 1, 0);
    }

    private NameInfo TypeToNameInfo(
      Type type,
      WriteObjectInfo objectInfo,
      InternalPrimitiveTypeE code,
      NameInfo nameInfo)
    {
      if (nameInfo == null)
        nameInfo = this.GetNameInfo();
      else
        nameInfo.Init();
      if (code == InternalPrimitiveTypeE.Invalid && objectInfo != null)
      {
        nameInfo.NIname = objectInfo.GetTypeFullName();
        nameInfo._assemId = objectInfo._assemId;
      }
      nameInfo._primitiveTypeEnum = code;
      nameInfo._type = type;
      return nameInfo;
    }

    private NameInfo TypeToNameInfo(Type type)
    {
      return this.TypeToNameInfo(type, (WriteObjectInfo) null, this.ToCode(type), (NameInfo) null);
    }

    private NameInfo TypeToNameInfo(WriteObjectInfo objectInfo)
    {
      return this.TypeToNameInfo(objectInfo._objectType, objectInfo, this.ToCode(objectInfo._objectType), (NameInfo) null);
    }

    private NameInfo TypeToNameInfo(WriteObjectInfo objectInfo, NameInfo nameInfo)
    {
      return this.TypeToNameInfo(objectInfo._objectType, objectInfo, this.ToCode(objectInfo._objectType), nameInfo);
    }

    private void TypeToNameInfo(Type type, NameInfo nameInfo)
    {
      this.TypeToNameInfo(type, (WriteObjectInfo) null, this.ToCode(type), nameInfo);
    }

    private NameInfo MemberToNameInfo(string name)
    {
      NameInfo nameInfo = this.GetNameInfo();
      nameInfo.NIname = name;
      return nameInfo;
    }

    internal InternalPrimitiveTypeE ToCode(Type type)
    {
      if ((object) this._previousType == (object) type)
        return this._previousCode;
      InternalPrimitiveTypeE code = Converter.ToCode(type);
      if (code != InternalPrimitiveTypeE.Invalid)
      {
        this._previousType = type;
        this._previousCode = code;
      }
      return code;
    }

    private long GetAssemblyId(WriteObjectInfo objectInfo)
    {
      if (this._assemblyToIdTable == null)
        this._assemblyToIdTable = new Dictionary<string, long>();
      long assemId = 0;
      string assemblyString1 = objectInfo.GetAssemblyString();
      string assemblyString2 = assemblyString1;
      if (assemblyString1.Length == 0)
        assemId = 0L;
      else if (assemblyString1.Equals(Converter.s_urtAssemblyString) || assemblyString1.Equals(Converter.s_urtAlternativeAssemblyString))
      {
        assemId = 0L;
      }
      else
      {
        bool isNew = false;
        if (this._assemblyToIdTable.TryGetValue(assemblyString1, out assemId))
        {
          isNew = false;
        }
        else
        {
          assemId = this.InternalGetId((object) ("___AssemblyString___" + assemblyString1), false, (Type) null, out isNew);
          this._assemblyToIdTable[assemblyString1] = assemId;
        }
        this._serWriter.WriteAssembly(objectInfo._objectType, assemblyString2, (int) assemId, isNew);
      }
      return assemId;
    }

    private Type GetType(object obj) => obj.GetType();

    private NameInfo GetNameInfo()
    {
      NameInfo nameInfo;
      if (!this._niPool.IsEmpty())
      {
        nameInfo = (NameInfo) this._niPool.Pop();
        nameInfo.Init();
      }
      else
        nameInfo = new NameInfo();
      return nameInfo;
    }

    private bool CheckTypeFormat(FormatterTypeStyle test, FormatterTypeStyle want)
    {
      return (test & want) == want;
    }

    private void PutNameInfo(NameInfo nameInfo) => this._niPool.Push((object) nameInfo);
  }
}
