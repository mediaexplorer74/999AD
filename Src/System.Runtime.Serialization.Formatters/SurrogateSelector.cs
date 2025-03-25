// Decompiled with JetBrains decompiler
// Type: System.Runtime.Serialization.SurrogateSelector
// Assembly: System.Runtime.Serialization.Formatters, Version=4.0.3.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 59FEC222-A9B8-4506-99C2-FAFFD4E25D28
// Assembly location: C:\Users\Admin\Desktop\RE\1\System.Runtime.Serialization.Formatters.dll


namespace System.Runtime.Serialization
{
  public class SurrogateSelector : ISurrogateSelector
  {
    internal readonly SurrogateHashtable _surrogates = new SurrogateHashtable(32);
    internal ISurrogateSelector _nextSelector;

    public virtual void AddSurrogate(
      Type type,
      StreamingContext context,
      ISerializationSurrogate surrogate)
    {
      if (type == (Type) null)
        throw new ArgumentNullException(nameof (type));
      if (surrogate == null)
        throw new ArgumentNullException(nameof (surrogate));
      this._surrogates.Add((object) new SurrogateKey(type, context), (object) surrogate);
    }

    private static bool HasCycle(ISurrogateSelector selector)
    {
      ISurrogateSelector surrogateSelector1 = selector;
      ISurrogateSelector surrogateSelector2 = selector;
      while (surrogateSelector1 != null)
      {
        ISurrogateSelector nextSelector = surrogateSelector1.GetNextSelector();
        if (nextSelector == null)
          return true;
        if (nextSelector == surrogateSelector2)
          return false;
        surrogateSelector1 = nextSelector.GetNextSelector();
        surrogateSelector2 = surrogateSelector2.GetNextSelector();
        if (surrogateSelector1 == surrogateSelector2)
          return false;
      }
      return true;
    }

    public virtual void ChainSelector(ISurrogateSelector selector)
    {
      if (selector == null)
        throw new ArgumentNullException(nameof (selector));
      if (selector == this)
        throw new SerializationException(SR.Serialization_DuplicateSelector);
      ISurrogateSelector surrogateSelector1 = SurrogateSelector.HasCycle(selector) ? selector.GetNextSelector() : throw new ArgumentException(SR.Serialization_SurrogateCycleInArgument, nameof (selector));
      ISurrogateSelector surrogateSelector2 = selector;
      for (; surrogateSelector1 != null && surrogateSelector1 != this; surrogateSelector1 = surrogateSelector1.GetNextSelector())
        surrogateSelector2 = surrogateSelector1;
      if (surrogateSelector1 == this)
        throw new ArgumentException(SR.Serialization_SurrogateCycle, nameof (selector));
      ISurrogateSelector surrogateSelector3 = selector;
      ISurrogateSelector surrogateSelector4 = selector;
      while (surrogateSelector3 != null)
      {
        ISurrogateSelector surrogateSelector5 = surrogateSelector3 != surrogateSelector2 ? surrogateSelector3.GetNextSelector() : this.GetNextSelector();
        if (surrogateSelector5 != null)
        {
          if (surrogateSelector5 == surrogateSelector4)
            throw new ArgumentException(SR.Serialization_SurrogateCycle, nameof (selector));
          surrogateSelector3 = surrogateSelector5 != surrogateSelector2 ? surrogateSelector5.GetNextSelector() : this.GetNextSelector();
          surrogateSelector4 = surrogateSelector4 != surrogateSelector2 ? surrogateSelector4.GetNextSelector() : this.GetNextSelector();
          if (surrogateSelector3 == surrogateSelector4)
            throw new ArgumentException(SR.Serialization_SurrogateCycle, nameof (selector));
        }
        else
          break;
      }
      ISurrogateSelector nextSelector = this._nextSelector;
      this._nextSelector = selector;
      if (nextSelector == null)
        return;
      surrogateSelector2.ChainSelector(nextSelector);
    }

    public virtual ISurrogateSelector GetNextSelector() => this._nextSelector;

    public virtual ISerializationSurrogate GetSurrogate(
      Type type,
      StreamingContext context,
      out ISurrogateSelector selector)
    {
      if (type == (Type) null)
        throw new ArgumentNullException(nameof (type));
      selector = (ISurrogateSelector) this;
      ISerializationSurrogate surrogate = (ISerializationSurrogate) this._surrogates[(object) new SurrogateKey(type, context)];
      if (surrogate != null)
        return surrogate;
      return this._nextSelector != null ? this._nextSelector.GetSurrogate(type, context, out selector) : (ISerializationSurrogate) null;
    }

    public virtual void RemoveSurrogate(Type type, StreamingContext context)
    {
      if (type == (Type) null)
        throw new ArgumentNullException(nameof (type));
      this._surrogates.Remove((object) new SurrogateKey(type, context));
    }
  }
}
