// Decompiled with JetBrains decompiler
// Type: System.Runtime.Serialization.SerializationObjectManager
// Assembly: System.Runtime.Serialization.Formatters, Version=4.0.3.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 59FEC222-A9B8-4506-99C2-FAFFD4E25D28
// Assembly location: C:\Users\Admin\Desktop\RE\1\System.Runtime.Serialization.Formatters.dll

using System.Collections.Generic;


namespace System.Runtime.Serialization
{
  public sealed class SerializationObjectManager
  {
    private readonly Dictionary<object, object> _objectSeenTable;
    private readonly StreamingContext _context;
    private SerializationEventHandler _onSerializedHandler;

    public SerializationObjectManager(StreamingContext context)
    {
      this._context = context;
      this._objectSeenTable = new Dictionary<object, object>();
    }

    public void RegisterObject(object obj)
    {
      SerializationEvents serializationEventsForType = SerializationEventsCache.GetSerializationEventsForType(obj.GetType());
      if (!serializationEventsForType.HasOnSerializingEvents /*|| !this._objectSeenTable.TryAdd(obj, (object) true)*/)
        return;
      serializationEventsForType.InvokeOnSerializing(obj, this._context);
      this.AddOnSerialized(obj);
    }

    public void RaiseOnSerializedEvent()
    {
      SerializationEventHandler serializedHandler = this._onSerializedHandler;
      if (serializedHandler == null)
        return;
      serializedHandler(this._context);
    }

    private void AddOnSerialized(object obj)
    {
      this._onSerializedHandler = SerializationEventsCache.GetSerializationEventsForType(obj.GetType()).AddOnSerialized(obj, this._onSerializedHandler);
    }
  }
}
