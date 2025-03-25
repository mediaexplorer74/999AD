// Decompiled with JetBrains decompiler
// Type: System.Runtime.Serialization.Formatters.Binary.Converter
// Assembly: System.Runtime.Serialization.Formatters, Version=4.0.3.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 59FEC222-A9B8-4506-99C2-FAFFD4E25D28
// Assembly location: C:\Users\Admin\Desktop\RE\1\System.Runtime.Serialization.Formatters.dll

using System.Globalization;
using System.Reflection;


namespace System.Runtime.Serialization.Formatters.Binary
{
  internal static class Converter
  {
    internal static readonly Type s_typeofISerializable = typeof (ISerializable);
    internal static readonly Type s_typeofString = typeof (string);
    internal static readonly Type s_typeofConverter = typeof (Converter);
    internal static readonly Type s_typeofBoolean = typeof (bool);
    internal static readonly Type s_typeofByte = typeof (byte);
    internal static readonly Type s_typeofChar = typeof (char);
    internal static readonly Type s_typeofDecimal = typeof (Decimal);
    internal static readonly Type s_typeofDouble = typeof (double);
    internal static readonly Type s_typeofInt16 = typeof (short);
    internal static readonly Type s_typeofInt32 = typeof (int);
    internal static readonly Type s_typeofInt64 = typeof (long);
    internal static readonly Type s_typeofSByte = typeof (sbyte);
    internal static readonly Type s_typeofSingle = typeof (float);
    internal static readonly Type s_typeofTimeSpan = typeof (TimeSpan);
    internal static readonly Type s_typeofDateTime = typeof (DateTime);
    internal static readonly Type s_typeofUInt16 = typeof (ushort);
    internal static readonly Type s_typeofUInt32 = typeof (uint);
    internal static readonly Type s_typeofUInt64 = typeof (ulong);
    internal static readonly Type s_typeofObject = typeof (object);
    internal static readonly Type s_typeofSystemVoid = typeof (void);
        internal static readonly Assembly
                s_urtAssembly = Assembly.Load(new AssemblyName("mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"));
    internal static readonly string s_urtAssemblyString = Converter.s_urtAssembly.FullName;
        internal static readonly Assembly s_urtAlternativeAssembly = default;//Converter.s_typeofString.Assembly;
    internal static readonly string s_urtAlternativeAssemblyString = Converter.s_urtAlternativeAssembly.FullName;
    internal static readonly Type s_typeofTypeArray = typeof (Type[]);
    internal static readonly Type s_typeofObjectArray = typeof (object[]);
    internal static readonly Type s_typeofStringArray = typeof (string[]);
    internal static readonly Type s_typeofBooleanArray = typeof (bool[]);
    internal static readonly Type s_typeofByteArray = typeof (byte[]);
    internal static readonly Type s_typeofCharArray = typeof (char[]);
    internal static readonly Type s_typeofDecimalArray = typeof (Decimal[]);
    internal static readonly Type s_typeofDoubleArray = typeof (double[]);
    internal static readonly Type s_typeofInt16Array = typeof (short[]);
    internal static readonly Type s_typeofInt32Array = typeof (int[]);
    internal static readonly Type s_typeofInt64Array = typeof (long[]);
    internal static readonly Type s_typeofSByteArray = typeof (sbyte[]);
    internal static readonly Type s_typeofSingleArray = typeof (float[]);
    internal static readonly Type s_typeofTimeSpanArray = typeof (TimeSpan[]);
    internal static readonly Type s_typeofDateTimeArray = typeof (DateTime[]);
    internal static readonly Type s_typeofUInt16Array = typeof (ushort[]);
    internal static readonly Type s_typeofUInt32Array = typeof (uint[]);
    internal static readonly Type s_typeofUInt64Array = typeof (ulong[]);
    internal static readonly Type s_typeofMarshalByRefObject = typeof (MarshalByRefObject);
    private const int PrimitiveTypeEnumLength = 17;
    private static volatile Type[] s_typeA;
    private static volatile Type[] s_arrayTypeA;
    private static volatile string[] s_valueA;
    private static volatile TypeCode[] s_typeCodeA;
    private static volatile InternalPrimitiveTypeE[] s_codeA;

