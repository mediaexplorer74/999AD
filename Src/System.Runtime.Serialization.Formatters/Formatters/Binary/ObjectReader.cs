// Decompiled with JetBrains decompiler
// Type: System.Runtime.Serialization.Formatters.Binary.ObjectReader
// Assembly: System.Runtime.Serialization.Formatters, Version=4.0.3.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 59FEC222-A9B8-4506-99C2-FAFFD4E25D28
// Assembly location: C:\Users\Admin\Desktop\RE\1\System.Runtime.Serialization.Formatters.dll

using System.Globalization;
using System.IO;
using System.Reflection;


namespace System.Runtime.Serialization.Formatters.Binary
{
  internal sealed class ObjectReader
  {
    internal Stream _stream;
    internal ISurrogateSelector _surrogates;
    internal StreamingContext _context;
    internal ObjectManager _objectManager;
    internal InternalFE _formatterEnums;
    internal SerializationBinder _binder;
    internal long _topId;
    internal bool _isSimpleAssembly;
    internal object _topObject;
    internal SerObjectInfoInit _serObjectInfoInit;
    internal IFormatterConverter _formatterConverter;
    internal SerStack _stack;
    private SerStack _valueFixupStack;
    internal object[] _crossAppDomainArray;
    private bool _fullDeserialization;
    private const int ThresholdForValueTypeIds = 2147483647;
    private bool _oldFormatDetected;
    private IntSizedArray _valTypeObjectIdTable;
    private readonly NameCache _typeCache = new NameCache();
    private string _previousAssemblyString;
    private string _previousName;
    private Type _previousType;

    private SerStack ValueFixupStack
    {
      get
      {
        return this._valueFixupStack ?? (this._valueFixupStack = new SerStack("ValueType Fixup Stack"));
      }
    }

    internal object TopObject
    {
      get => this._topObject;
      set
      {
        this._topObject = value;
        if (this._objectManager == null)
          return;
        this._objectManager.TopObject = value;
      }
    }

    internal ObjectReader(
      Stream stream,
      ISurrogateSelector selector,
      StreamingContext context,
      InternalFE formatterEnums,
      SerializationBinder binder)
    {
      this._stream = stream != null ? stream : throw new ArgumentNullException(nameof (stream));
      this._surrogates = selector;
      this._context = context;
      this._binder = binder;
      this._formatterEnums = formatterEnums;
    }

    internal object Deserialize(BinaryParser serParser, bool fCheck)
    {
      if (serParser == null)
        throw new ArgumentNullException(nameof (serParser));
      this._fullDeserialization = false;
      this.TopObject = (object) null;
      this._topId = 0L;
      this._isSimpleAssembly = this._formatterEnums._assemblyFormat == FormatterAssemblyStyle.Simple;
      if (this._fullDeserialization)
      {
        this._objectManager = new ObjectManager(this._surrogates, this._context, false, false);
        this._serObjectInfoInit = new SerObjectInfoInit();
      }
      serParser.Run();
      if (this._fullDeserialization)
        this._objectManager.DoFixups();
      if (this.TopObject == null)
        throw new SerializationException(SR.Serialization_TopObject);
      if (this.HasSurrogate(this.TopObject.GetType()) && this._topId != 0L)
        this.TopObject = this._objectManager.GetObject(this._topId);
      if (this.TopObject is IObjectReference)
        this.TopObject = ((IObjectReference) this.TopObject).GetRealObject(this._context);
      if (this._fullDeserialization)
        this._objectManager.RaiseDeserializationEvent();
      return this.TopObject;
    }

    private bool HasSurrogate(Type t)
    {
      return this._surrogates != null && this._surrogates.GetSurrogate(t, this._context, out ISurrogateSelector _) != null;
    }

