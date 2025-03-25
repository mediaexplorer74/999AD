// Decompiled with JetBrains decompiler
// Type: System.Runtime.Serialization.ISurrogateSelector
// Assembly: System.Runtime.Serialization.Formatters, Version=4.0.3.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 59FEC222-A9B8-4506-99C2-FAFFD4E25D28
// Assembly location: C:\Users\Admin\Desktop\RE\1\System.Runtime.Serialization.Formatters.dll


namespace System.Runtime.Serialization
{
  public interface ISurrogateSelector
  {
    void ChainSelector(ISurrogateSelector selector);

    ISerializationSurrogate GetSurrogate(
      Type type,
      StreamingContext context,
      out ISurrogateSelector selector);

    ISurrogateSelector GetNextSelector();
  }
}
