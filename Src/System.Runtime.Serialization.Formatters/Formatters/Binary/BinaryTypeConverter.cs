// Decompiled with JetBrains decompiler
// Type: System.Runtime.Serialization.Formatters.Binary.BinaryTypeConverter
// Assembly: System.Runtime.Serialization.Formatters, Version=4.0.3.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 59FEC222-A9B8-4506-99C2-FAFFD4E25D28
// Assembly location: C:\Users\Admin\Desktop\RE\1\System.Runtime.Serialization.Formatters.dll

using System.Reflection;


namespace System.Runtime.Serialization.Formatters.Binary
{
  internal static class BinaryTypeConverter
  {
    internal static BinaryTypeEnum GetBinaryTypeInfo(
      Type type,
      WriteObjectInfo objectInfo,
      string typeName,
      ObjectWriter objectWriter,
      out object typeInformation,
      out int assemId)
    {
      assemId = 0;
      typeInformation = (object) null;
      BinaryTypeEnum binaryTypeInfo;
      if ((object) type == (object) Converter.s_typeofString)
        binaryTypeInfo = BinaryTypeEnum.String;
      else if ((objectInfo == null || objectInfo != null && !objectInfo._isSi) && (object) type == (object) Converter.s_typeofObject)
        binaryTypeInfo = BinaryTypeEnum.Object;
      else if ((object) type == (object) Converter.s_typeofStringArray)
        binaryTypeInfo = BinaryTypeEnum.StringArray;
      else if ((object) type == (object) Converter.s_typeofObjectArray)
        binaryTypeInfo = BinaryTypeEnum.ObjectArray;
      else if (Converter.IsPrimitiveArray(type, out typeInformation))
      {
        binaryTypeInfo = BinaryTypeEnum.PrimitiveArray;
      }
      else
      {
        InternalPrimitiveTypeE code = objectWriter.ToCode(type);
        if (code == InternalPrimitiveTypeE.Invalid)
        {
          string str;
          if (objectInfo == null)
          {
            str = type.GetTypeInfo().Assembly.FullName;
            typeInformation = (object) type.FullName;
          }
          else
          {
            str = objectInfo.GetAssemblyString();
            typeInformation = (object) objectInfo.GetTypeFullName();
          }
          if (str.Equals(Converter.s_urtAssemblyString) || str.Equals(Converter.s_urtAlternativeAssemblyString))
          {
            binaryTypeInfo = BinaryTypeEnum.ObjectUrt;
            assemId = 0;
          }
          else
          {
            binaryTypeInfo = BinaryTypeEnum.ObjectUser;
            assemId = (int) objectInfo._assemId;
            if (assemId == 0)
              throw new SerializationException(SR.Format(SR.Serialization_AssemblyId, typeInformation));
          }
        }
        else
        {
          binaryTypeInfo = BinaryTypeEnum.Primitive;
          typeInformation = (object) code;
        }
      }
      return binaryTypeInfo;
    }

    internal static BinaryTypeEnum GetParserBinaryTypeInfo(Type type, out object typeInformation)
    {
      typeInformation = (object) null;
      BinaryTypeEnum parserBinaryTypeInfo;
      if ((object) type == (object) Converter.s_typeofString)
        parserBinaryTypeInfo = BinaryTypeEnum.String;
      else if ((object) type == (object) Converter.s_typeofObject)
        parserBinaryTypeInfo = BinaryTypeEnum.Object;
      else if ((object) type == (object) Converter.s_typeofObjectArray)
        parserBinaryTypeInfo = BinaryTypeEnum.ObjectArray;
      else if ((object) type == (object) Converter.s_typeofStringArray)
        parserBinaryTypeInfo = BinaryTypeEnum.StringArray;
      else if (Converter.IsPrimitiveArray(type, out typeInformation))
      {
        parserBinaryTypeInfo = BinaryTypeEnum.PrimitiveArray;
      }
      else
      {
        InternalPrimitiveTypeE code = Converter.ToCode(type);
        if (code == InternalPrimitiveTypeE.Invalid)
        {
          parserBinaryTypeInfo = Assembly.Equals(type.GetTypeInfo().Assembly, Converter.s_urtAssembly) 
                        ? BinaryTypeEnum.ObjectUrt 
                        : BinaryTypeEnum.ObjectUser;
          typeInformation = (object) type.FullName;
        }
        else
        {
          parserBinaryTypeInfo = BinaryTypeEnum.Primitive;
          typeInformation = (object) code;
        }
      }
      return parserBinaryTypeInfo;
    }

