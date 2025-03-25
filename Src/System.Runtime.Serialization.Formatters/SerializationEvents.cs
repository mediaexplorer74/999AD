// Decompiled with JetBrains decompiler
// Type: System.Runtime.Serialization.SerializationEvents
// Assembly: System.Runtime.Serialization.Formatters, Version=4.0.3.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 59FEC222-A9B8-4506-99C2-FAFFD4E25D28
// Assembly location: C:\Users\Admin\Desktop\RE\1\System.Runtime.Serialization.Formatters.dll

using System.Collections.Generic;
using System.Reflection;


namespace System.Runtime.Serialization
{
  internal sealed class SerializationEvents
  {
    private readonly List<MethodInfo> _onSerializingMethods;
    private readonly List<MethodInfo> _onSerializedMethods;
    private readonly List<MethodInfo> _onDeserializingMethods;
    private readonly List<MethodInfo> _onDeserializedMethods;

    internal SerializationEvents(Type t)
    {
      this._onSerializingMethods = this.GetMethodsWithAttribute(typeof (OnSerializingAttribute), t);
      this._onSerializedMethods = this.GetMethodsWithAttribute(typeof (OnSerializedAttribute), t);
      this._onDeserializingMethods = this.GetMethodsWithAttribute(typeof (OnDeserializingAttribute), t);
      this._onDeserializedMethods = this.GetMethodsWithAttribute(typeof (OnDeserializedAttribute), t);
    }

    private List<MethodInfo> GetMethodsWithAttribute(Type attribute, Type t)
    {
      List<MethodInfo> methodsWithAttribute = (List<MethodInfo>) null;
      for (Type type = t; type != (Type) null && type != typeof (object); type = type.GetTypeInfo().BaseType)
      {
        foreach (MethodInfo method in type.GetMethods((BindingFlags) 54))
        {
          if (((MemberInfo) method).IsDefined(attribute, false))
          {
            if (methodsWithAttribute == null)
              methodsWithAttribute = new List<MethodInfo>();
            methodsWithAttribute.Add(method);
          }
        }
      }
      methodsWithAttribute?.Reverse();
      return methodsWithAttribute;
    }

    internal bool HasOnSerializingEvents
    {
      get => this._onSerializingMethods != null || this._onSerializedMethods != null;
    }

    internal void InvokeOnSerializing(object obj, StreamingContext context)
    {
      SerializationEvents.InvokeOnDelegate(obj, context, this._onSerializingMethods);
    }

    internal void InvokeOnDeserializing(object obj, StreamingContext context)
    {
      SerializationEvents.InvokeOnDelegate(obj, context, this._onDeserializingMethods);
    }

    internal void InvokeOnDeserialized(object obj, StreamingContext context)
    {
      SerializationEvents.InvokeOnDelegate(obj, context, this._onDeserializedMethods);
    }

    internal SerializationEventHandler AddOnSerialized(
      object obj,
      SerializationEventHandler handler)
    {
      return SerializationEvents.AddOnDelegate(obj, handler, this._onSerializedMethods);
    }

    internal SerializationEventHandler AddOnDeserialized(
      object obj,
      SerializationEventHandler handler)
    {
      return SerializationEvents.AddOnDelegate(obj, handler, this._onDeserializedMethods);
    }

    private static void InvokeOnDelegate(
      object obj,
      StreamingContext context,
      List<MethodInfo> methods)
    {
      SerializationEventHandler serializationEventHandler = SerializationEvents.AddOnDelegate(obj, (SerializationEventHandler) null, methods);
      if (serializationEventHandler == null)
        return;
      serializationEventHandler(context);
    }

    private static SerializationEventHandler AddOnDelegate(
      object obj,
      SerializationEventHandler handler,
      List<MethodInfo> methods)
    {
      if (methods != null)
      {
        foreach (MethodInfo method in methods)
        {
          SerializationEventHandler serializationEventHandler = (SerializationEventHandler) method.CreateDelegate(typeof (SerializationEventHandler), obj);
          handler += serializationEventHandler;
        }
      }
      return handler;
    }
  }
}
