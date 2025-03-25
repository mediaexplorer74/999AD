// Decompiled with JetBrains decompiler
// Type: System.Runtime.Serialization.ObjectHolderListEnumerator
// Assembly: System.Runtime.Serialization.Formatters, Version=4.0.3.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 59FEC222-A9B8-4506-99C2-FAFFD4E25D28
// Assembly location: C:\Users\Admin\Desktop\RE\1\System.Runtime.Serialization.Formatters.dll


namespace System.Runtime.Serialization
{
  internal sealed class ObjectHolderListEnumerator
  {
    private readonly bool _isFixupEnumerator;
    private readonly ObjectHolderList _list;
    private readonly int _startingVersion;
    private int _currPos;

    internal ObjectHolderListEnumerator(ObjectHolderList list, bool isFixupEnumerator)
    {
      this._list = list;
      this._startingVersion = this._list.Version;
      this._currPos = -1;
      this._isFixupEnumerator = isFixupEnumerator;
    }

    internal bool MoveNext()
    {
      if (this._isFixupEnumerator)
      {
        do
          ;
        while (++this._currPos < this._list.Count && this._list._values[this._currPos].CompletelyFixed);
        return this._currPos != this._list.Count;
      }
      ++this._currPos;
      return this._currPos != this._list.Count;
    }

    internal ObjectHolder Current => this._list._values[this._currPos];
  }
}
