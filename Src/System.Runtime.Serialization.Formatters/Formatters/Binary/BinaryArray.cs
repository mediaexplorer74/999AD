// Decompiled with JetBrains decompiler
// Type: System.Runtime.Serialization.Formatters.Binary.BinaryArray
// Assembly: System.Runtime.Serialization.Formatters, Version=4.0.3.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 59FEC222-A9B8-4506-99C2-FAFFD4E25D28
// Assembly location: C:\Users\Admin\Desktop\RE\1\System.Runtime.Serialization.Formatters.dll


namespace System.Runtime.Serialization.Formatters.Binary
{
  internal sealed class BinaryArray : IStreamable
  {
    internal int _objectId;
    internal int _rank;
    internal int[] _lengthA;
    internal int[] _lowerBoundA;
    internal BinaryTypeEnum _binaryTypeEnum;
    internal object _typeInformation;
    internal int _assemId;
    private BinaryHeaderEnum _binaryHeaderEnum;
    internal BinaryArrayTypeEnum _binaryArrayTypeEnum;

    internal BinaryArray()
    {
    }

    internal BinaryArray(BinaryHeaderEnum binaryHeaderEnum)
    {
      this._binaryHeaderEnum = binaryHeaderEnum;
    }

    internal void Set(
      int objectId,
      int rank,
      int[] lengthA,
      int[] lowerBoundA,
      BinaryTypeEnum binaryTypeEnum,
      object typeInformation,
      BinaryArrayTypeEnum binaryArrayTypeEnum,
      int assemId)
    {
      this._objectId = objectId;
      this._binaryArrayTypeEnum = binaryArrayTypeEnum;
      this._rank = rank;
      this._lengthA = lengthA;
      this._lowerBoundA = lowerBoundA;
      this._binaryTypeEnum = binaryTypeEnum;
      this._typeInformation = typeInformation;
      this._assemId = assemId;
      this._binaryHeaderEnum = BinaryHeaderEnum.Array;
      if (binaryArrayTypeEnum != BinaryArrayTypeEnum.Single)
        return;
      switch (binaryTypeEnum)
      {
        case BinaryTypeEnum.Primitive:
          this._binaryHeaderEnum = BinaryHeaderEnum.ArraySinglePrimitive;
          break;
        case BinaryTypeEnum.String:
          this._binaryHeaderEnum = BinaryHeaderEnum.ArraySingleString;
          break;
        case BinaryTypeEnum.Object:
          this._binaryHeaderEnum = BinaryHeaderEnum.ArraySingleObject;
          break;
      }
    }

    public void Write(BinaryFormatterWriter output)
    {
      switch (this._binaryHeaderEnum)
      {
        case BinaryHeaderEnum.ArraySinglePrimitive:
          output.WriteByte((byte) this._binaryHeaderEnum);
          output.WriteInt32(this._objectId);
          output.WriteInt32(this._lengthA[0]);
          output.WriteByte((byte) (InternalPrimitiveTypeE) this._typeInformation);
          break;
        case BinaryHeaderEnum.ArraySingleObject:
          output.WriteByte((byte) this._binaryHeaderEnum);
          output.WriteInt32(this._objectId);
          output.WriteInt32(this._lengthA[0]);
          break;
        case BinaryHeaderEnum.ArraySingleString:
          output.WriteByte((byte) this._binaryHeaderEnum);
          output.WriteInt32(this._objectId);
          output.WriteInt32(this._lengthA[0]);
          break;
        default:
          output.WriteByte((byte) this._binaryHeaderEnum);
          output.WriteInt32(this._objectId);
          output.WriteByte((byte) this._binaryArrayTypeEnum);
          output.WriteInt32(this._rank);
          for (int index = 0; index < this._rank; ++index)
            output.WriteInt32(this._lengthA[index]);
          if (this._binaryArrayTypeEnum == BinaryArrayTypeEnum.SingleOffset || this._binaryArrayTypeEnum == BinaryArrayTypeEnum.JaggedOffset || this._binaryArrayTypeEnum == BinaryArrayTypeEnum.RectangularOffset)
          {
            for (int index = 0; index < this._rank; ++index)
              output.WriteInt32(this._lowerBoundA[index]);
          }
          output.WriteByte((byte) this._binaryTypeEnum);
          BinaryTypeConverter.WriteTypeInfo(this._binaryTypeEnum, this._typeInformation, this._assemId, output);
          break;
      }
    }

    public void Read(BinaryParser input)
    {
      switch (this._binaryHeaderEnum)
      {
        case BinaryHeaderEnum.ArraySinglePrimitive:
          this._objectId = input.ReadInt32();
          this._lengthA = new int[1];
          this._lengthA[0] = input.ReadInt32();
          this._binaryArrayTypeEnum = BinaryArrayTypeEnum.Single;
          this._rank = 1;
          this._lowerBoundA = new int[this._rank];
          this._binaryTypeEnum = BinaryTypeEnum.Primitive;
          this._typeInformation = (object) (InternalPrimitiveTypeE) input.ReadByte();
          break;
        case BinaryHeaderEnum.ArraySingleObject:
          this._objectId = input.ReadInt32();
          this._lengthA = new int[1];
          this._lengthA[0] = input.ReadInt32();
          this._binaryArrayTypeEnum = BinaryArrayTypeEnum.Single;
          this._rank = 1;
          this._lowerBoundA = new int[this._rank];
          this._binaryTypeEnum = BinaryTypeEnum.Object;
          this._typeInformation = (object) null;
          break;
        case BinaryHeaderEnum.ArraySingleString:
          this._objectId = input.ReadInt32();
          this._lengthA = new int[1];
          this._lengthA[0] = input.ReadInt32();
          this._binaryArrayTypeEnum = BinaryArrayTypeEnum.Single;
          this._rank = 1;
          this._lowerBoundA = new int[this._rank];
          this._binaryTypeEnum = BinaryTypeEnum.String;
          this._typeInformation = (object) null;
          break;
        default:
          this._objectId = input.ReadInt32();
          this._binaryArrayTypeEnum = (BinaryArrayTypeEnum) input.ReadByte();
          this._rank = input.ReadInt32();
          this._lengthA = new int[this._rank];
          this._lowerBoundA = new int[this._rank];
          for (int index = 0; index < this._rank; ++index)
            this._lengthA[index] = input.ReadInt32();
          if (this._binaryArrayTypeEnum == BinaryArrayTypeEnum.SingleOffset || this._binaryArrayTypeEnum == BinaryArrayTypeEnum.JaggedOffset || this._binaryArrayTypeEnum == BinaryArrayTypeEnum.RectangularOffset)
          {
            for (int index = 0; index < this._rank; ++index)
              this._lowerBoundA[index] = input.ReadInt32();
          }
          this._binaryTypeEnum = (BinaryTypeEnum) input.ReadByte();
          this._typeInformation = BinaryTypeConverter.ReadTypeInfo(this._binaryTypeEnum, input, out this._assemId);
          break;
      }
    }
  }
}
