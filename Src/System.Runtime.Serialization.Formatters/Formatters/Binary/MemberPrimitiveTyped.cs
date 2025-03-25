// Decompiled with JetBrains decompiler
// Type: System.Runtime.Serialization.Formatters.Binary.MemberPrimitiveTyped
// Assembly: System.Runtime.Serialization.Formatters, Version=4.0.3.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 59FEC222-A9B8-4506-99C2-FAFFD4E25D28
// Assembly location: C:\Users\Admin\Desktop\RE\1\System.Runtime.Serialization.Formatters.dll


namespace System.Runtime.Serialization.Formatters.Binary
{
  internal sealed class MemberPrimitiveTyped : IStreamable
  {
    internal InternalPrimitiveTypeE _primitiveTypeEnum;
    internal object _value;

    internal MemberPrimitiveTyped()
    {
    }

    internal void Set(InternalPrimitiveTypeE primitiveTypeEnum, object value)
    {
      this._primitiveTypeEnum = primitiveTypeEnum;
      this._value = value;
    }

    public void Write(BinaryFormatterWriter output)
    {
      output.WriteByte((byte) 8);
      output.WriteByte((byte) this._primitiveTypeEnum);
      output.WriteValue(this._primitiveTypeEnum, this._value);
    }

    public void Read(BinaryParser input)
    {
      this._primitiveTypeEnum = (InternalPrimitiveTypeE) input.ReadByte();
      this._value = input.ReadValue(this._primitiveTypeEnum);
    }
  }
}
