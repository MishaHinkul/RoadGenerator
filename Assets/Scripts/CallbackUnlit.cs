using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallbackUnlit
{
  public static bool Unlit()
  {
    return true;
  }

  public static void Execute(System.Func<bool> callback)
  {
    if (callback != null)
    {
      callback();
    }
  }
}