        private void CheckSerializable(Type t)
        {
            if (!t.GetTypeInfo().IsSerializable && !this.HasSurrogate(t))
                throw new SerializationException(
                    string.Format((IFormatProvider)CultureInfo.InvariantCulture, SR.Serialization_NonSerType, 
                       (object)t.FullName, (object)t.AssemblyQualifiedName));

        }

    private void InitFullDeserialization()
    {
      this._fullDeserialization = true;
      this._stack = new SerStack("ObjectReader Object Stack");
      this._objectManager = new ObjectManager(this._surrogates, this._context, false, false);
      if (this._formatterConverter != null)
        return;
      this._formatterConverter = (IFormatterConverter) new FormatterConverter();
    }

    internal object CrossAppDomainArray(int index) => this._crossAppDomainArray[index];

    internal ReadObjectInfo CreateReadObjectInfo(Type objectType)
    {
      return ReadObjectInfo.Create(objectType, this._surrogates, this._context, 
          this._objectManager, this._serObjectInfoInit, this._formatterConverter, this._isSimpleAssembly);
    }

    internal ReadObjectInfo CreateReadObjectInfo(
      Type objectType,
      string[] memberNames,
      Type[] memberTypes)
    {
      return ReadObjectInfo.Create(objectType, memberNames, memberTypes, this._surrogates,
          this._context, this._objectManager, this._serObjectInfoInit, this._formatterConverter, this._isSimpleAssembly);
    }

    internal void Parse(ParseRecord pr)
    {
      switch (pr._parseTypeEnum)
      {
        case InternalParseTypeE.SerializedStreamHeader:
          this.ParseSerializedStreamHeader(pr);
          break;
        case InternalParseTypeE.Object:
          this.ParseObject(pr);
          break;
        case InternalParseTypeE.Member:
          this.ParseMember(pr);
          break;
        case InternalParseTypeE.ObjectEnd:
          this.ParseObjectEnd(pr);
          break;
        case InternalParseTypeE.MemberEnd:
          this.ParseMemberEnd(pr);
          break;
        case InternalParseTypeE.SerializedStreamHeaderEnd:
          this.ParseSerializedStreamHeaderEnd(pr);
          break;
        case InternalParseTypeE.Envelope:
          break;
        case InternalParseTypeE.EnvelopeEnd:
          break;
        case InternalParseTypeE.Body:
          break;
        case InternalParseTypeE.BodyEnd:
          break;
        default:
          throw new SerializationException(SR.Format(SR.Serialization_XMLElement, (object) pr._name));
      }
    }

    private void ParseError(ParseRecord processing, ParseRecord onStack)
    {
      throw new SerializationException(SR.Format(SR.Serialization_ParseError,
          (object) (onStack._name + " " + (object) onStack._parseTypeEnum + " "
          + processing._name + " " + (object) processing._parseTypeEnum)));
    }

    private void ParseSerializedStreamHeader(ParseRecord pr) => this._stack.Push((object) pr);

    private void ParseSerializedStreamHeaderEnd(ParseRecord pr) => this._stack.Pop();

    private void ParseObject(ParseRecord pr)
    {
      if (!this._fullDeserialization)
        this.InitFullDeserialization();
      if (pr._objectPositionEnum == InternalObjectPositionE.Top)
        this._topId = pr._objectId;
      if (pr._parseTypeEnum == InternalParseTypeE.Object)
        this._stack.Push((object) pr);
      if (pr._objectTypeEnum == InternalObjectTypeE.Array)
        this.ParseArray(pr);
      else if (pr._dtType == (Type) null)
        pr._newObj = (object) new TypeLoadExceptionHolder(pr._keyDt);
      else if ((object) pr._dtType == (object) Converter.s_typeofString)
      {
        if (pr._value == null)
          return;
        pr._newObj = (object) pr._value;
        if (pr._objectPositionEnum == InternalObjectPositionE.Top)
        {
          this.TopObject = pr._newObj;
        }
        else
        {
          this._stack.Pop();
          this.RegisterObject(pr._newObj, pr, (ParseRecord) this._stack.Peek());
        }
      }
      else
      {
        this.CheckSerializable(pr._dtType);
        pr._newObj = FormatterServices.GetUninitializedObject(pr._dtType);
        this._objectManager.RaiseOnDeserializingEvent(pr._newObj);
        if (pr._newObj == null)
          throw new SerializationException(SR.Format(SR.Serialization_TopObjectInstantiate, (object) pr._dtType));
        if (pr._objectPositionEnum == InternalObjectPositionE.Top)
          this.TopObject = pr._newObj;
        if (pr._objectInfo != null)
          return;
        pr._objectInfo = ReadObjectInfo.Create(pr._dtType, this._surrogates,
            this._context, this._objectManager, this._serObjectInfoInit, 
            this._formatterConverter, this._isSimpleAssembly);
      }
    }

