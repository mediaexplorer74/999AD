// Decompiled with JetBrains decompiler
// Type: System.SR
// Assembly: System.Runtime.Serialization.Formatters, Version=4.0.3.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 59FEC222-A9B8-4506-99C2-FAFFD4E25D28
// Assembly location: C:\Users\Admin\Desktop\RE\1\System.Runtime.Serialization.Formatters.dll

using System.Resources;
using System.Runtime.CompilerServices;


namespace System
{
  internal static class SR
  {
    private static ResourceManager s_resourceManager;
    private const string s_resourcesName = "FxResources.System.Runtime.Serialization.Formatters.SR";

    private static ResourceManager ResourceManager
    {
      get => SR.s_resourceManager ?? (SR.s_resourceManager = new ResourceManager(SR.ResourceType));
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static bool UsingResourceKeys() => false;

    internal static string GetResourceString(string resourceKey, string defaultString)
    {
      string str = (string) null;
      try
      {
        str = SR.ResourceManager.GetString(resourceKey);
      }
      catch (MissingManifestResourceException ex)
      {
      }
      return defaultString != null && resourceKey.Equals(str, StringComparison.Ordinal) ? defaultString : str;
    }

    internal static string Format(string resourceFormat, params object[] args)
    {
      if (args == null)
        return resourceFormat;
      return SR.UsingResourceKeys() ? resourceFormat + string.Join(", ", args) : string.Format(resourceFormat, args);
    }

    internal static string Format(string resourceFormat, object p1)
    {
      if (!SR.UsingResourceKeys())
        return string.Format(resourceFormat, p1);
      return string.Join(", ", (object) resourceFormat, p1);
    }

    internal static string Format(string resourceFormat, object p1, object p2)
    {
      if (!SR.UsingResourceKeys())
        return string.Format(resourceFormat, p1, p2);
      return string.Join(", ", (object) resourceFormat, p1, p2);
    }

    internal static string Format(string resourceFormat, object p1, object p2, object p3)
    {
      if (!SR.UsingResourceKeys())
        return string.Format(resourceFormat, p1, p2, p3);
      return string.Join(", ", (object) resourceFormat, p1, p2, p3);
    }

    internal static string Serialization_NonSerType
    {
      get => SR.GetResourceString(nameof (Serialization_NonSerType), (string) null);
    }

    internal static string Argument_DataLengthDifferent
    {
      get => SR.GetResourceString(nameof (Argument_DataLengthDifferent), (string) null);
    }

    internal static string ArgumentNull_NullMember
    {
      get => SR.GetResourceString(nameof (ArgumentNull_NullMember), (string) null);
    }

    internal static string Serialization_UnknownMemberInfo
    {
      get => SR.GetResourceString(nameof (Serialization_UnknownMemberInfo), (string) null);
    }

    internal static string Serialization_NoID
    {
      get => SR.GetResourceString(nameof (Serialization_NoID), (string) null);
    }

    internal static string Serialization_TooManyElements
    {
      get => SR.GetResourceString(nameof (Serialization_TooManyElements), (string) null);
    }

    internal static string Argument_InvalidFieldInfo
    {
      get => SR.GetResourceString(nameof (Argument_InvalidFieldInfo), (string) null);
    }

    internal static string Serialization_NeverSeen
    {
      get => SR.GetResourceString(nameof (Serialization_NeverSeen), (string) null);
    }

    internal static string Serialization_IORIncomplete
    {
      get => SR.GetResourceString(nameof (Serialization_IORIncomplete), (string) null);
    }

    internal static string Serialization_ObjectNotSupplied
    {
      get => SR.GetResourceString(nameof (Serialization_ObjectNotSupplied), (string) null);
    }

    internal static string Serialization_NotCyclicallyReferenceableSurrogate
    {
      get
      {
        return SR.GetResourceString(nameof (Serialization_NotCyclicallyReferenceableSurrogate), (string) null);
      }
    }

    internal static string Serialization_TooManyReferences
    {
      get => SR.GetResourceString(nameof (Serialization_TooManyReferences), (string) null);
    }

    internal static string Serialization_MissingObject
    {
      get => SR.GetResourceString(nameof (Serialization_MissingObject), (string) null);
    }

    internal static string Serialization_InvalidFixupDiscovered
    {
      get => SR.GetResourceString(nameof (Serialization_InvalidFixupDiscovered), (string) null);
    }

    internal static string Serialization_TypeLoadFailure
    {
      get => SR.GetResourceString(nameof (Serialization_TypeLoadFailure), (string) null);
    }

    internal static string Serialization_ValueTypeFixup
    {
      get => SR.GetResourceString(nameof (Serialization_ValueTypeFixup), (string) null);
    }

    internal static string Serialization_PartialValueTypeFixup
    {
      get => SR.GetResourceString(nameof (Serialization_PartialValueTypeFixup), (string) null);
    }

    internal static string Serialization_UnableToFixup
    {
      get => SR.GetResourceString(nameof (Serialization_UnableToFixup), (string) null);
    }

    internal static string ArgumentOutOfRange_ObjectID
    {
      get => SR.GetResourceString(nameof (ArgumentOutOfRange_ObjectID), (string) null);
    }

    internal static string Serialization_RegisterTwice
    {
      get => SR.GetResourceString(nameof (Serialization_RegisterTwice), (string) null);
    }

    internal static string Serialization_NotISer
    {
      get => SR.GetResourceString(nameof (Serialization_NotISer), (string) null);
    }

    internal static string Serialization_ConstructorNotFound
    {
      get => SR.GetResourceString(nameof (Serialization_ConstructorNotFound), (string) null);
    }

    internal static string Serialization_IncorrectNumberOfFixups
    {
      get => SR.GetResourceString(nameof (Serialization_IncorrectNumberOfFixups), (string) null);
    }

    internal static string Serialization_InvalidFixupType
    {
      get => SR.GetResourceString(nameof (Serialization_InvalidFixupType), (string) null);
    }

    internal static string Serialization_IdTooSmall
    {
      get => SR.GetResourceString(nameof (Serialization_IdTooSmall), (string) null);
    }

    internal static string Serialization_ParentChildIdentical
    {
      get => SR.GetResourceString(nameof (Serialization_ParentChildIdentical), (string) null);
    }

    internal static string Serialization_InvalidType
    {
      get => SR.GetResourceString(nameof (Serialization_InvalidType), (string) null);
    }

    internal static string Argument_MustSupplyParent
    {
      get => SR.GetResourceString(nameof (Argument_MustSupplyParent), (string) null);
    }

    internal static string Argument_MemberAndArray
    {
      get => SR.GetResourceString(nameof (Argument_MemberAndArray), (string) null);
    }

    internal static string Serialization_CorruptedStream
    {
      get => SR.GetResourceString(nameof (Serialization_CorruptedStream), (string) null);
    }

    internal static string Serialization_Stream
    {
      get => SR.GetResourceString(nameof (Serialization_Stream), (string) null);
    }

    internal static string Serialization_BinaryHeader
    {
      get => SR.GetResourceString(nameof (Serialization_BinaryHeader), (string) null);
    }

    internal static string Serialization_TypeExpected
    {
      get => SR.GetResourceString(nameof (Serialization_TypeExpected), (string) null);
    }

    internal static string Serialization_StreamEnd
    {
      get => SR.GetResourceString(nameof (Serialization_StreamEnd), (string) null);
    }

    internal static string Serialization_CrossAppDomainError
    {
      get => SR.GetResourceString(nameof (Serialization_CrossAppDomainError), (string) null);
    }

    internal static string Serialization_Map
    {
      get => SR.GetResourceString(nameof (Serialization_Map), (string) null);
    }

    internal static string Serialization_Assembly
    {
      get => SR.GetResourceString(nameof (Serialization_Assembly), (string) null);
    }

    internal static string Serialization_ObjectTypeEnum
    {
      get => SR.GetResourceString(nameof (Serialization_ObjectTypeEnum), (string) null);
    }

    internal static string Serialization_AssemblyId
    {
      get => SR.GetResourceString(nameof (Serialization_AssemblyId), (string) null);
    }

    internal static string Serialization_ArrayType
    {
      get => SR.GetResourceString(nameof (Serialization_ArrayType), (string) null);
    }

    internal static string Serialization_TypeCode
    {
      get => SR.GetResourceString(nameof (Serialization_TypeCode), (string) null);
    }

    internal static string Serialization_TypeWrite
    {
      get => SR.GetResourceString(nameof (Serialization_TypeWrite), (string) null);
    }

    internal static string Serialization_TypeRead
    {
      get => SR.GetResourceString(nameof (Serialization_TypeRead), (string) null);
    }

    internal static string Serialization_AssemblyNotFound
    {
      get => SR.GetResourceString(nameof (Serialization_AssemblyNotFound), (string) null);
    }

    internal static string Serialization_InvalidFormat
    {
      get => SR.GetResourceString(nameof (Serialization_InvalidFormat), (string) null);
    }

    internal static string Serialization_TopObject
    {
      get => SR.GetResourceString(nameof (Serialization_TopObject), (string) null);
    }

    internal static string Serialization_XMLElement
    {
      get => SR.GetResourceString(nameof (Serialization_XMLElement), (string) null);
    }

    internal static string Serialization_TopObjectInstantiate
    {
      get => SR.GetResourceString(nameof (Serialization_TopObjectInstantiate), (string) null);
    }

    internal static string Serialization_ArrayTypeObject
    {
      get => SR.GetResourceString(nameof (Serialization_ArrayTypeObject), (string) null);
    }

    internal static string Serialization_TypeMissing
    {
      get => SR.GetResourceString(nameof (Serialization_TypeMissing), (string) null);
    }

    internal static string Serialization_ObjNoID
    {
      get => SR.GetResourceString(nameof (Serialization_ObjNoID), (string) null);
    }

    internal static string Serialization_SerMemberInfo
    {
      get => SR.GetResourceString(nameof (Serialization_SerMemberInfo), (string) null);
    }

    internal static string Argument_MustSupplyContainer
    {
      get => SR.GetResourceString(nameof (Argument_MustSupplyContainer), (string) null);
    }

    internal static string Serialization_ParseError
    {
      get => SR.GetResourceString(nameof (Serialization_ParseError), (string) null);
    }

    internal static string Serialization_ISerializableMemberInfo
    {
      get => SR.GetResourceString(nameof (Serialization_ISerializableMemberInfo), (string) null);
    }

    internal static string Serialization_MemberInfo
    {
      get => SR.GetResourceString(nameof (Serialization_MemberInfo), (string) null);
    }

    internal static string Serialization_ISerializableTypes
    {
      get => SR.GetResourceString(nameof (Serialization_ISerializableTypes), (string) null);
    }

    internal static string Serialization_MissingMember
    {
      get => SR.GetResourceString(nameof (Serialization_MissingMember), (string) null);
    }

    internal static string Serialization_NoMemberInfo
    {
      get => SR.GetResourceString(nameof (Serialization_NoMemberInfo), (string) null);
    }

    internal static string Serialization_DuplicateSelector
    {
      get => SR.GetResourceString(nameof (Serialization_DuplicateSelector), (string) null);
    }

    internal static string Serialization_SurrogateCycleInArgument
    {
      get => SR.GetResourceString(nameof (Serialization_SurrogateCycleInArgument), (string) null);
    }

    internal static string Serialization_SurrogateCycle
    {
      get => SR.GetResourceString(nameof (Serialization_SurrogateCycle), (string) null);
    }

    internal static string IO_EOF_ReadBeyondEOF
    {
      get => SR.GetResourceString(nameof (IO_EOF_ReadBeyondEOF), (string) null);
    }

    internal static Type ResourceType => typeof (FxResources.System.Runtime.Serialization.Formatters.SR);
  }
}