    internal static InternalPrimitiveTypeE ToCode(Type type)
    {
      if (type == (Type) null)
        return Converter.ToPrimitiveTypeEnum(TypeCode.Empty);
      //RnD
      //if (type.GetTypeInfo().IsPrimitive)
      //  return Converter.ToPrimitiveTypeEnum(Type.GetTypeCode(type));
      if ((object) type == (object) Converter.s_typeofDateTime)
        return InternalPrimitiveTypeE.DateTime;
      if ((object) type == (object) Converter.s_typeofTimeSpan)
        return InternalPrimitiveTypeE.TimeSpan;
      return (object) type != (object) Converter.s_typeofDecimal ? InternalPrimitiveTypeE.Invalid : InternalPrimitiveTypeE.Decimal;
    }

    internal static bool IsWriteAsByteArray(InternalPrimitiveTypeE code)
    {
      switch (code)
      {
        case InternalPrimitiveTypeE.Boolean:
        case InternalPrimitiveTypeE.Byte:
        case InternalPrimitiveTypeE.Char:
        case InternalPrimitiveTypeE.Double:
        case InternalPrimitiveTypeE.Int16:
        case InternalPrimitiveTypeE.Int32:
        case InternalPrimitiveTypeE.Int64:
        case InternalPrimitiveTypeE.SByte:
        case InternalPrimitiveTypeE.Single:
        case InternalPrimitiveTypeE.UInt16:
        case InternalPrimitiveTypeE.UInt32:
        case InternalPrimitiveTypeE.UInt64:
          return true;
        default:
          return false;
      }
    }

    internal static int TypeLength(InternalPrimitiveTypeE code)
    {
      switch (code)
      {
        case InternalPrimitiveTypeE.Boolean:
          return 1;
        case InternalPrimitiveTypeE.Byte:
          return 1;
        case InternalPrimitiveTypeE.Char:
          return 2;
        case InternalPrimitiveTypeE.Double:
          return 8;
        case InternalPrimitiveTypeE.Int16:
          return 2;
        case InternalPrimitiveTypeE.Int32:
          return 4;
        case InternalPrimitiveTypeE.Int64:
          return 8;
        case InternalPrimitiveTypeE.SByte:
          return 1;
        case InternalPrimitiveTypeE.Single:
          return 4;
        case InternalPrimitiveTypeE.UInt16:
          return 2;
        case InternalPrimitiveTypeE.UInt32:
          return 4;
        case InternalPrimitiveTypeE.UInt64:
          return 8;
        default:
          return 0;
      }
    }

    internal static InternalNameSpaceE GetNameSpaceEnum(
      InternalPrimitiveTypeE code,
      Type type,
      WriteObjectInfo objectInfo,
      out string typeName)
    {
      InternalNameSpaceE nameSpaceEnum = InternalNameSpaceE.None;
      typeName = (string) null;
      switch (code)
      {
        case InternalPrimitiveTypeE.Boolean:
        case InternalPrimitiveTypeE.Byte:
        case InternalPrimitiveTypeE.Char:
        case InternalPrimitiveTypeE.Double:
        case InternalPrimitiveTypeE.Int16:
        case InternalPrimitiveTypeE.Int32:
        case InternalPrimitiveTypeE.Int64:
        case InternalPrimitiveTypeE.SByte:
        case InternalPrimitiveTypeE.Single:
        case InternalPrimitiveTypeE.TimeSpan:
        case InternalPrimitiveTypeE.DateTime:
        case InternalPrimitiveTypeE.UInt16:
        case InternalPrimitiveTypeE.UInt32:
        case InternalPrimitiveTypeE.UInt64:
          nameSpaceEnum = InternalNameSpaceE.XdrPrimitive;
          typeName = "System." + Converter.ToComType(code);
          break;
        case InternalPrimitiveTypeE.Decimal:
          nameSpaceEnum = InternalNameSpaceE.UrtSystem;
          typeName = "System." + Converter.ToComType(code);
          break;
      }
      if (nameSpaceEnum == InternalNameSpaceE.None && type != (Type) null)
      {
        if ((object) type == (object) Converter.s_typeofString)
          nameSpaceEnum = InternalNameSpaceE.XdrString;
        else if (objectInfo == null)
        {
          typeName = type.FullName;
          nameSpaceEnum = Assembly.Equals(type.GetTypeInfo().Assembly, Converter.s_urtAssembly) ? InternalNameSpaceE.UrtSystem : InternalNameSpaceE.UrtUser;
        }
        else
        {
          typeName = objectInfo.GetTypeFullName();
          nameSpaceEnum = objectInfo.GetAssemblyString().Equals(Converter.s_urtAssemblyString) ? InternalNameSpaceE.UrtSystem : InternalNameSpaceE.UrtUser;
        }
      }
      return nameSpaceEnum;
    }