    internal static void WriteTypeInfo(
      BinaryTypeEnum binaryTypeEnum,
      object typeInformation,
      int assemId,
      BinaryFormatterWriter output)
    {
      switch (binaryTypeEnum)
      {
        case BinaryTypeEnum.Primitive:
        case BinaryTypeEnum.PrimitiveArray:
          output.WriteByte((byte) (InternalPrimitiveTypeE) typeInformation);
          break;
        case BinaryTypeEnum.String:
          break;
        case BinaryTypeEnum.Object:
          break;
        case BinaryTypeEnum.ObjectUrt:
          output.WriteString(typeInformation.ToString());
          break;
        case BinaryTypeEnum.ObjectUser:
          output.WriteString(typeInformation.ToString());
          output.WriteInt32(assemId);
          break;
        case BinaryTypeEnum.ObjectArray:
          break;
        case BinaryTypeEnum.StringArray:
          break;
        default:
          throw new SerializationException(SR.Format(SR.Serialization_TypeWrite, (object) binaryTypeEnum.ToString()));
      }
    }

    internal static object ReadTypeInfo(
      BinaryTypeEnum binaryTypeEnum,
      BinaryParser input,
      out int assemId)
    {
      object obj = (object) null;
      int num = 0;
      switch (binaryTypeEnum)
      {
        case BinaryTypeEnum.Primitive:
        case BinaryTypeEnum.PrimitiveArray:
          obj = (object) (InternalPrimitiveTypeE) input.ReadByte();
          goto case BinaryTypeEnum.String;
        case BinaryTypeEnum.String:
        case BinaryTypeEnum.Object:
        case BinaryTypeEnum.ObjectArray:
        case BinaryTypeEnum.StringArray:
          assemId = num;
          return obj;
        case BinaryTypeEnum.ObjectUrt:
          obj = (object) input.ReadString();
          goto case BinaryTypeEnum.String;
        case BinaryTypeEnum.ObjectUser:
          obj = (object) input.ReadString();
          num = input.ReadInt32();
          goto case BinaryTypeEnum.String;
        default:
          throw new SerializationException(SR.Format(SR.Serialization_TypeRead, (object) binaryTypeEnum.ToString()));
      }
    }

    internal static void TypeFromInfo(
      BinaryTypeEnum binaryTypeEnum,
      object typeInformation,
      ObjectReader objectReader,
      BinaryAssemblyInfo assemblyInfo,
      out InternalPrimitiveTypeE primitiveTypeEnum,
      out string typeString,
      out Type type,
      out bool isVariant)
    {
      isVariant = false;
      primitiveTypeEnum = InternalPrimitiveTypeE.Invalid;
      typeString = (string) null;
      type = (Type) null;
      switch (binaryTypeEnum)
      {
        case BinaryTypeEnum.Primitive:
          primitiveTypeEnum = (InternalPrimitiveTypeE) typeInformation;
          typeString = Converter.ToComType(primitiveTypeEnum);
          type = Converter.ToType(primitiveTypeEnum);
          break;
        case BinaryTypeEnum.String:
          type = Converter.s_typeofString;
          break;
        case BinaryTypeEnum.Object:
          type = Converter.s_typeofObject;
          isVariant = true;
          break;
        case BinaryTypeEnum.ObjectUrt:
        case BinaryTypeEnum.ObjectUser:
          if (typeInformation == null)
            break;
          typeString = typeInformation.ToString();
          type = objectReader.GetType(assemblyInfo, typeString);
          if ((object) type != (object) Converter.s_typeofObject)
            break;
          isVariant = true;
          break;
        case BinaryTypeEnum.ObjectArray:
          type = Converter.s_typeofObjectArray;
          break;
        case BinaryTypeEnum.StringArray:
          type = Converter.s_typeofStringArray;
          break;
        case BinaryTypeEnum.PrimitiveArray:
          primitiveTypeEnum = (InternalPrimitiveTypeE) typeInformation;
          type = Converter.ToArrayType(primitiveTypeEnum);
          break;
        default:
          throw new SerializationException(SR.Format(SR.Serialization_TypeRead, (object) binaryTypeEnum.ToString()));
      }
    }
  }
}
