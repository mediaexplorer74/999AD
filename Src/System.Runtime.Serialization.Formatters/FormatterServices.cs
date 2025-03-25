// Decompiled with JetBrains decompiler
// Type: System.Runtime.Serialization.FormatterServices
// Assembly: System.Runtime.Serialization.Formatters, Version=4.0.3.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 59FEC222-A9B8-4506-99C2-FAFFD4E25D28
// Assembly location: C:\Users\Admin\Desktop\RE\1\System.Runtime.Serialization.Formatters.dll

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters;
using System.Text;


namespace System.Runtime.Serialization
{
  public static class FormatterServices
  {
    private static readonly ConcurrentDictionary<MemberHolder, MemberInfo[]> s_memberInfoTable 
            = new ConcurrentDictionary<MemberHolder, MemberInfo[]>();

        /*
    private static FieldInfo[] InternalGetSerializableMembers(Type type)
    {
      if (type.GetTypeInfo().IsInterface)
        return Array.Empty<FieldInfo>();
      FieldInfo[] sourceArray = type.GetTypeInfo().IsSerializable 
                ? FormatterServices.GetSerializableFields(type) 
                : throw new SerializationException(SR.Format(SR.Serialization_NonSerType, 
                (object) type.FullName, (object) type.GetTypeInfo().Assembly.FullName));
      Type baseType = type.GetTypeInfo().BaseType;
      if (baseType != (Type) null && baseType != typeof (object))
      {
        Type[] parentTypes1;
        int parentTypeCount;
        bool parentTypes2 = FormatterServices.GetParentTypes(baseType, out parentTypes1, out parentTypeCount);
        if (parentTypeCount > 0)
        {
          List<FieldInfo> fieldInfoList = new List<FieldInfo>();
          for (int index = 0; index < parentTypeCount; ++index)
          {
            Type type1 = parentTypes1[index];
            FieldInfo[] fieldInfoArray = type1.GetTypeInfo().IsSerializable ? type1.GetFields((BindingFlags) 36) : throw new SerializationException
                            (SR.Format(SR.Serialization_NonSerType, (object) type1.FullName, (object) type1.GetTypeInfo().Module.Assembly.FullName));
            string namePrefix = parentTypes2 ? type1.Name : type1.FullName;
            foreach (FieldInfo field in fieldInfoArray)
            {
              if (!field.IsNotSerialized)
                fieldInfoList.Add((FieldInfo) new SerializationFieldInfo(field, namePrefix));
            }
          }
          if (fieldInfoList != null && fieldInfoList.Count > 0)
          {
            FieldInfo[] fieldInfoArray = new FieldInfo[fieldInfoList.Count + sourceArray.Length];
            Array.Copy((Array) sourceArray, (Array) fieldInfoArray, sourceArray.Length);
            fieldInfoList.CopyTo(fieldInfoArray, sourceArray.Length);
            sourceArray = fieldInfoArray;
          }
        }
      }
      return sourceArray;
    }
        */
        private static FieldInfo[] InternalGetSerializableMembers(Type type)
        {
            if (type.GetTypeInfo().IsInterface)
                return Array.Empty<FieldInfo>();
            FieldInfo[] sourceArray = type.GetTypeInfo().IsSerializable
                ? FormatterServices.GetSerializableFields(type)
                : throw new SerializationException(SR.Format(SR.Serialization_NonSerType,
                (object)type.FullName, (object)type.GetTypeInfo().Assembly.FullName));
            Type baseType = type.GetTypeInfo().BaseType;
            if (baseType != (Type)null && baseType != typeof(object))
            {
                Type[] parentTypes1;
                int parentTypeCount;
                bool parentTypes2 = FormatterServices.GetParentTypes(baseType, out parentTypes1, out parentTypeCount);
                if (parentTypeCount > 0)
                {
                    List<FieldInfo> fieldInfoList = new List<FieldInfo>();
                    for (int index = 0; index < parentTypeCount; ++index)
                    {
                        Type type1 = parentTypes1[index];
                        FieldInfo[] fieldInfoArray = type1.GetTypeInfo().IsSerializable ? type1.GetFields((BindingFlags)36) : throw new SerializationException
                                        (SR.Format(SR.Serialization_NonSerType, (object)type1.FullName, (object)type1.GetTypeInfo().Module.Assembly.FullName));
                        string namePrefix = parentTypes2 ? type1.Name : type1.FullName;
                        foreach (FieldInfo field in fieldInfoArray)
                        {
                            if (!field.IsDefined(typeof(NonSerializedAttribute), false))
                                fieldInfoList.Add((FieldInfo)new SerializationFieldInfo(field, namePrefix));
                        }
                    }
                    if (fieldInfoList != null && fieldInfoList.Count > 0)
                    {
                        FieldInfo[] fieldInfoArray = new FieldInfo[fieldInfoList.Count + sourceArray.Length];
                        Array.Copy((Array)sourceArray, (Array)fieldInfoArray, sourceArray.Length);
                        fieldInfoList.CopyTo(fieldInfoArray, sourceArray.Length);
                        sourceArray = fieldInfoArray;
                    }
                }
            }
            return sourceArray;
        }

