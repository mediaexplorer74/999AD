// Decompiled with JetBrains decompiler
// Type: System.Runtime.Serialization.Formatters.Binary.BinaryFormatter
// Assembly: System.Runtime.Serialization.Formatters, Version=4.0.3.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 59FEC222-A9B8-4506-99C2-FAFFD4E25D28
// Assembly location: C:\Users\Admin\Desktop\RE\1\System.Runtime.Serialization.Formatters.dll

using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


namespace System.Runtime.Serialization.Formatters.Binary1
{
  public sealed class BinaryFormatter : IFormatter
  {
    private static readonly Dictionary<Type, TypeInformation> s_typeNameCache = new Dictionary<Type, TypeInformation>();
    internal ISurrogateSelector _surrogates;
    internal StreamingContext _context;
    internal SerializationBinder _binder;
    internal FormatterTypeStyle _typeFormat = FormatterTypeStyle.TypesAlways;
    internal FormatterAssemblyStyle _assemblyFormat;
    internal TypeFilterLevel _securityLevel = TypeFilterLevel.Full;
    internal object[] _crossAppDomainArray;

    public FormatterTypeStyle TypeFormat
    {
      get => this._typeFormat;
      set => this._typeFormat = value;
    }

    public FormatterAssemblyStyle AssemblyFormat
    {
      get => this._assemblyFormat;
      set => this._assemblyFormat = value;
    }

    public TypeFilterLevel FilterLevel
    {
      get => this._securityLevel;
      set => this._securityLevel = value;
    }

    public ISurrogateSelector SurrogateSelector
    {
      get => this._surrogates;
      set => this._surrogates = value;
    }

    public SerializationBinder Binder
    {
      get => this._binder;
      set => this._binder = value;
    }

    public StreamingContext Context
    {
      get => this._context;
      set => this._context = value;
    }

    public BinaryFormatter()
      //: this((ISurrogateSelector) null, new StreamingContext(/*(StreamingContextStates) (int) byte.MaxValue)*/)
    {
    }

    public BinaryFormatter(ISurrogateSelector selector, StreamingContext context)
    {
      this._surrogates = selector;
      this._context = context;
    }

    public object Deserialize(Stream serializationStream)
    {
      return this.Deserialize(serializationStream, true);
    }

    internal object Deserialize(Stream serializationStream, bool check)
    {
      if (serializationStream == null)
        throw new ArgumentNullException(nameof (serializationStream));
      if (serializationStream.CanSeek && serializationStream.Length == 0L)
        throw new SerializationException(SR.Serialization_Stream);
      InternalFE formatterEnums = new InternalFE()
      {
        _typeFormat = this._typeFormat,
        _serializerTypeEnum = InternalSerializerTypeE.Binary,
        _assemblyFormat = this._assemblyFormat,
        _securityLevel = this._securityLevel
      };
      ObjectReader objectReader = new ObjectReader(serializationStream, this._surrogates, this._context, formatterEnums, this._binder)
      {
        _crossAppDomainArray = this._crossAppDomainArray
      };
      BinaryParser serParser = new BinaryParser(serializationStream, objectReader);
      return objectReader.Deserialize(serParser, check);
    }

    public void Serialize(Stream serializationStream, object graph)
    {
      this.Serialize(serializationStream, graph, true);
    }

    internal void Serialize(Stream serializationStream, object graph, bool check)
    {
      if (serializationStream == null)
        throw new ArgumentNullException(nameof (serializationStream));
      ObjectWriter objectWriter = new ObjectWriter(this._surrogates, this._context, new InternalFE()
      {
        _typeFormat = this._typeFormat,
        _serializerTypeEnum = InternalSerializerTypeE.Binary,
        _assemblyFormat = this._assemblyFormat
      }, this._binder);
      BinaryFormatterWriter serWriter = new BinaryFormatterWriter(serializationStream, objectWriter, this._typeFormat);
      objectWriter.Serialize(graph, serWriter, check);
      this._crossAppDomainArray = objectWriter._crossAppDomainArray;
    }

    internal static TypeInformation GetTypeInformation(Type type)
    {
      lock (BinaryFormatter.s_typeNameCache)
      {
        TypeInformation typeInformation;
        if (!BinaryFormatter.s_typeNameCache.TryGetValue(type, out typeInformation))
        {
          bool hasTypeForwardedFrom;
          string clrAssemblyName = FormatterServices.GetClrAssemblyName(type, out hasTypeForwardedFrom);
          typeInformation = new TypeInformation(FormatterServices.GetClrTypeFullName(type), clrAssemblyName, hasTypeForwardedFrom);
          BinaryFormatter.s_typeNameCache.Add(type, typeInformation);
        }
        return typeInformation;
      }
    }
  }
}
