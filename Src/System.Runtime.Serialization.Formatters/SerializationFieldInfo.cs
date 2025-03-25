// Decompiled with JetBrains decompiler
// Type: System.Runtime.Serialization.SerializationFieldInfo
// Assembly: System.Runtime.Serialization.Formatters, Version=4.0.3.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 59FEC222-A9B8-4506-99C2-FAFFD4E25D28
// Assembly location: C:\Users\Admin\Desktop\RE\1\System.Runtime.Serialization.Formatters.dll

using System.Globalization;
using System.Reflection;


namespace System.Runtime.Serialization
{
    internal sealed class SerializationFieldInfo //: FieldInfo
    {
        private readonly FieldInfo m_field;
        private readonly string m_serializationName;
        private FieldInfo field;
        private string namePrefix;

        public SerializationFieldInfo(FieldInfo field, string namePrefix)
        {
            this.field = field;
            this.namePrefix = namePrefix;
        }

        //internal SerializationFieldInfo(FieldInfo field, string namePrefix)
        //{
        //    this.m_field = field;
        //    this.m_serializationName = namePrefix + "+" + ((MemberInfo)this.m_field).Name;
        //}

        internal FieldInfo FieldInfo => this.m_field;

        public static explicit operator FieldInfo(SerializationFieldInfo v)
        {
            throw new NotImplementedException();
        }

        //public virtual string Name => this.m_serializationName;

        //public virtual Module Module => ((MemberInfo)this.m_field).Module;

        //public virtual int MetadataToken => default;//((MemberInfo)this.m_field).MetadataToken;

        //public virtual Type DeclaringType => default;//((MemberInfo)this.m_field).DeclaringType;

        //public virtual Type ReflectedType => default;//((MemberInfo)this.m_field).ReflectedType;

        //public virtual object[] GetCustomAttributes(bool inherit)
        //{
        //    return default;//((MemberInfo)this.m_field).GetCustomAttributes(inherit);
        //}

        //public virtual object[] GetCustomAttributes(Type attributeType, bool inherit)
        //{
        //    return default;//((MemberInfo)this.m_field).GetCustomAttributes(attributeType, inherit);
        //}

        //public virtual bool IsDefined(Type attributeType, bool inherit)
        //{
        //    return ((MemberInfo)this.m_field).IsDefined(attributeType, inherit);
        //}

        //public override Type FieldType => this.m_field.FieldType;

        //public override object GetValue(object obj) => this.m_field.GetValue(obj);

        //public virtual void SetValue(
        //  object obj,
        //  object value,
        //  BindingFlags invokeAttr,
        //  Binder binder,
        //  CultureInfo culture)
        //{
        //    this.m_field.SetValue(obj, value, invokeAttr, binder, culture);
        //}

        //public virtual RuntimeFieldHandle FieldHandle
        //{
        //    get
        //    {
        //        return this.m_field.FieldHandle;
        //    }
        //}

        //public override FieldAttributes Attributes => this.m_field.Attributes;

        //public override Type DeclaringType => throw new NotImplementedException();
    }
}
