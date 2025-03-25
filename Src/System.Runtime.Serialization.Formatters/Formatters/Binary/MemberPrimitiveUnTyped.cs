// Decompiled with JetBrains decompiler
// Type: System.Runtime.Serialization.Formatters.Binary.MemberPrimitiveUnTyped
// Assembly: System.Runtime.Serialization.Formatters, Version=4.0.3.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 59FEC222-A9B8-4506-99C2-FAFFD4E25D28
// Assembly location: C:\Users\Admin\Desktop\RE\1\System.Runtime.Serialization.Formatters.dll


namespace System.Runtime.Serialization.Formatters.Binary
{
  internal sealed class MemberPrimitiveUnTyped : IStreamable
  {
    internal InternalPrimitiveTypeE _typeInformation;
    internal object _value;

    internal MemberPrimitiveUnTyped()
    {
    }

    internal void Set(InternalPrimitiveTypeE typeInformation, object value)
    {
      this._typeInformation = typeInformation;
      this._value = value;
    }

    internal void Set(InternalPrimitiveTypeE typeInformation)
    {
      this._typeInformation = typeInformation;
    }

    public void Write(BinaryFormatterWriter output)
    {
      output.WriteValue(this._typeInformation, this._value);
    }

    public void Read(BinaryParser input) => this._value = input.ReadValue(this._typeInformation);
  }
}
