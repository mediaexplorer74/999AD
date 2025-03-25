// Decompiled with JetBrains decompiler
// Type: System.Runtime.Serialization.Formatters.Binary.ValueFixup
// Assembly: System.Runtime.Serialization.Formatters, Version=4.0.3.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 59FEC222-A9B8-4506-99C2-FAFFD4E25D28
// Assembly location: C:\Users\Admin\Desktop\RE\1\System.Runtime.Serialization.Formatters.dll

using System.Reflection;


namespace System.Runtime.Serialization.Formatters.Binary
{
  internal sealed class ValueFixup
  {
    internal ValueFixupEnum _valueFixupEnum;
    internal Array _arrayObj;
    internal int[] _indexMap;
    internal object _header;
    internal object _memberObject;
    internal ReadObjectInfo _objectInfo;
    internal string _memberName;

    internal ValueFixup(Array arrayObj, int[] indexMap)
    {
      this._valueFixupEnum = ValueFixupEnum.Array;
      this._arrayObj = arrayObj;
      this._indexMap = indexMap;
    }

    internal ValueFixup(object memberObject, string memberName, ReadObjectInfo objectInfo)
    {
      this._valueFixupEnum = ValueFixupEnum.Member;
      this._memberObject = memberObject;
      this._memberName = memberName;
      this._objectInfo = objectInfo;
    }

    internal void Fixup(ParseRecord record, ParseRecord parent)
    {
      object newObj = record._newObj;
      switch (this._valueFixupEnum)
      {
        case ValueFixupEnum.Array:
          this._arrayObj.SetValue(newObj, this._indexMap);
          break;
        case ValueFixupEnum.Header:
          throw new PlatformNotSupportedException();
        case ValueFixupEnum.Member:
          if (this._objectInfo._isSi)
          {
            this._objectInfo._objectManager.RecordDelayedFixup(parent._objectId, this._memberName, record._objectId);
            break;
          }
          MemberInfo memberInfo = this._objectInfo.GetMemberInfo(this._memberName);
          if (!(memberInfo != null))
             break;
          this._objectInfo._objectManager.RecordFixup(parent._objectId, memberInfo, record._objectId);
          break;
      }
    }
  }
}
