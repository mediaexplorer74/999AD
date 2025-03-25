// Decompiled with JetBrains decompiler
// Type: System.Runtime.Serialization.ObjectIDGenerator
// Assembly: System.Runtime.Serialization.Formatters, Version=4.0.3.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 59FEC222-A9B8-4506-99C2-FAFFD4E25D28
// Assembly location: C:\Users\Admin\Desktop\RE\1\System.Runtime.Serialization.Formatters.dll

using System.Runtime.CompilerServices;


namespace System.Runtime.Serialization
{
  public class ObjectIDGenerator
  {
    private const int NumBins = 4;
    private static readonly int[] s_sizes = new int[21]
    {
      5,
      11,
      29,
      47,
      97,
      197,
      397,
      797,
      1597,
      3203,
      6421,
      12853,
      25717,
      51437,
      102877,
      205759,
      411527,
      823117,
      1646237,
      3292489,
      6584983
    };
    internal int _currentCount;
    internal int _currentSize;
    internal long[] _ids;
    internal object[] _objs;

    public ObjectIDGenerator()
    {
      this._currentCount = 1;
      this._currentSize = ObjectIDGenerator.s_sizes[0];
      this._ids = new long[this._currentSize * 4];
      this._objs = new object[this._currentSize * 4];
    }

    private int FindElement(object obj, out bool found)
    {
      int hashCode = RuntimeHelpers.GetHashCode(obj);
      int num1 = 1 + (hashCode & int.MaxValue) % (this._currentSize - 2);
      while (true)
      {
        int num2 = (hashCode & int.MaxValue) % this._currentSize * 4;
        for (int element = num2; element < num2 + 4; ++element)
        {
          if (this._objs[element] == null)
          {
            found = false;
            return element;
          }
          if (this._objs[element] == obj)
          {
            found = true;
            return element;
          }
        }
        hashCode += num1;
      }
    }

    public virtual long GetId(object obj, out bool firstTime)
    {
      bool found;
      int index = obj != null ? this.FindElement(obj, out found) : throw new ArgumentNullException(nameof (obj));
      long id;
      if (!found)
      {
        this._objs[index] = obj;
        this._ids[index] = (long) this._currentCount++;
        id = this._ids[index];
        if (this._currentCount > this._currentSize * 4 / 2)
          this.Rehash();
      }
      else
        id = this._ids[index];
      firstTime = !found;
      return id;
    }

    public virtual long HasId(object obj, out bool firstTime)
    {
      bool found;
      int index = obj != null ? this.FindElement(obj, out found) : throw new ArgumentNullException(nameof (obj));
      if (found)
      {
        firstTime = false;
        return this._ids[index];
      }
      firstTime = true;
      return 0;
    }

    private void Rehash()
    {
      int index1 = 0;
      int currentSize = this._currentSize;
      while (index1 < ObjectIDGenerator.s_sizes.Length && ObjectIDGenerator.s_sizes[index1] <= currentSize)
        ++index1;
      if (index1 == ObjectIDGenerator.s_sizes.Length)
        throw new SerializationException(SR.Serialization_TooManyElements);
      this._currentSize = ObjectIDGenerator.s_sizes[index1];
      long[] numArray = new long[this._currentSize * 4];
      object[] objArray = new object[this._currentSize * 4];
      long[] ids = this._ids;
      object[] objs = this._objs;
      this._ids = numArray;
      this._objs = objArray;
      for (int index2 = 0; index2 < objs.Length; ++index2)
      {
        if (objs[index2] != null)
        {
          int element = this.FindElement(objs[index2], out bool _);
          this._objs[element] = objs[index2];
          this._ids[element] = ids[index2];
        }
      }
    }
  }
}
