// Decompiled with JetBrains decompiler
// Type: System.Runtime.Serialization.Formatters.Binary.SerStack
// Assembly: System.Runtime.Serialization.Formatters, Version=4.0.3.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 59FEC222-A9B8-4506-99C2-FAFFD4E25D28
// Assembly location: C:\Users\Admin\Desktop\RE\1\System.Runtime.Serialization.Formatters.dll


namespace System.Runtime.Serialization.Formatters.Binary
{
  internal sealed class SerStack
  {
    internal object[] _objects = new object[5];
    internal string _stackId;
    internal int _top = -1;

    internal SerStack(string stackId) => this._stackId = stackId;

    internal void Push(object obj)
    {
      if (this._top == this._objects.Length - 1)
        this.IncreaseCapacity();
      this._objects[++this._top] = obj;
    }

    internal object Pop()
    {
      if (this._top < 0)
        return (object) null;
      object obj = this._objects[this._top];
      this._objects[this._top--] = (object) null;
      return obj;
    }

    internal void IncreaseCapacity()
    {
      object[] destinationArray = new object[this._objects.Length * 2];
      Array.Copy((Array) this._objects, 0, (Array) destinationArray, 0, this._objects.Length);
      this._objects = destinationArray;
    }

    internal object Peek() => this._top >= 0 ? this._objects[this._top] : (object) null;

    internal object PeekPeek() => this._top >= 1 ? this._objects[this._top - 1] : (object) null;

    internal bool IsEmpty() => this._top <= 0;
  }
}
