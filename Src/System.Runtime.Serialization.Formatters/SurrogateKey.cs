// Decompiled with JetBrains decompiler
// Type: System.Runtime.Serialization.SurrogateKey
// Assembly: System.Runtime.Serialization.Formatters, Version=4.0.3.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 59FEC222-A9B8-4506-99C2-FAFFD4E25D28
// Assembly location: C:\Users\Admin\Desktop\RE\1\System.Runtime.Serialization.Formatters.dll


namespace System.Runtime.Serialization
{
  internal sealed class SurrogateKey
  {
    internal readonly Type _type;
    internal readonly StreamingContext _context;

    internal SurrogateKey(Type type, StreamingContext context)
    {
      this._type = type;
      this._context = context;
    }

    public override int GetHashCode() => this._type.GetHashCode();
  }
}
