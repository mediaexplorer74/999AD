// Decompiled with JetBrains decompiler
// Type: System.Runtime.Serialization.FixupHolderList
// Assembly: System.Runtime.Serialization.Formatters, Version=4.0.3.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 59FEC222-A9B8-4506-99C2-FAFFD4E25D28
// Assembly location: C:\Users\Admin\Desktop\RE\1\System.Runtime.Serialization.Formatters.dll


namespace System.Runtime.Serialization
{
  internal sealed class FixupHolderList
  {
    internal const int InitialSize = 2;
    internal FixupHolder[] _values;
    internal int _count;

    internal FixupHolderList()
      : this(2)
    {
    }

    internal FixupHolderList(int startingSize)
    {
      this._count = 0;
      this._values = new FixupHolder[startingSize];
    }

    internal void Add(FixupHolder fixup)
    {
      if (this._count == this._values.Length)
        this.EnlargeArray();
      this._values[this._count++] = fixup;
    }

    private void EnlargeArray()
    {
      int length = this._values.Length * 2;
      if (length < 0)
        length = length != int.MaxValue ? int.MaxValue : throw new SerializationException(SR.Serialization_TooManyElements);
      FixupHolder[] destinationArray = new FixupHolder[length];
      Array.Copy((Array) this._values, 0, (Array) destinationArray, 0, this._count);
      this._values = destinationArray;
    }
  }
}
