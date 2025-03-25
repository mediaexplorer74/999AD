// Decompiled with JetBrains decompiler
// Type: System.Runtime.Serialization.Formatters.Binary.NameCache
// Assembly: System.Runtime.Serialization.Formatters, Version=4.0.3.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 59FEC222-A9B8-4506-99C2-FAFFD4E25D28
// Assembly location: C:\Users\Admin\Desktop\RE\1\System.Runtime.Serialization.Formatters.dll

using System.Collections.Concurrent;


namespace System.Runtime.Serialization.Formatters.Binary
{
  internal sealed class NameCache
  {
    private static readonly ConcurrentDictionary<string, object> s_ht = new ConcurrentDictionary<string, object>();
    private string _name;

    internal object GetCachedValue(string name)
    {
      this._name = name;
      object obj;
      return !NameCache.s_ht.TryGetValue(name, out obj) ? (object) null : obj;
    }

    internal void SetCachedValue(object value) => NameCache.s_ht[this._name] = value;
  }
}
