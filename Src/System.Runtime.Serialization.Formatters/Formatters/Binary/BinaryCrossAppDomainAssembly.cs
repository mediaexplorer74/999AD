// Decompiled with JetBrains decompiler
// Type: System.Runtime.Serialization.Formatters.Binary.BinaryCrossAppDomainAssembly
// Assembly: System.Runtime.Serialization.Formatters, Version=4.0.3.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 59FEC222-A9B8-4506-99C2-FAFFD4E25D28
// Assembly location: C:\Users\Admin\Desktop\RE\1\System.Runtime.Serialization.Formatters.dll


namespace System.Runtime.Serialization.Formatters.Binary
{
  internal sealed class BinaryCrossAppDomainAssembly : IStreamable
  {
    internal int _assemId;
    internal int _assemblyIndex;

    internal BinaryCrossAppDomainAssembly()
    {
    }

    public void Write(BinaryFormatterWriter output)
    {
      output.WriteByte((byte) 20);
      output.WriteInt32(this._assemId);
      output.WriteInt32(this._assemblyIndex);
    }

    public void Read(BinaryParser input)
    {
      this._assemId = input.ReadInt32();
      this._assemblyIndex = input.ReadInt32();
    }
  }
}