    private void ParseObjectEnd(ParseRecord pr)
    {
      ParseRecord parseRecord1 = (ParseRecord) this._stack.Peek() ?? pr;
      if (parseRecord1._objectPositionEnum == InternalObjectPositionE.Top 
                && (object) parseRecord1._dtType == (object) Converter.s_typeofString)
      {
        parseRecord1._newObj = (object) parseRecord1._value;
        this.TopObject = parseRecord1._newObj;
      }
      else
      {
        this._stack.Pop();
        ParseRecord parseRecord2 = (ParseRecord) this._stack.Peek();
        if (parseRecord1._newObj == null)
          return;
        if (parseRecord1._objectTypeEnum == InternalObjectTypeE.Array)
        {
          if (parseRecord1._objectPositionEnum == InternalObjectPositionE.Top)
            this.TopObject = parseRecord1._newObj;
          this.RegisterObject(parseRecord1._newObj, parseRecord1, parseRecord2);
        }
        else
        {
          parseRecord1._objectInfo.PopulateObjectMembers(parseRecord1._newObj, parseRecord1._memberData);
          if (!parseRecord1._isRegistered && parseRecord1._objectId > 0L)
            this.RegisterObject(parseRecord1._newObj, parseRecord1, parseRecord2);
          if (parseRecord1._isValueTypeFixup)
            ((ValueFixup) this.ValueFixupStack.Pop()).Fixup(parseRecord1, parseRecord2);
          if (parseRecord1._objectPositionEnum == InternalObjectPositionE.Top)
            this.TopObject = parseRecord1._newObj;
          parseRecord1._objectInfo.ObjectEnd();
        }
      }
    }