        private static FieldInfo[] GetSerializableFields(Type type)
    {
      FieldInfo[] fields = type.GetFields((BindingFlags) 52);
      int length = 0;
      for (int index = 0; index < fields.Length; ++index)
      {
        if ((fields[index].Attributes & (FieldAttributes)128) != (FieldAttributes)128)
          ++length;
      }
      if (length == fields.Length)
        return fields;
      FieldInfo[] serializableFields = new FieldInfo[length];
      int index1 = 0;
      for (int index2 = 0; index2 < fields.Length; ++index2)
      {
        if ((fields[index2].Attributes & (FieldAttributes)128) != (FieldAttributes)128)
        {
          serializableFields[index1] = fields[index2];
          ++index1;
        }
      }
      return serializableFields;
    }

    private static bool GetParentTypes(
      Type parentType,
      out Type[] parentTypes,
      out int parentTypeCount)
    {
      parentTypes = (Type[]) null;
      parentTypeCount = 0;
      bool parentTypes1 = true;
      Type type1 = typeof (object);
      for (Type type2 = parentType; type2 != type1; type2 = type2.GetTypeInfo().BaseType)
      {
        if (!type2.GetTypeInfo().IsInterface)
        {
          string name1 = type2.Name;
          for (int index = 0; parentTypes1 && index < parentTypeCount; ++index)
          {
            string name2 = parentTypes[index].Name;
            if (name2.Length == name1.Length && (int) name2[0] == (int) name1[0] && name1 == name2)
            {
              parentTypes1 = false;
              break;
            }
          }
          if (parentTypes == null || parentTypeCount == parentTypes.Length)
            Array.Resize<Type>(ref parentTypes, Math.Max(parentTypeCount * 2, 12));
          parentTypes[parentTypeCount++] = type2;
        }
      }
      return parentTypes1;
    }


    public static MemberInfo[] GetSerializableMembers(Type type)
    {
      return FormatterServices.GetSerializableMembers(type, 
          new StreamingContext(
              //(StreamingContextStates) (int) byte.MaxValue
              ));
    }

    public static MemberInfo[] GetSerializableMembers(Type type, StreamingContext context)
    {
      if (type == (Type) null)
        throw new ArgumentNullException(nameof (type));
      return FormatterServices.s_memberInfoTable.GetOrAdd(new MemberHolder(type, context), (Func<MemberHolder, MemberInfo[]>) (mh => (MemberInfo[]) FormatterServices.InternalGetSerializableMembers(mh._memberType)));
    }

    public static void CheckTypeSecurity(Type t, TypeFilterLevel securityLevel)
    {
    }

    public static object GetUninitializedObject(Type type)
    {
            return default;//RuntimeHelpers.GetUninitializedObject(type);
    }

    public static object GetSafeUninitializedObject(Type type)
    {
            return default;// RuntimeHelpers.GetUninitializedObject(type);
    }

    internal static void SerializationSetValue(MemberInfo fi, object target, object value)
    {
      FieldInfo fieldInfo = fi as FieldInfo;
      if (fieldInfo !=(FieldInfo) null)
        throw new ArgumentException(SR.Argument_InvalidFieldInfo);
      fieldInfo.SetValue(target, value);
    }

    public static object PopulateObjectMembers(object obj, MemberInfo[] members, object[] data)
    {
      if (obj == null)
        throw new ArgumentNullException(nameof (obj));
      if (members == null)
        throw new ArgumentNullException(nameof (members));
      if (data == null)
        throw new ArgumentNullException(nameof (data));
      if (members.Length != data.Length)
        throw new ArgumentException(SR.Argument_DataLengthDifferent);
      for (int p1 = 0; p1 < members.Length; ++p1)
      {
        MemberInfo member = members[p1];
        if (MemberInfo.Equals(member, (MemberInfo) null))
          throw new ArgumentNullException(nameof (members), SR.Format(SR.ArgumentNull_NullMember, (object) p1));
        if (data[p1] != null)
        {
          FieldInfo fieldInfo = member as FieldInfo;
          if (!(fieldInfo != (FieldInfo)null))
            throw new SerializationException(SR.Serialization_UnknownMemberInfo);
          fieldInfo.SetValue(obj, data[p1]);
        }
      }
      return obj;
    }

