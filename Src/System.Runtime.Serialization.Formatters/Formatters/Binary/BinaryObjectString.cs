// Decompiled with JetBrains decompiler
// Type: System.Runtime.Serialization.Formatters.Binary.BinaryObjectString
// Assembly: System.Runtime.Serialization.Formatters, Version=4.0.3.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 59FEC222-A9B8-4506-99C2-FAFFD4E25D28
// Assembly location: C:\Users\Admin\Desktop\RE\1\System.Runtime.Serialization.Formatters.dll


namespace System.Runtime.Serialization.Formatters.Binary
{
  internal sealed class BinaryObjectString : IStreamable
  {
    internal int _objectId;
    internal string _value;

    internal BinaryObjectString()
    {
    }

    internal void Set(int objectId, string value)
    {
      this._objectId = objectId;
      this._value = value;
    }

    public void Write(BinaryFormatterWriter output)
    {
      output.WriteByte((byte) 6);
      output.WriteInt32(this._objectId);
      output.WriteString(this._value);
    }

    public void Read(BinaryParser input)
    {
      this._objectId = input.ReadInt32();
      this._value = input.ReadString();
    }
  }
}
