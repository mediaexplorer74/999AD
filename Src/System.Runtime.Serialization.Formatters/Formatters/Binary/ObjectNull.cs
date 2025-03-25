// Decompiled with JetBrains decompiler
// Type: System.Runtime.Serialization.Formatters.Binary.ObjectNull
// Assembly: System.Runtime.Serialization.Formatters, Version=4.0.3.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 59FEC222-A9B8-4506-99C2-FAFFD4E25D28
// Assembly location: C:\Users\Admin\Desktop\RE\1\System.Runtime.Serialization.Formatters.dll


namespace System.Runtime.Serialization.Formatters.Binary
{
  internal sealed class ObjectNull : IStreamable
  {
    internal int _nullCount;

    internal ObjectNull()
    {
    }

    internal void SetNullCount(int nullCount) => this._nullCount = nullCount;

    public void Write(BinaryFormatterWriter output)
    {
      if (this._nullCount == 1)
        output.WriteByte((byte) 10);
      else if (this._nullCount < 256)
      {
        output.WriteByte((byte) 13);
        output.WriteByte((byte) this._nullCount);
      }
      else
      {
        output.WriteByte((byte) 14);
        output.WriteInt32(this._nullCount);
      }
    }

    public void Read(BinaryParser input) => this.Read(input, BinaryHeaderEnum.ObjectNull);

    public void Read(BinaryParser input, BinaryHeaderEnum binaryHeaderEnum)
    {
      switch (binaryHeaderEnum)
      {
        case BinaryHeaderEnum.ObjectNull:
          this._nullCount = 1;
          break;
        case BinaryHeaderEnum.ObjectNullMultiple256:
          this._nullCount = (int) input.ReadByte();
          break;
        case BinaryHeaderEnum.ObjectNullMultiple:
          this._nullCount = input.ReadInt32();
          break;
      }
    }
  }
}