    private void ParseArray(ParseRecord pr)
    {
      long objectId = pr._objectId;
      if (pr._arrayTypeEnum == InternalArrayTypeE.Base64)
      {
        pr._newObj = pr._value.Length > 0 ? (object) Convert.FromBase64String(pr._value) : (object) Array.Empty<byte>();
        if (this._stack.Peek() == pr)
          this._stack.Pop();
        if (pr._objectPositionEnum == InternalObjectPositionE.Top)
          this.TopObject = pr._newObj;
        ParseRecord objectPr = (ParseRecord) this._stack.Peek();
        this.RegisterObject(pr._newObj, pr, objectPr);
      }
      else if (pr._newObj != null && Converter.IsWriteAsByteArray(pr._arrayElementTypeCode))
      {
        if (pr._objectPositionEnum == InternalObjectPositionE.Top)
          this.TopObject = pr._newObj;
        ParseRecord objectPr = (ParseRecord) this._stack.Peek();
        this.RegisterObject(pr._newObj, pr, objectPr);
      }
      else if (pr._arrayTypeEnum == InternalArrayTypeE.Jagged || pr._arrayTypeEnum == InternalArrayTypeE.Single)
      {
        bool flag = true;
        if (pr._lowerBoundA == null || pr._lowerBoundA[0] == 0)
        {
          if ((object) pr._arrayElementType == (object) Converter.s_typeofString)
          {
            pr._objectA = (object[]) new string[pr._lengthA[0]];
            pr._newObj = (object) pr._objectA;
            flag = false;
          }
          else if ((object) pr._arrayElementType == (object) Converter.s_typeofObject)
          {
            pr._objectA = new object[pr._lengthA[0]];
            pr._newObj = (object) pr._objectA;
            flag = false;
          }
          else if (pr._arrayElementType != (Type) null)
            pr._newObj = (object) Array.CreateInstance(pr._arrayElementType, pr._lengthA[0]);
          pr._isLowerBound = false;
        }
        else
        {
          if (pr._arrayElementType != (Type) null)
            pr._newObj = (object) Array.CreateInstance(pr._arrayElementType, pr._lengthA, pr._lowerBoundA);
          pr._isLowerBound = true;
        }
        if (pr._arrayTypeEnum == InternalArrayTypeE.Single)
        {
          if (!pr._isLowerBound && Converter.IsWriteAsByteArray(pr._arrayElementTypeCode))
            pr._primitiveArray = new PrimitiveArray(pr._arrayElementTypeCode, (Array) pr._newObj);
                    else if (flag && pr._arrayElementType != (Type)null 
                        && pr._arrayElementType.GetTypeInfo().IsValueType 
                        && !pr._isLowerBound)
            pr._objectA = (object[]) pr._newObj;
        }
        pr._indexMap = new int[1];
      }
      else
      {
        if (pr._arrayTypeEnum != InternalArrayTypeE.Rectangular)
          throw new SerializationException(SR.Format(SR.Serialization_ArrayType, (object) pr._arrayTypeEnum));
        pr._isLowerBound = false;
        if (pr._lowerBoundA != null)
        {
          for (int index = 0; index < pr._rank; ++index)
          {
            if (pr._lowerBoundA[index] != 0)
              pr._isLowerBound = true;
          }
        }
        if (pr._arrayElementType != (Type) null)
          pr._newObj = !pr._isLowerBound
                        ? (object) Array.CreateInstance(pr._arrayElementType, pr._lengthA)
                        : (object) Array.CreateInstance(pr._arrayElementType, pr._lengthA, pr._lowerBoundA);
        int num = 1;
        for (int index = 0; index < pr._rank; ++index)
          num *= pr._lengthA[index];
        pr._indexMap = new int[pr._rank];
        pr._rectangularMap = new int[pr._rank];
        pr._linearlength = num;
      }
    }

    private void NextRectangleMap(ParseRecord pr)
    {
      for (int index1 = pr._rank - 1; index1 > -1; --index1)
      {
        if (pr._rectangularMap[index1] < pr._lengthA[index1] - 1)
        {
          ++pr._rectangularMap[index1];
          if (index1 < pr._rank - 1)
          {
            for (int index2 = index1 + 1; index2 < pr._rank; ++index2)
              pr._rectangularMap[index2] = 0;
          }
          Array.Copy((Array) pr._rectangularMap, 0, (Array) pr._indexMap, 0, pr._rank);
          break;
        }
      }
    }

