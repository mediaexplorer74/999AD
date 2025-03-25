// Decompiled with JetBrains decompiler
// Type: System.Runtime.Serialization.LongList
// Assembly: System.Runtime.Serialization.Formatters, Version=4.0.3.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 59FEC222-A9B8-4506-99C2-FAFFD4E25D28
// Assembly location: C:\Users\Admin\Desktop\RE\1\System.Runtime.Serialization.Formatters.dll


namespace System.Runtime.Serialization
{
  internal sealed class LongList
  {
    private const int InitialSize = 2;
    private long[] _values;
    private int _count;
    private int _totalItems;
    private int _currentItem;

    internal LongList()
      : this(2)
    {
    }

    internal LongList(int startingSize)
    {
      this._count = 0;
      this._totalItems = 0;
      this._values = new long[startingSize];
    }

    internal void Add(long value)
    {
      if (this._totalItems == this._values.Length)
        this.EnlargeArray();
      this._values[this._totalItems++] = value;
      ++this._count;
    }

    internal int Count => this._count;

    internal void StartEnumeration() => this._currentItem = -1;

    internal bool MoveNext()
    {
      do
        ;
      while (++this._currentItem < this._totalItems && this._values[this._currentItem] == -1L);
      return this._currentItem != this._totalItems;
    }

    internal long Current => this._values[this._currentItem];

    internal bool RemoveElement(long value)
    {
      int index = 0;
      while (index < this._totalItems && this._values[index] != value)
        ++index;
      if (index == this._totalItems)
        return false;
      this._values[index] = -1L;
      return true;
    }

    private void EnlargeArray()
    {
      int length = this._values.Length * 2;
      if (length < 0)
        length = length != int.MaxValue ? int.MaxValue : throw new SerializationException(SR.Serialization_TooManyElements);
      long[] destinationArray = new long[length];
      Array.Copy((Array) this._values, 0, (Array) destinationArray, 0, this._count);
      this._values = destinationArray;
    }
  }
}
