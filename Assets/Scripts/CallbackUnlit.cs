using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallbackUnlit
{
  private static Stack<FlagValue> flags = new Stack<FlagValue>();

  public class FlagValue
  {
    public bool Value { get; set; }
  }

  private static bool flag = false;


  public static FlagValue PushFlag()
  {
    FlagValue result = new FlagValue();
    flags.Push(result);

    return result;
  }

  public static void PopFlag()
  {
    flags.Pop();
  }

  public static bool PeekFlag()
  {
    FlagValue v = flags.Peek();
    return v.Value;
  }

  public static System.Func<bool> PeekFlagAnonym()
  {
    return delegate ()
    {
      FlagValue v = flags.Peek();
      return v.Value;
    };
  }

  public static bool PeekFlagTrue()
  {
    FlagValue v = flags.Peek();
    v.Value = true;
    return v.Value;
  }
  public static bool SetLastFlagFalse()
  {
    FlagValue v = flags.Peek();
    v.Value = false;
    return v.Value;
  }

  public static bool ReturnTrue()
  {
    return true;
  }

  public static bool ReturnFalse()
  {
    return false;
  }

  public static void Execute(System.Func<bool> callback)
  {
    if (callback != null)
    {
      callback();
    }
  }
}
