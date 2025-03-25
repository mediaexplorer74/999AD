// Decompiled with JetBrains decompiler
// Type: System.Runtime.Serialization.ISerializationSurrogate
// Assembly: System.Runtime.Serialization.Formatters, Version=4.0.3.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 59FEC222-A9B8-4506-99C2-FAFFD4E25D28
// Assembly location: C:\Users\Admin\Desktop\RE\1\System.Runtime.Serialization.Formatters.dll


using System.Runtime.Serialization.Formatters.Binary;

namespace System.Runtime.Serialization
{
  public interface ISerializationSurrogate
  {
    void GetObjectData(object obj, SerializationInfo si, StreamingContext context);
    object SetObjectData(
      object obj,
      SerializationInfo info,
      StreamingContext context,
      ISurrogateSelector selector);
  }
}
