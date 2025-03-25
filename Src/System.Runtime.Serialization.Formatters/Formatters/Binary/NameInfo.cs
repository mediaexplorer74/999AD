// Decompiled with JetBrains decompiler
// Type: System.Runtime.Serialization.Formatters.Binary.NameInfo
// Assembly: System.Runtime.Serialization.Formatters, Version=4.0.3.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 59FEC222-A9B8-4506-99C2-FAFFD4E25D28
// Assembly location: C:\Users\Admin\Desktop\RE\1\System.Runtime.Serialization.Formatters.dll


using System.Reflection;

namespace System.Runtime.Serialization.Formatters.Binary
{
  internal sealed class NameInfo
  {
    internal string _fullName;
    internal long _objectId;
    internal long _assemId;
    internal InternalPrimitiveTypeE _primitiveTypeEnum;
    internal Type _type;
    internal bool _isSealed;
    internal bool _isArray;
    internal bool _isArrayItem;
    internal bool _transmitTypeOnObject;
    internal bool _transmitTypeOnMember;
    internal bool _isParentTypeOnObject;
    internal InternalArrayTypeE _arrayEnum;
    private bool _sealedStatusChecked;

    internal NameInfo()
    {
    }

    internal void Init()
    {
      this._fullName = (string) null;
      this._objectId = 0L;
      this._assemId = 0L;
      this._primitiveTypeEnum = InternalPrimitiveTypeE.Invalid;
      this._type = (Type) null;
      this._isSealed = false;
      this._transmitTypeOnObject = false;
      this._transmitTypeOnMember = false;
      this._isParentTypeOnObject = false;
      this._isArray = false;
      this._isArrayItem = false;
      this._arrayEnum = InternalArrayTypeE.Empty;
      this._sealedStatusChecked = false;
    }

        public bool IsSealed
        {
            get
            {
                if (!this._sealedStatusChecked)
                {
                    this._isSealed = this._type.GetTypeInfo().IsSealed;
                    this._sealedStatusChecked = true;
                }
                return this._isSealed;
            }
        }

    public string NIname
    {
      get => this._fullName ?? (this._fullName = this._type.FullName);
      set => this._fullName = value;
    }
  }
}