    internal static Type ToArrayType(InternalPrimitiveTypeE code)
    {
      if (Converter.s_arrayTypeA == null)
        Converter.InitArrayTypeA();
      return Converter.s_arrayTypeA[(int) code];
    }

    private static void InitTypeA()
    {
      Converter.s_typeA = new Type[17]
      {
        (Type) null,
        Converter.s_typeofBoolean,
        Converter.s_typeofByte,
        Converter.s_typeofChar,
        null,
        Converter.s_typeofDecimal,
        Converter.s_typeofDouble,
        Converter.s_typeofInt16,
        Converter.s_typeofInt32,
        Converter.s_typeofInt64,
        Converter.s_typeofSByte,
        Converter.s_typeofSingle,
        Converter.s_typeofTimeSpan,
        Converter.s_typeofDateTime,
        Converter.s_typeofUInt16,
        Converter.s_typeofUInt32,
        Converter.s_typeofUInt64
      };
    }

    private static void InitArrayTypeA()
    {
      Converter.s_arrayTypeA = new Type[17]
      {
        (Type) null,
        Converter.s_typeofBooleanArray,
        Converter.s_typeofByteArray,
        Converter.s_typeofCharArray,
        null,
        Converter.s_typeofDecimalArray,
        Converter.s_typeofDoubleArray,
        Converter.s_typeofInt16Array,
        Converter.s_typeofInt32Array,
        Converter.s_typeofInt64Array,
        Converter.s_typeofSByteArray,
        Converter.s_typeofSingleArray,
        Converter.s_typeofTimeSpanArray,
        Converter.s_typeofDateTimeArray,
        Converter.s_typeofUInt16Array,
        Converter.s_typeofUInt32Array,
        Converter.s_typeofUInt64Array
      };
    }

    internal static Type ToType(InternalPrimitiveTypeE code)
    {
      if (Converter.s_typeA == null)
        Converter.InitTypeA();
      return Converter.s_typeA[(int) code];
    }

    internal static Array CreatePrimitiveArray(InternalPrimitiveTypeE code, int length)
    {
      switch (code)
      {
        case InternalPrimitiveTypeE.Boolean:
          return (Array) new bool[length];
        case InternalPrimitiveTypeE.Byte:
          return (Array) new byte[length];
        case InternalPrimitiveTypeE.Char:
          return (Array) new char[length];
        case InternalPrimitiveTypeE.Decimal:
          return (Array) new Decimal[length];
        case InternalPrimitiveTypeE.Double:
          return (Array) new double[length];
        case InternalPrimitiveTypeE.Int16:
          return (Array) new short[length];
        case InternalPrimitiveTypeE.Int32:
          return (Array) new int[length];
        case InternalPrimitiveTypeE.Int64:
          return (Array) new long[length];
        case InternalPrimitiveTypeE.SByte:
          return (Array) new sbyte[length];
        case InternalPrimitiveTypeE.Single:
          return (Array) new float[length];
        case InternalPrimitiveTypeE.TimeSpan:
          return (Array) new TimeSpan[length];
        case InternalPrimitiveTypeE.DateTime:
          return (Array) new DateTime[length];
        case InternalPrimitiveTypeE.UInt16:
          return (Array) new ushort[length];
        case InternalPrimitiveTypeE.UInt32:
          return (Array) new uint[length];
        case InternalPrimitiveTypeE.UInt64:
          return (Array) new ulong[length];
        default:
          return (Array) null;
      }
    }

