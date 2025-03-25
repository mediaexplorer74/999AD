// Decompiled with JetBrains decompiler
// Type: System.Runtime.Serialization.Formatter
// Assembly: System.Runtime.Serialization.Formatters, Version=4.0.3.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 59FEC222-A9B8-4506-99C2-FAFFD4E25D28
// Assembly location: C:\Users\Admin\Desktop\RE\1\System.Runtime.Serialization.Formatters.dll

using System.Collections;
using System.Globalization;
using System.IO;
using System.Reflection;


namespace System.Runtime.Serialization
{
  [CLSCompliant(false)]
  public abstract class Formatter : IFormatter
  {
    protected ObjectIDGenerator m_idGenerator;
    protected Queue m_objectQueue;

    protected Formatter()
    {
      this.m_objectQueue = new Queue();
      this.m_idGenerator = new ObjectIDGenerator();
    }

    public abstract object Deserialize(Stream serializationStream);

    protected virtual object GetNext(out long objID)
    {
      if (this.m_objectQueue.Count == 0)
      {
        objID = 0L;
        return (object) null;
      }
      object next = this.m_objectQueue.Dequeue();
      bool firstTime;
      objID = this.m_idGenerator.HasId(next, out firstTime);
      if (firstTime)
        throw new SerializationException(SR.Serialization_NoID);
      return next;
    }

    protected virtual long Schedule(object obj)
    {
      if (obj == null)
        return 0;
      bool firstTime;
      long id = this.m_idGenerator.GetId(obj, out firstTime);
      if (firstTime)
        this.m_objectQueue.Enqueue(obj);
      return id;
    }

    public abstract void Serialize(Stream serializationStream, object graph);

    protected abstract void WriteArray(object obj, string name, Type memberType);

    protected abstract void WriteBoolean(bool val, string name);

    protected abstract void WriteByte(byte val, string name);

    protected abstract void WriteChar(char val, string name);

    protected abstract void WriteDateTime(DateTime val, string name);

    protected abstract void WriteDecimal(Decimal val, string name);

    protected abstract void WriteDouble(double val, string name);

    protected abstract void WriteInt16(short val, string name);

    protected abstract void WriteInt32(int val, string name);

    protected abstract void WriteInt64(long val, string name);

    protected abstract void WriteObjectRef(object obj, string name, Type memberType);

    protected virtual void WriteMember(string memberName, object data)
    {
      if (data == null)
      {
        this.WriteObjectRef(data, memberName, typeof (object));
      }
      else
      {
        Type type = data.GetType();
        if (type == typeof (bool))
          this.WriteBoolean(Convert.ToBoolean(data, (IFormatProvider) CultureInfo.InvariantCulture), memberName);
        else if (type == typeof (char))
          this.WriteChar(Convert.ToChar(data, (IFormatProvider) CultureInfo.InvariantCulture), memberName);
        else if (type == typeof (sbyte))
          this.WriteSByte(Convert.ToSByte(data, (IFormatProvider) CultureInfo.InvariantCulture), memberName);
        else if (type == typeof (byte))
          this.WriteByte(Convert.ToByte(data, (IFormatProvider) CultureInfo.InvariantCulture), memberName);
        else if (type == typeof (short))
          this.WriteInt16(Convert.ToInt16(data, (IFormatProvider) CultureInfo.InvariantCulture), memberName);
        else if (type == typeof (int))
          this.WriteInt32(Convert.ToInt32(data, (IFormatProvider) CultureInfo.InvariantCulture), memberName);
        else if (type == typeof (long))
          this.WriteInt64(Convert.ToInt64(data, (IFormatProvider) CultureInfo.InvariantCulture), memberName);
        else if (type == typeof (float))
          this.WriteSingle(Convert.ToSingle(data, (IFormatProvider) CultureInfo.InvariantCulture), memberName);
        else if (type == typeof (double))
          this.WriteDouble(Convert.ToDouble(data, (IFormatProvider) CultureInfo.InvariantCulture), memberName);
        else if (type == typeof (DateTime))
          this.WriteDateTime(Convert.ToDateTime(data, (IFormatProvider) CultureInfo.InvariantCulture), memberName);
        else if (type == typeof (Decimal))
          this.WriteDecimal(Convert.ToDecimal(data, (IFormatProvider) CultureInfo.InvariantCulture), memberName);
        else if (type == typeof (ushort))
          this.WriteUInt16(Convert.ToUInt16(data, (IFormatProvider) CultureInfo.InvariantCulture), memberName);
        else if (type == typeof (uint))
          this.WriteUInt32(Convert.ToUInt32(data, (IFormatProvider) CultureInfo.InvariantCulture), memberName);
        else if (type == typeof (ulong))
          this.WriteUInt64(Convert.ToUInt64(data, (IFormatProvider) CultureInfo.InvariantCulture), memberName);
        else if (type.IsArray)
          this.WriteArray(data, memberName, type);
        else if (type.GetTypeInfo().IsValueType)
          this.WriteValueType(data, memberName, type);
        else
          this.WriteObjectRef(data, memberName, type);
      }
    }

    [CLSCompliant(false)]
    protected abstract void WriteSByte(sbyte val, string name);

    protected abstract void WriteSingle(float val, string name);

    protected abstract void WriteTimeSpan(TimeSpan val, string name);

    [CLSCompliant(false)]
    protected abstract void WriteUInt16(ushort val, string name);

    [CLSCompliant(false)]
    protected abstract void WriteUInt32(uint val, string name);

    [CLSCompliant(false)]
    protected abstract void WriteUInt64(ulong val, string name);

    protected abstract void WriteValueType(object obj, string name, Type memberType);

    public abstract ISurrogateSelector SurrogateSelector { get; set; }

    public abstract SerializationBinder Binder { get; set; }

    public abstract StreamingContext Context { get; set; }
  }
}
