// Decompiled with JetBrains decompiler
// Type: System.Runtime.Serialization.SerializationBinder
// Assembly: System.Runtime.Serialization.Formatters, Version=4.0.3.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 59FEC222-A9B8-4506-99C2-FAFFD4E25D28
// Assembly location: C:\Users\Admin\Desktop\RE\1\System.Runtime.Serialization.Formatters.dll


namespace System.Runtime.Serialization
{
  public abstract class SerializationBinder
  {
    public virtual void BindToName(
      Type serializedType,
      out string assemblyName,
      out string typeName)
    {
      assemblyName = (string) null;
      typeName = (string) null;
    }

    public abstract Type BindToType(string assemblyName, string typeName);
  }
}
