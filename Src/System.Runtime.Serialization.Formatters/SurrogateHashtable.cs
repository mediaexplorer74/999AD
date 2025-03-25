// Decompiled with JetBrains decompiler
// Type: System.Runtime.Serialization.SurrogateHashtable
// Assembly: System.Runtime.Serialization.Formatters, Version=4.0.3.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 59FEC222-A9B8-4506-99C2-FAFFD4E25D28
// Assembly location: C:\Users\Admin\Desktop\RE\1\System.Runtime.Serialization.Formatters.dll

using System.Collections;


namespace System.Runtime.Serialization
{
  internal sealed class SurrogateHashtable : Hashtable
  {
    internal SurrogateHashtable(int size)
      : base(size)
    {
    }

    protected override bool KeyEquals(object key, object item)
    {
      SurrogateKey surrogateKey1 = (SurrogateKey) item;
      SurrogateKey surrogateKey2 = (SurrogateKey) key;
      if (surrogateKey2._type == surrogateKey1._type)
      {
                StreamingContextStates streamingContextStates = default;//surrogateKey2._context.State & surrogateKey1._context.State;
        StreamingContext context1 = surrogateKey1._context;
                StreamingContextStates state = default;//context1.State;
        if (streamingContextStates == state)
        {
          context1 = surrogateKey2._context;
        //object context2 = context1.Context;
        //context1 = surrogateKey1._context;
        //object context3 = context1.Context;
        return true;//context2 == context3;
        }
      }
      return false;
    }
  }
}