    internal static bool IsPrimitiveArray(Type type, out object typeInformation)
    {
      bool flag = true;
      if ((object) type == (object) Converter.s_typeofBooleanArray)
        typeInformation = (object) InternalPrimitiveTypeE.Boolean;
      else if ((object) type == (object) Converter.s_typeofByteArray)
        typeInformation = (object) InternalPrimitiveTypeE.Byte;
      else if ((object) type == (object) Converter.s_typeofCharArray)
        typeInformation = (object) InternalPrimitiveTypeE.Char;
      else if ((object) type == (object) Converter.s_typeofDoubleArray)
        typeInformation = (object) InternalPrimitiveTypeE.Double;
      else if ((object) type == (object) Converter.s_typeofInt16Array)
        typeInformation = (object) InternalPrimitiveTypeE.Int16;
      else if ((object) type == (object) Converter.s_typeofInt32Array)
        typeInformation = (object) InternalPrimitiveTypeE.Int32;
      else if ((object) type == (object) Converter.s_typeofInt64Array)
        typeInformation = (object) InternalPrimitiveTypeE.Int64;
      else if ((object) type == (object) Converter.s_typeofSByteArray)
        typeInformation = (object) InternalPrimitiveTypeE.SByte;
      else if ((object) type == (object) Converter.s_typeofSingleArray)
        typeInformation = (object) InternalPrimitiveTypeE.Single;
      else if ((object) type == (object) Converter.s_typeofUInt16Array)
        typeInformation = (object) InternalPrimitiveTypeE.UInt16;
      else if ((object) type == (object) Converter.s_typeofUInt32Array)
        typeInformation = (object) InternalPrimitiveTypeE.UInt32;
      else if ((object) type == (object) Converter.s_typeofUInt64Array)
      {
        typeInformation = (object) InternalPrimitiveTypeE.UInt64;
      }
      else
      {
        typeInformation = (object) null;
        flag = false;
      }
      return flag;
    }

    private static void InitValueA()
    {
      Converter.s_valueA = new string[17]
      {
        (string) null,
        "Boolean",
        "Byte",
        "Char",
        null,
        "Decimal",
        "Double",
        "Int16",
        "Int32",
        "Int64",
        "SByte",
        "Single",
        "TimeSpan",
        "DateTime",
        "UInt16",
        "UInt32",
        "UInt64"
      };
    }

    internal static string ToComType(InternalPrimitiveTypeE code)
    {
      if (Converter.s_valueA == null)
        Converter.InitValueA();
      return Converter.s_valueA[(int) code];
    }

    private static void InitTypeCodeA()
    {
      Converter.s_typeCodeA = new TypeCode[17]
      {
        TypeCode.Object,
        TypeCode.Boolean,
        TypeCode.Byte,
        TypeCode.Char,
        TypeCode.Empty,
        TypeCode.Decimal,
        TypeCode.Double,
        TypeCode.Int16,
        TypeCode.Int32,
        TypeCode.Int64,
        TypeCode.SByte,
        TypeCode.Single,
        TypeCode.Object,
        TypeCode.DateTime,
        TypeCode.UInt16,
        TypeCode.UInt32,
        TypeCode.UInt64
      };
    }

    internal static TypeCode ToTypeCode(InternalPrimitiveTypeE code)
    {
      if (Converter.s_typeCodeA == null)
        Converter.InitTypeCodeA();
      return Converter.s_typeCodeA[(int) code];
    }

    private static void InitCodeA()
    {
      Converter.s_codeA = new InternalPrimitiveTypeE[19]
      {
        InternalPrimitiveTypeE.Invalid,
        InternalPrimitiveTypeE.Invalid,
        InternalPrimitiveTypeE.Invalid,
        InternalPrimitiveTypeE.Boolean,
        InternalPrimitiveTypeE.Char,
        InternalPrimitiveTypeE.SByte,
        InternalPrimitiveTypeE.Byte,
        InternalPrimitiveTypeE.Int16,
        InternalPrimitiveTypeE.UInt16,
        InternalPrimitiveTypeE.Int32,
        InternalPrimitiveTypeE.UInt32,
        InternalPrimitiveTypeE.Int64,
        InternalPrimitiveTypeE.UInt64,
        InternalPrimitiveTypeE.Single,
        InternalPrimitiveTypeE.Double,
        InternalPrimitiveTypeE.Decimal,
        InternalPrimitiveTypeE.DateTime,
        InternalPrimitiveTypeE.Invalid,
        InternalPrimitiveTypeE.Invalid
      };
    }

    internal static InternalPrimitiveTypeE ToPrimitiveTypeEnum(TypeCode typeCode)
    {
      if (Converter.s_codeA == null)
        Converter.InitCodeA();
      return Converter.s_codeA[(int) typeCode];
    }

    internal static object FromString(string value, InternalPrimitiveTypeE code)
    {
      return code == InternalPrimitiveTypeE.Invalid ? (object) value : Convert.ChangeType((object) value, Converter.ToTypeCode(code), (IFormatProvider) CultureInfo.InvariantCulture);
    }
  }
}