    private void ParseArrayMember(ParseRecord pr)
    {
      ParseRecord parseRecord = (ParseRecord) this._stack.Peek();
      if (parseRecord._arrayTypeEnum == InternalArrayTypeE.Rectangular)
      {
        if (parseRecord._memberIndex > 0)
          this.NextRectangleMap(parseRecord);
        if (parseRecord._isLowerBound)
        {
          for (int index = 0; index < parseRecord._rank; ++index)
            parseRecord._indexMap[index] = parseRecord._rectangularMap[index] + parseRecord._lowerBoundA[index];
        }
      }
      else
        parseRecord._indexMap[0] = !parseRecord._isLowerBound
                    ? parseRecord._memberIndex 
                    : parseRecord._lowerBoundA[0] + parseRecord._memberIndex;

      if (pr._memberValueEnum == InternalMemberValueE.Reference)
      {
        object obj = this._objectManager.GetObject(pr._idRef);
        if (obj == null)
        {
          int[] numArray = new int[parseRecord._rank];
          Array.Copy((Array) parseRecord._indexMap, 0, (Array) numArray, 0, parseRecord._rank);
          this._objectManager.RecordArrayElementFixup(parseRecord._objectId, numArray, pr._idRef);
        }
        else if (parseRecord._objectA != null)
          parseRecord._objectA[parseRecord._indexMap[0]] = obj;
        else
          ((Array) parseRecord._newObj).SetValue(obj, parseRecord._indexMap);
      }
      else if (pr._memberValueEnum == InternalMemberValueE.Nested)
      {
        if (pr._dtType == (Type) null)
          pr._dtType = parseRecord._arrayElementType;
        this.ParseObject(pr);
        this._stack.Push((object) pr);
        if (parseRecord._arrayElementType != (Type) null)
        {
          if (parseRecord._arrayElementType.GetTypeInfo().IsValueType
                        && pr._arrayElementTypeCode == InternalPrimitiveTypeE.Invalid)
          {
            pr._isValueTypeFixup = true;
            this.ValueFixupStack.Push((object) new ValueFixup((Array) parseRecord._newObj, parseRecord._indexMap));
          }
          else if (parseRecord._objectA != null)
            parseRecord._objectA[parseRecord._indexMap[0]] = pr._newObj;
          else
            ((Array) parseRecord._newObj).SetValue(pr._newObj, parseRecord._indexMap);
        }
      }
      else if (pr._memberValueEnum == InternalMemberValueE.InlineValue)
      {
        if ((object) parseRecord._arrayElementType == (object) Converter.s_typeofString 
                    || (object) pr._dtType == (object) Converter.s_typeofString)
        {
          this.ParseString(pr, parseRecord);
          if (parseRecord._objectA != null)
            parseRecord._objectA[parseRecord._indexMap[0]] = (object) pr._value;
          else
            ((Array) parseRecord._newObj).SetValue((object) pr._value, parseRecord._indexMap);
        }
        else if (parseRecord._isArrayVariant)
        {
          if (pr._keyDt == null)
            throw new SerializationException(SR.Serialization_ArrayTypeObject);
          object obj;
          if ((object) pr._dtType == (object) Converter.s_typeofString)
          {
            this.ParseString(pr, parseRecord);
            obj = (object) pr._value;
          }
          else if ((Enum) pr._dtTypeCode == (Enum) InternalPrimitiveTypeE.Invalid)
          {
            this.CheckSerializable(pr._dtType);
            obj = FormatterServices.GetUninitializedObject(pr._dtType);
          }
          else
            obj = pr._varValue != null ? pr._varValue : Converter.FromString(pr._value, pr._dtTypeCode);
          if (parseRecord._objectA != null)
            parseRecord._objectA[parseRecord._indexMap[0]] = obj;
          else
            ((Array) parseRecord._newObj).SetValue(obj, parseRecord._indexMap);
        }
        else if (parseRecord._primitiveArray != null)
        {
          parseRecord._primitiveArray.SetValue(pr._value, parseRecord._indexMap[0]);
        }
        else
        {
          object obj = pr._varValue != null ? pr._varValue : Converter.FromString(pr._value, 
              parseRecord._arrayElementTypeCode);
          if (parseRecord._objectA != null)
            parseRecord._objectA[parseRecord._indexMap[0]] = obj;
          else
            ((Array) parseRecord._newObj).SetValue(obj, parseRecord._indexMap);
        }
      }
      else if (pr._memberValueEnum == InternalMemberValueE.Null)
        parseRecord._memberIndex += pr._consecutiveNullArrayEntryCount - 1;
      else
        this.ParseError(pr, parseRecord);
      ++parseRecord._memberIndex;
    }

