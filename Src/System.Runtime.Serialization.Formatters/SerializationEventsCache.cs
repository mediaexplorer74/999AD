// Decompiled with JetBrains decompiler
// Type: System.Runtime.Serialization.SerializationEventsCache
// Assembly: System.Runtime.Serialization.Formatters, Version=4.0.3.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 59FEC222-A9B8-4506-99C2-FAFFD4E25D28
// Assembly location: C:\Users\Admin\Desktop\RE\1\System.Runtime.Serialization.Formatters.dll

using System.Collections.Concurrent;


namespace System.Runtime.Serialization
{
  internal static class SerializationEventsCache
  {
    private static readonly ConcurrentDictionary<Type, SerializationEvents> s_cache = new ConcurrentDictionary<Type, SerializationEvents>();

    internal static SerializationEvents GetSerializationEventsForType(Type t)
    {
      return SerializationEventsCache.s_cache.GetOrAdd(t, (Func<Type, SerializationEvents>) (type => new SerializationEvents(type)));
    }
  }
}
