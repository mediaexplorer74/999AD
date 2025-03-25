// Decompiled with JetBrains decompiler
// Type: System.Runtime.Serialization.Formatters.Binary.PrimitiveArray
// Assembly: System.Runtime.Serialization.Formatters, Version=4.0.3.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 59FEC222-A9B8-4506-99C2-FAFFD4E25D28
// Assembly location: C:\Users\Admin\Desktop\RE\1\System.Runtime.Serialization.Formatters.dll

using System.Globalization;


namespace System.Runtime.Serialization.Formatters.Binary
{
  internal sealed class PrimitiveArray
  {
    private InternalPrimitiveTypeE _code;
    private bool[] _booleanA;
    private char[] _charA;
    private double[] _doubleA;
    private short[] _int16A;
    private int[] _int32A;
    private long[] _int64A;
    private sbyte[] _sbyteA;
    private float[] _singleA;
    private ushort[] _uint16A;
    private uint[] _uint32A;
    private ulong[] _uint64A;

    internal PrimitiveArray(InternalPrimitiveTypeE code, Array array)
    {
      this._code = code;
      switch (code)
      {
        case InternalPrimitiveTypeE.Boolean:
          this._booleanA = (bool[]) array;
          break;
        case InternalPrimitiveTypeE.Char:
          this._charA = (char[]) array;
          break;
        case InternalPrimitiveTypeE.Double:
          this._doubleA = (double[]) array;
          break;
        case InternalPrimitiveTypeE.Int16:
          this._int16A = (short[]) array;
          break;
        case InternalPrimitiveTypeE.Int32:
          this._int32A = (int[]) array;
          break;
        case InternalPrimitiveTypeE.Int64:
          this._int64A = (long[]) array;
          break;
        case InternalPrimitiveTypeE.SByte:
          this._sbyteA = (sbyte[]) array;
          break;
        case InternalPrimitiveTypeE.Single:
          this._singleA = (float[]) array;
          break;
        case InternalPrimitiveTypeE.UInt16:
          this._uint16A = (ushort[]) array;
          break;
        case InternalPrimitiveTypeE.UInt32:
          this._uint32A = (uint[]) array;
          break;
        case InternalPrimitiveTypeE.UInt64:
          this._uint64A = (ulong[]) array;
          break;
      }
    }

    internal void SetValue(string value, int index)
    {
      switch (this._code)
      {
        case InternalPrimitiveTypeE.Boolean:
          this._booleanA[index] = bool.Parse(value);
          break;
        case InternalPrimitiveTypeE.Char:
          if (value[0] == '_' && value.Equals("_0x00_"))
          {
            this._charA[index] = char.MinValue;
            break;
          }
          this._charA[index] = char.Parse(value);
          break;
        case InternalPrimitiveTypeE.Double:
          this._doubleA[index] = double.Parse(value, (IFormatProvider) CultureInfo.InvariantCulture);
          break;
        case InternalPrimitiveTypeE.Int16:
          this._int16A[index] = short.Parse(value, (IFormatProvider) CultureInfo.InvariantCulture);
          break;
        case InternalPrimitiveTypeE.Int32:
          this._int32A[index] = int.Parse(value, (IFormatProvider) CultureInfo.InvariantCulture);
          break;
        case InternalPrimitiveTypeE.Int64:
          this._int64A[index] = long.Parse(value, (IFormatProvider) CultureInfo.InvariantCulture);
          break;
        case InternalPrimitiveTypeE.SByte:
          this._sbyteA[index] = sbyte.Parse(value, (IFormatProvider) CultureInfo.InvariantCulture);
          break;
        case InternalPrimitiveTypeE.Single:
          this._singleA[index] = float.Parse(value, (IFormatProvider) CultureInfo.InvariantCulture);
          break;
        case InternalPrimitiveTypeE.UInt16:
          this._uint16A[index] = ushort.Parse(value, (IFormatProvider) CultureInfo.InvariantCulture);
          break;
        case InternalPrimitiveTypeE.UInt32:
          this._uint32A[index] = uint.Parse(value, (IFormatProvider) CultureInfo.InvariantCulture);
          break;
        case InternalPrimitiveTypeE.UInt64:
          this._uint64A[index] = ulong.Parse(value, (IFormatProvider) CultureInfo.InvariantCulture);
          break;
      }
    }
  }
}