    private void ParseArrayMemberEnd(ParseRecord pr)
    {
      if (pr._memberValueEnum != InternalMemberValueE.Nested)
        return;
      this.ParseObjectEnd(pr);
    }

    private void ParseMember(ParseRecord pr)
    {
      ParseRecord parseRecord = (ParseRecord) this._stack.Peek();
      string name = parseRecord?._name;
      switch (pr._memberTypeEnum)
      {
        case InternalMemberTypeE.Item:
          this.ParseArrayMember(pr);
          break;
        default:
          if (pr._dtType == (Type) null && parseRecord._objectInfo._isTyped)
          {
            pr._dtType = parseRecord._objectInfo.GetType(pr._name);
            if (pr._dtType != (Type) null)
              pr._dtTypeCode = Converter.ToCode(pr._dtType);
          }
          if (pr._memberValueEnum == InternalMemberValueE.Null)
          {
            parseRecord._objectInfo.AddValue(pr._name, (object) null, ref parseRecord._si,
                ref parseRecord._memberData);
            break;
          }
          if (pr._memberValueEnum == InternalMemberValueE.Nested)
          {
            this.ParseObject(pr);
            this._stack.Push((object) pr);
            if (pr._objectInfo != null && pr._objectInfo._objectType != (Type) null 
                            && pr._objectInfo._objectType.GetTypeInfo().IsValueType)
            {
              pr._isValueTypeFixup = true;
              this.ValueFixupStack.Push((object) new ValueFixup(parseRecord._newObj, pr._name, parseRecord._objectInfo));
              break;
            }
            parseRecord._objectInfo.AddValue(pr._name, pr._newObj, ref parseRecord._si, ref parseRecord._memberData);
            break;
          }
          if (pr._memberValueEnum == InternalMemberValueE.Reference)
          {
            object obj = this._objectManager.GetObject(pr._idRef);
            if (obj == null)
            {
              parseRecord._objectInfo.AddValue(pr._name, (object) null, ref parseRecord._si,
                  ref parseRecord._memberData);
              parseRecord._objectInfo.RecordFixup(parseRecord._objectId, pr._name, pr._idRef);
              break;
            }
            parseRecord._objectInfo.AddValue(pr._name, obj, ref parseRecord._si,
                ref parseRecord._memberData);
            break;
          }
          if (pr._memberValueEnum == InternalMemberValueE.InlineValue)
          {
            if ((object) pr._dtType == (object) Converter.s_typeofString)
            {
              this.ParseString(pr, parseRecord);
              parseRecord._objectInfo.AddValue(pr._name, (object) pr._value, ref parseRecord._si, ref 
                  parseRecord._memberData);
              break;
            }
            if (pr._dtTypeCode == InternalPrimitiveTypeE.Invalid)
            {
              if (pr._arrayTypeEnum == InternalArrayTypeE.Base64)
              {
                parseRecord._objectInfo.AddValue(pr._name, (object) Convert.FromBase64String(pr._value), 
                    ref parseRecord._si, ref parseRecord._memberData);
                break;
              }
              if ((object) pr._dtType == (object) Converter.s_typeofObject)
                throw new SerializationException(SR.Format(SR.Serialization_TypeMissing, (object) pr._name));
              this.ParseString(pr, parseRecord);
              if ((object) pr._dtType == (object) Converter.s_typeofSystemVoid)
              {
                parseRecord._objectInfo.AddValue(pr._name, (object) pr._dtType, ref parseRecord._si, 
                    ref parseRecord._memberData);
                break;
              }
              if (!parseRecord._objectInfo._isSi)
                break;
              parseRecord._objectInfo.AddValue(pr._name, (object) pr._value, ref parseRecord._si,
                  ref parseRecord._memberData);
              break;
            }
            object obj = pr._varValue != null ? pr._varValue : Converter.FromString(pr._value, pr._dtTypeCode);
            parseRecord._objectInfo.AddValue(pr._name, obj, ref parseRecord._si, ref parseRecord._memberData);
            break;
          }
          this.ParseError(pr, parseRecord);
          break;
      }
    }

