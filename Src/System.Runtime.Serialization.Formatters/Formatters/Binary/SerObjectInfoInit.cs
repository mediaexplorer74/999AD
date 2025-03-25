// Decompiled with JetBrains decompiler
// Type: System.Runtime.Serialization.Formatters.Binary.SerObjectInfoInit
// Assembly: System.Runtime.Serialization.Formatters, Version=4.0.3.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 59FEC222-A9B8-4506-99C2-FAFFD4E25D28
// Assembly location: C:\Users\Admin\Desktop\RE\1\System.Runtime.Serialization.Formatters.dll

using System.Collections.Generic;


namespace System.Runtime.Serialization.Formatters.Binary
{
  internal sealed class SerObjectInfoInit
  {
    internal readonly Dictionary<Type, SerObjectInfoCache> _seenBeforeTable = new Dictionary<Type, SerObjectInfoCache>();
    internal int _objectInfoIdCount = 1;
    internal SerStack _oiPool = new SerStack("SerObjectInfo Pool");
  }
}
