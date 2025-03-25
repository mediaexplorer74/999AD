// Decompiled with JetBrains decompiler
// Type: System.Runtime.Serialization.FixupHolder
// Assembly: System.Runtime.Serialization.Formatters, Version=4.0.3.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 59FEC222-A9B8-4506-99C2-FAFFD4E25D28
// Assembly location: C:\Users\Admin\Desktop\RE\1\System.Runtime.Serialization.Formatters.dll


namespace System.Runtime.Serialization
{
  internal sealed class FixupHolder
  {
    internal const int ArrayFixup = 1;
    internal const int MemberFixup = 2;
    internal const int DelayedFixup = 4;
    internal long _id;
    internal object _fixupInfo;
    internal readonly int _fixupType;

    internal FixupHolder(long id, object fixupInfo, int fixupType)
    {
      this._id = id;
      this._fixupInfo = fixupInfo;
      this._fixupType = fixupType;
    }
  }
}