    private void ParseMemberEnd(ParseRecord pr)
    {
      switch (pr._memberTypeEnum)
      {
        case InternalMemberTypeE.Field:
          if (pr._memberValueEnum != InternalMemberValueE.Nested)
            break;
          this.ParseObjectEnd(pr);
          break;
        case InternalMemberTypeE.Item:
          this.ParseArrayMemberEnd(pr);
          break;
        default:
          this.ParseError(pr, (ParseRecord) this._stack.Peek());
          break;
      }
    }

    private void ParseString(ParseRecord pr, ParseRecord parentPr)
    {
      if (pr._isRegistered || pr._objectId <= 0L)
        return;
      this.RegisterObject((object) pr._value, pr, parentPr, true);
    }

    private void RegisterObject(object obj, ParseRecord pr, ParseRecord objectPr)
    {
      this.RegisterObject(obj, pr, objectPr, false);
    }

    private void RegisterObject(object obj, ParseRecord pr, ParseRecord objectPr, bool bIsString)
    {
      if (pr._isRegistered)
        return;
      pr._isRegistered = true;
      long idOfContainingObj = 0;
      MemberInfo member = (MemberInfo) null;
      int[] arrayIndex = (int[]) null;
      if (objectPr != null)
      {
        arrayIndex = objectPr._indexMap;
        idOfContainingObj = objectPr._objectId;
        if (objectPr._objectInfo != null && !objectPr._objectInfo._isSi)
          member = objectPr._objectInfo.GetMemberInfo(pr._name);
      }
      SerializationInfo si = pr._si;
      if (bIsString)
        this._objectManager.RegisterString((string) obj, pr._objectId, si, idOfContainingObj, member);
      else
        this._objectManager.RegisterObject(obj, pr._objectId, si, idOfContainingObj, member, arrayIndex);
    }

    internal long GetId(long objectId)
    {
      if (!this._fullDeserialization)
        this.InitFullDeserialization();
      if (objectId > 0L)
        return objectId;
      if (!this._oldFormatDetected && objectId != -1L)
        return -1L * objectId;
      this._oldFormatDetected = true;
      if (this._valTypeObjectIdTable == null)
        this._valTypeObjectIdTable = new IntSizedArray();
      long id;
      if ((id = (long) this._valTypeObjectIdTable[(int) objectId]) == 0L)
      {
        id = (long) int.MaxValue + objectId;
        this._valTypeObjectIdTable[(int) objectId] = (int) id;
      }
      return id;
    }

    internal Type Bind(string assemblyString, string typeString)
    {
      Type type = (Type) null;
      if (this._binder != null)
        type = this._binder.BindToType(assemblyString, typeString);
      if (type == (Type) null)
        type = this.FastBindToType(assemblyString, typeString);
      return type;
    }

    internal Type FastBindToType(string assemblyName, string typeName)
    {
      Type type = (Type) null;
      ObjectReader.TypeNAssembly typeNassembly = (ObjectReader.TypeNAssembly) this._typeCache.GetCachedValue(typeName);
      if (typeNassembly == null || typeNassembly.AssemblyName != assemblyName)
      {
        if (assemblyName == null)
          return (Type) null;
        Assembly assembly = (Assembly) null;
        AssemblyName assemblyName1;
        try
        {
          assemblyName1 = new AssemblyName(assemblyName);
        }
        catch
        {
          return (Type) null;
        }
        if (this._isSimpleAssembly)
        {
          assembly = ObjectReader.ResolveSimpleAssemblyName(assemblyName1);
        }
        else
        {
          try
          {
            assembly = Assembly.Load(assemblyName1);
          }
          catch
          {
          }
        }
        if (Assembly.Equals(assembly, (Assembly) null))
          return (Type) null;
        if (this._isSimpleAssembly)
          ObjectReader.GetSimplyNamedTypeFromAssembly(assembly, typeName, ref type);
        else
          type = FormatterServices.GetTypeFromAssembly(assembly, typeName);
        if (type == (Type) null)
          return (Type) null;
              
        ObjectReader.CheckTypeForwardedTo(assembly, type.GetTypeInfo().Assembly, type);
        typeNassembly = new ObjectReader.TypeNAssembly();
        typeNassembly.Type = type;
        typeNassembly.AssemblyName = assemblyName;
        this._typeCache.SetCachedValue((object) typeNassembly);
      }
      return typeNassembly.Type;
    }

