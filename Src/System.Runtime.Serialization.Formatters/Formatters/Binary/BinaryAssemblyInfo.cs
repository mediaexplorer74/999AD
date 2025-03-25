// Decompiled with JetBrains decompiler
// Type: System.Runtime.Serialization.Formatters.Binary.BinaryAssemblyInfo
// Assembly: System.Runtime.Serialization.Formatters, Version=4.0.3.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 59FEC222-A9B8-4506-99C2-FAFFD4E25D28
// Assembly location: C:\Users\Admin\Desktop\RE\1\System.Runtime.Serialization.Formatters.dll

using System.Reflection;


namespace System.Runtime.Serialization.Formatters.Binary
{
  internal sealed class BinaryAssemblyInfo
  {
    internal string _assemblyString;
    private Assembly _assembly;

    internal BinaryAssemblyInfo(string assemblyString) => this._assemblyString = assemblyString;

    internal BinaryAssemblyInfo(string assemblyString, Assembly assembly)
      : this(assemblyString)
    {
      this._assembly = assembly;
    }

    internal Assembly GetAssembly()
    {
      if (Assembly.Equals(this._assembly, (Assembly) null))
      {
        this._assembly = FormatterServices.LoadAssemblyFromStringNoThrow(this._assemblyString);
        if (Assembly.Equals(this._assembly, (Assembly) null))
          throw new SerializationException(SR.Format(SR.Serialization_AssemblyNotFound, (object) this._assemblyString));
      }
      return this._assembly;
    }
  }
}
