// Decompiled with JetBrains decompiler
// Type: System.Runtime.Serialization.MemberHolder
// Assembly: System.Runtime.Serialization.Formatters, Version=4.0.3.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 59FEC222-A9B8-4506-99C2-FAFFD4E25D28
// Assembly location: C:\Users\Admin\Desktop\RE\1\System.Runtime.Serialization.Formatters.dll

using System.Reflection;


namespace System.Runtime.Serialization
{
  internal sealed class MemberHolder
  {
    internal readonly MemberInfo[] _members;
    internal readonly Type _memberType;
    internal readonly StreamingContext _context;

    internal MemberHolder(Type type, StreamingContext ctx)
    {
      this._memberType = type;
      this._context = ctx;
    }

    public override int GetHashCode() => this._memberType.GetHashCode();

    public override bool Equals(object obj)
    {
            //RnD
      return obj is MemberHolder memberHolder
                && (object) memberHolder._memberType == (object)
                this._memberType/* && memberHolder._context.State == this._context.State*/;
    }
  }
}