        private static Assembly ResolveSimpleAssemblyName(AssemblyName assemblyName)
        {
            try
            {
                return Assembly.Load(assemblyName);
            }
            catch
            {
            }
            if (assemblyName != null)
            {
                try
                {
                    return Assembly.Load(new AssemblyName(assemblyName.Name));
                }
                catch
                {
                }
            }
            return (Assembly)null;
        }

    private static void GetSimplyNamedTypeFromAssembly(
      Assembly assm,
      string typeName,
      ref Type type)
    {
      try
      {
        type = FormatterServices.GetTypeFromAssembly(assm, typeName);
      }
      catch (TypeLoadException ex)
      {
      }
      catch (FileNotFoundException ex)
      {
      }
      catch (FileLoadException ex)
      {
      }
      catch (BadImageFormatException ex)
      {
      }
      if (!(type == (Type) null))
        return;
      type = Type.GetType
      (
          typeName,
          false,//new Func<AssemblyName, Assembly>(ObjectReader.ResolveSimpleAssemblyName),
          false//new Func<Assembly, string, bool, Type>(new ObjectReader.TopLevelAssemblyTypeResolver(assm).ResolveType )
      );//, false);
    }

    internal Type GetType(BinaryAssemblyInfo assemblyInfo, string name)
    {
      Type type;
      if (this._previousName != null && this._previousName.Length == name.Length 
                && this._previousName.Equals(name) && this._previousAssemblyString != null && this._previousAssemblyString.Length == assemblyInfo._assemblyString.Length && this._previousAssemblyString.Equals(assemblyInfo._assemblyString))
      {
        type = this._previousType;
      }
      else
      {
        type = this.Bind(assemblyInfo._assemblyString, name);
        if (type == (Type) null)
        {
          Assembly assembly = assemblyInfo.GetAssembly();
          if (this._isSimpleAssembly)
            ObjectReader.GetSimplyNamedTypeFromAssembly(assembly, name, ref type);
          else
            type = FormatterServices.GetTypeFromAssembly(assembly, name);
          if (type != (Type) null)
            ObjectReader.CheckTypeForwardedTo(assembly, type.GetTypeInfo().Assembly, type);
        }
        this._previousAssemblyString = assemblyInfo._assemblyString;
        this._previousName = name;
        this._previousType = type;
      }
      return type;
    }

    private static void CheckTypeForwardedTo(
      Assembly sourceAssembly,
      Assembly destAssembly,
      Type resolvedType)
    {
    }

    internal sealed class TypeNAssembly
    {
      public Type Type;
      public string AssemblyName;
    }

    internal sealed class TopLevelAssemblyTypeResolver
    {
      private readonly Assembly _topLevelAssembly;

      public TopLevelAssemblyTypeResolver(Assembly topLevelAssembly)
      {
        this._topLevelAssembly = topLevelAssembly;
      }

      public Type ResolveType(Assembly assembly, string simpleTypeName, bool ignoreCase)
      {
        if (Assembly.Equals(assembly, (Assembly) null))
          assembly = this._topLevelAssembly;
        return assembly.GetType(simpleTypeName, false, ignoreCase);
      }
    }
  }
}
