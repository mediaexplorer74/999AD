// Decompiled with JetBrains decompiler
// Type: System.Runtime.Serialization.Formatters.Binary.MemberReference
// Assembly: System.Runtime.Serialization.Formatters, Version=4.0.3.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 59FEC222-A9B8-4506-99C2-FAFFD4E25D28
// Assembly location: C:\Users\Admin\Desktop\RE\1\System.Runtime.Serialization.Formatters.dll


namespace System.Runtime.Serialization.Formatters.Binary
{
  internal sealed class MemberReference : IStreamable
  {
    internal int _idRef;

    internal MemberReference()
    {
    }

    internal void Set(int idRef) => this._idRef = idRef;

    public void Write(BinaryFormatterWriter output)
    {
      output.WriteByte((byte) 9);
      output.WriteInt32(this._idRef);
    }

    public void Read(BinaryParser input) => this._idRef = input.ReadInt32();
  }
}
