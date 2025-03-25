// Decompiled with JetBrains decompiler
// Type: System.Runtime.Serialization.SerializationInfoExtensions
// Assembly: System.Runtime.Serialization.Formatters, Version=4.0.3.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 59FEC222-A9B8-4506-99C2-FAFFD4E25D28
// Assembly location: C:\Users\Admin\Desktop\RE\1\System.Runtime.Serialization.Formatters.dll

using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;


namespace System.Runtime.Serialization
{
  internal static class SerializationInfoExtensions
  {
    private static readonly Action<SerializationInfo, string, object, Type> s_updateValue = (Action<SerializationInfo, string, object, Type>) typeof (SerializationInfo).GetMethod("UpdateValue", (BindingFlags) 52).CreateDelegate(typeof (Action<SerializationInfo, string, object, Type>));

    public static void UpdateValue(
      this SerializationInfo si,
      string name,
      object value,
      Type type)
    {
      SerializationInfoExtensions.s_updateValue(si, name, value, type);
    }
  }
}
