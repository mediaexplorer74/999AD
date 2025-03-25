// Decompiled with JetBrains decompiler
// Type: System.Runtime.Serialization.Formatters.Binary.ObjectMap
// Assembly: System.Runtime.Serialization.Formatters, Version=4.0.3.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 59FEC222-A9B8-4506-99C2-FAFFD4E25D28
// Assembly location: C:\Users\Admin\Desktop\RE\1\System.Runtime.Serialization.Formatters.dll



namespace System.Runtime.Serialization.Formatters.Binary
{
    public class SerializationInfo
    {
        internal int MemberCount;
        internal string FullTypeName;
        internal string AssemblyName;
        internal bool IsFullTypeNameSetExplicit;
        internal Type ObjectType;
        internal bool IsAssemblyNameSetExplicit;
        private Type objectType;
        private IFormatterConverter formatterConverter;

        public SerializationInfo(Type objectType, IFormatterConverter formatterConverter)
        {
            this.objectType = objectType;
            this.formatterConverter = formatterConverter;
        }

        internal void AddValue(string name, object value)
        {
            throw new NotImplementedException();
        }

        internal SerializationInfoEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}