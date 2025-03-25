// Decompiled with JetBrains decompiler
// Type: System.Runtime.Serialization.FormatterConverter
// Assembly: System.Runtime.Serialization.Formatters, Version=4.0.3.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 59FEC222-A9B8-4506-99C2-FAFFD4E25D28
// Assembly location: C:\Users\Admin\Desktop\RE\1\System.Runtime.Serialization.Formatters.dll

using System.Globalization;
using System.Runtime.CompilerServices;


namespace System.Runtime.Serialization
{
  public class FormatterConverter : IFormatterConverter
  {
    public object Convert(object value, Type type)
    {
      if (value == null)
        FormatterConverter.ThrowValueNullException();
      return System.Convert.ChangeType(value, type, (IFormatProvider) CultureInfo.InvariantCulture);
    }

    public object Convert(object value, TypeCode typeCode)
    {
      if (value == null)
        FormatterConverter.ThrowValueNullException();
      return System.Convert.ChangeType(value, typeCode, (IFormatProvider) CultureInfo.InvariantCulture);
    }

    public bool ToBoolean(object value)
    {
      if (value == null)
        FormatterConverter.ThrowValueNullException();
      return System.Convert.ToBoolean(value, (IFormatProvider) CultureInfo.InvariantCulture);
    }

    public char ToChar(object value)
    {
      if (value == null)
        FormatterConverter.ThrowValueNullException();
      return System.Convert.ToChar(value, (IFormatProvider) CultureInfo.InvariantCulture);
    }

    [CLSCompliant(false)]
    public sbyte ToSByte(object value)
    {
      if (value == null)
        FormatterConverter.ThrowValueNullException();
      return System.Convert.ToSByte(value, (IFormatProvider) CultureInfo.InvariantCulture);
    }

    public byte ToByte(object value)
    {
      if (value == null)
        FormatterConverter.ThrowValueNullException();
      return System.Convert.ToByte(value, (IFormatProvider) CultureInfo.InvariantCulture);
    }

    public short ToInt16(object value)
    {
      if (value == null)
        FormatterConverter.ThrowValueNullException();
      return System.Convert.ToInt16(value, (IFormatProvider) CultureInfo.InvariantCulture);
    }

    [CLSCompliant(false)]
    public ushort ToUInt16(object value)
    {
      if (value == null)
        FormatterConverter.ThrowValueNullException();
      return System.Convert.ToUInt16(value, (IFormatProvider) CultureInfo.InvariantCulture);
    }

    public int ToInt32(object value)
    {
      if (value == null)
        FormatterConverter.ThrowValueNullException();
      return System.Convert.ToInt32(value, (IFormatProvider) CultureInfo.InvariantCulture);
    }

    [CLSCompliant(false)]
    public uint ToUInt32(object value)
    {
      if (value == null)
        FormatterConverter.ThrowValueNullException();
      return System.Convert.ToUInt32(value, (IFormatProvider) CultureInfo.InvariantCulture);
    }

    public long ToInt64(object value)
    {
      if (value == null)
        FormatterConverter.ThrowValueNullException();
      return System.Convert.ToInt64(value, (IFormatProvider) CultureInfo.InvariantCulture);
    }

    [CLSCompliant(false)]
    public ulong ToUInt64(object value)
    {
      if (value == null)
        FormatterConverter.ThrowValueNullException();
      return System.Convert.ToUInt64(value, (IFormatProvider) CultureInfo.InvariantCulture);
    }

    public float ToSingle(object value)
    {
      if (value == null)
        FormatterConverter.ThrowValueNullException();
      return System.Convert.ToSingle(value, (IFormatProvider) CultureInfo.InvariantCulture);
    }

    public double ToDouble(object value)
    {
      if (value == null)
        FormatterConverter.ThrowValueNullException();
      return System.Convert.ToDouble(value, (IFormatProvider) CultureInfo.InvariantCulture);
    }

    public Decimal ToDecimal(object value)
    {
      if (value == null)
        FormatterConverter.ThrowValueNullException();
      return System.Convert.ToDecimal(value, (IFormatProvider) CultureInfo.InvariantCulture);
    }

    public DateTime ToDateTime(object value)
    {
      if (value == null)
        FormatterConverter.ThrowValueNullException();
      return System.Convert.ToDateTime(value, (IFormatProvider) CultureInfo.InvariantCulture);
    }

    public string ToString(object value)
    {
      if (value == null)
        FormatterConverter.ThrowValueNullException();
      return System.Convert.ToString(value, (IFormatProvider) CultureInfo.InvariantCulture);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static void ThrowValueNullException() => throw new ArgumentNullException("value");
  }
}
