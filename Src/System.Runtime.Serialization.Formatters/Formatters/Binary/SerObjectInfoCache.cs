// Decompiled with JetBrains decompiler
// Type: System.Runtime.Serialization.Formatters.Binary.SerObjectInfoCache
// Assembly: System.Runtime.Serialization.Formatters, Version=4.0.3.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 59FEC222-A9B8-4506-99C2-FAFFD4E25D28
// Assembly location: C:\Users\Admin\Desktop\RE\1\System.Runtime.Serialization.Formatters.dll

using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary1;


namespace System.Runtime.Serialization.Formatters.Binary
{
  internal sealed class SerObjectInfoCache
  {
    internal readonly string _fullTypeName;
    internal readonly string _assemblyString;
    internal readonly bool _hasTypeForwardedFrom;
    internal MemberInfo[] _memberInfos;
    internal string[] _memberNames;
    internal Type[] _memberTypes;

    internal SerObjectInfoCache(string typeName, string assemblyName, bool hasTypeForwardedFrom)
    {
      this._fullTypeName = typeName;
      this._assemblyString = assemblyName;
      this._hasTypeForwardedFrom = hasTypeForwardedFrom;
    }

    internal SerObjectInfoCache(Type type)
    {
      TypeInformation typeInformation = BinaryFormatter.GetTypeInformation(type);
      this._fullTypeName = typeInformation.FullTypeName;
      this._assemblyString = typeInformation.AssemblyString;
      this._hasTypeForwardedFrom = typeInformation.HasTypeForwardedFrom;
    }
  }
}
