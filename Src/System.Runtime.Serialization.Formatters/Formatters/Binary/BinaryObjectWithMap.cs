// Decompiled with JetBrains decompiler
// Type: System.Runtime.Serialization.Formatters.Binary.BinaryObjectWithMap
// Assembly: System.Runtime.Serialization.Formatters, Version=4.0.3.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 59FEC222-A9B8-4506-99C2-FAFFD4E25D28
// Assembly location: C:\Users\Admin\Desktop\RE\1\System.Runtime.Serialization.Formatters.dll


namespace System.Runtime.Serialization.Formatters.Binary
{
  internal sealed class BinaryObjectWithMap : IStreamable
  {
    internal BinaryHeaderEnum _binaryHeaderEnum;
    internal int _objectId;
    internal string _name;
    internal int _numMembers;
    internal string[] _memberNames;
    internal int _assemId;

    internal BinaryObjectWithMap()
    {
    }

    internal BinaryObjectWithMap(BinaryHeaderEnum binaryHeaderEnum)
    {
      this._binaryHeaderEnum = binaryHeaderEnum;
    }

    internal void Set(
      int objectId,
      string name,
      int numMembers,
      string[] memberNames,
      int assemId)
    {
      this._objectId = objectId;
      this._name = name;
      this._numMembers = numMembers;
      this._memberNames = memberNames;
      this._assemId = assemId;
      this._binaryHeaderEnum = assemId > 0 ? BinaryHeaderEnum.ObjectWithMapAssemId : BinaryHeaderEnum.ObjectWithMap;
    }

    public void Write(BinaryFormatterWriter output)
    {
      output.WriteByte((byte) this._binaryHeaderEnum);
      output.WriteInt32(this._objectId);
      output.WriteString(this._name);
      output.WriteInt32(this._numMembers);
      for (int index = 0; index < this._numMembers; ++index)
        output.WriteString(this._memberNames[index]);
      if (this._assemId <= 0)
        return;
      output.WriteInt32(this._assemId);
    }

    public void Read(BinaryParser input)
    {
      this._objectId = input.ReadInt32();
      this._name = input.ReadString();
      this._numMembers = input.ReadInt32();
      this._memberNames = new string[this._numMembers];
      for (int index = 0; index < this._numMembers; ++index)
        this._memberNames[index] = input.ReadString();
      if (this._binaryHeaderEnum != BinaryHeaderEnum.ObjectWithMapAssemId)
        return;
      this._assemId = input.ReadInt32();
    }
  }
}
