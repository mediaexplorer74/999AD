// Decompiled with JetBrains decompiler
// Type: System.Runtime.Serialization.Formatters.Binary.BinaryCrossAppDomainString
// Assembly: System.Runtime.Serialization.Formatters, Version=4.0.3.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 59FEC222-A9B8-4506-99C2-FAFFD4E25D28
// Assembly location: C:\Users\Admin\Desktop\RE\1\System.Runtime.Serialization.Formatters.dll


namespace System.Runtime.Serialization.Formatters.Binary
{
  internal sealed class BinaryCrossAppDomainString : IStreamable
  {
    internal int _objectId;
    internal int _value;

    internal BinaryCrossAppDomainString()
    {
    }

    public void Write(BinaryFormatterWriter output)
    {
      output.WriteByte((byte) 19);
      output.WriteInt32(this._objectId);
      output.WriteInt32(this._value);
    }

    public void Read(BinaryParser input)
    {
      this._objectId = input.ReadInt32();
      this._value = input.ReadInt32();
    }
  }
}
