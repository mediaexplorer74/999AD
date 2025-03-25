// Decompiled with JetBrains decompiler
// Type: System.Runtime.Serialization.IFormatter
// Assembly: System.Runtime.Serialization.Formatters, Version=4.0.3.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 59FEC222-A9B8-4506-99C2-FAFFD4E25D28
// Assembly location: C:\Users\Admin\Desktop\RE\1\System.Runtime.Serialization.Formatters.dll

using System.IO;


namespace System.Runtime.Serialization
{
  public interface IFormatter
  {
    object Deserialize(Stream serializationStream);

    void Serialize(Stream serializationStream, object graph);

    ISurrogateSelector SurrogateSelector { get; set; }

    SerializationBinder Binder { get; set; }

    StreamingContext Context { get; set; }
  }
}
