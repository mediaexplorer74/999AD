// Decompiled with JetBrains decompiler
// Type: System.Runtime.Serialization.SurrogateForCyclicalReference
// Assembly: System.Runtime.Serialization.Formatters, Version=4.0.3.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 59FEC222-A9B8-4506-99C2-FAFFD4E25D28
// Assembly location: C:\Users\Admin\Desktop\RE\1\System.Runtime.Serialization.Formatters.dll


using System.Runtime.Serialization.Formatters.Binary;

namespace System.Runtime.Serialization
{
  internal sealed class SurrogateForCyclicalReference : ISerializationSurrogate
  {
    private readonly ISerializationSurrogate _innerSurrogate;

    internal SurrogateForCyclicalReference(ISerializationSurrogate innerSurrogate)
    {
      this._innerSurrogate = innerSurrogate;
    }

    public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
    {
      this._innerSurrogate.GetObjectData(obj, info, context);
    }

    public object SetObjectData(
      object obj,
      SerializationInfo info,
      StreamingContext context,
      ISurrogateSelector selector)
    {
      return this._innerSurrogate.SetObjectData(obj, info, context, selector);
    }
  }
}
