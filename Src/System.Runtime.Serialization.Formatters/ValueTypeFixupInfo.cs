// Decompiled with JetBrains decompiler
// Type: System.Runtime.Serialization.ValueTypeFixupInfo
// Assembly: System.Runtime.Serialization.Formatters, Version=4.0.3.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 59FEC222-A9B8-4506-99C2-FAFFD4E25D28
// Assembly location: C:\Users\Admin\Desktop\RE\1\System.Runtime.Serialization.Formatters.dll

using System.Reflection;


namespace System.Runtime.Serialization
{
  internal sealed class ValueTypeFixupInfo
  {
    private readonly long _containerID;
    private readonly FieldInfo _parentField;
    private readonly int[] _parentIndex;

    public ValueTypeFixupInfo(long containerID, FieldInfo member, int[] parentIndex)
    {
      if (FieldInfo.Equals(member, (FieldInfo) null) && parentIndex == null)
        throw new ArgumentException(SR.Argument_MustSupplyParent);
      if (containerID == 0L && FieldInfo.Equals(member, (FieldInfo) null))
      {
        this._containerID = containerID;
        this._parentField = member;
        this._parentIndex = parentIndex;
      }
      if ((member != null))
      {
        if (parentIndex != null)
          throw new ArgumentException(SR.Argument_MemberAndArray);
        if (member.FieldType.GetTypeInfo().IsValueType && containerID == 0L)
          throw new ArgumentException(SR.Argument_MustSupplyContainer);
      }
      this._containerID = containerID;
      this._parentField = member;
      this._parentIndex = parentIndex;
    }

    public long ContainerID => this._containerID;

    public FieldInfo ParentField => this._parentField;

    public int[] ParentIndex => this._parentIndex;
  }
}