    public static object[] GetObjectData(object obj, MemberInfo[] members)
    {
      if (obj == null)
        throw new ArgumentNullException(nameof (obj));
      object[] objectData = members != null ? new object[members.Length] 
                : throw new ArgumentNullException(nameof (members));
      for (int p1 = 0; p1 < members.Length; ++p1)
      {
        MemberInfo member = members[p1];
        FieldInfo fieldInfo = !MemberInfo.Equals(member, (MemberInfo) null) 
                    ? member as FieldInfo : throw new ArgumentNullException(nameof (members), SR.Format(SR.ArgumentNull_NullMember, (object) p1));
        objectData[p1] = !FieldInfo.Equals(fieldInfo, (FieldInfo) null)
                    ? fieldInfo.GetValue(obj) : throw new SerializationException(SR.Serialization_UnknownMemberInfo);
      }
      return objectData;
    }

    public static ISerializationSurrogate GetSurrogateForCyclicalReference(
      ISerializationSurrogate innerSurrogate)
    {
      return innerSurrogate != null 
                ? (ISerializationSurrogate) new SurrogateForCyclicalReference(innerSurrogate) 
                : throw new ArgumentNullException(nameof (innerSurrogate));
    }

    public static Type GetTypeFromAssembly(Assembly assem, string name)
    {
      return !Assembly.Equals(assem, (Assembly) null)
                ? assem.GetType(name, false, false)
                : throw new ArgumentNullException(nameof (assem));
    }

    internal static Assembly LoadAssemblyFromString(string assemblyName)
    {
      return Assembly.Load(new AssemblyName(assemblyName));
    }

    internal static Assembly LoadAssemblyFromStringNoThrow(string assemblyName)
    {
      try
      {
        return FormatterServices.LoadAssemblyFromString(assemblyName);
      }
      catch (Exception ex)
      {
      }
      return (Assembly) null;
    }

    internal static string GetClrAssemblyName(Type type, out bool hasTypeForwardedFrom)
    {
      Type type1 = !(type == (Type) null) ? type : throw new ArgumentNullException(nameof (type));
      while (type1.HasElementType)
        type1 = type1.GetElementType();
      object[] customAttributes = 
                type1.GetTypeInfo().GetCustomAttributes(typeof(TypeForwardedFromAttribute), false).ToArray();
      int index = 0;
      if (index < customAttributes.Length)
      {
        Attribute attribute = (Attribute) customAttributes[index];
        hasTypeForwardedFrom = true;
        return ((TypeForwardedFromAttribute) attribute).AssemblyFullName;
      }
      hasTypeForwardedFrom = false;
      return type.GetTypeInfo().Assembly.FullName;
    }

    internal static string GetClrTypeFullName(Type type)
    {
      return !type.IsArray 
                ? FormatterServices.GetClrTypeFullNameForNonArrayTypes(type) 
                : FormatterServices.GetClrTypeFullNameForArray(type);
    }

    private static string GetClrTypeFullNameForArray(Type type)
    {
      int arrayRank = type.GetArrayRank();
      string clrTypeFullName = FormatterServices.GetClrTypeFullName(type.GetElementType());
      return arrayRank != 1 ? clrTypeFullName + "[" + new string(',', arrayRank - 1) + "]" : clrTypeFullName + "[]";
    }

    private static string GetClrTypeFullNameForNonArrayTypes(Type type)
    {
      if (!type.GetTypeInfo().IsGenericType)
        return type.FullName;
      StringBuilder stringBuilder = new StringBuilder(type.GetGenericTypeDefinition().FullName).Append("[");
      foreach (Type genericArgument in type.GetGenericArguments())
      {
        stringBuilder.Append("[").Append(FormatterServices.GetClrTypeFullName(genericArgument)).Append(", ");
        stringBuilder.Append(FormatterServices.GetClrAssemblyName(genericArgument, out bool _)).Append("],");
      }
      return stringBuilder.Remove(stringBuilder.Length - 1, 1).Append("]").ToString();
    }
  }
}
