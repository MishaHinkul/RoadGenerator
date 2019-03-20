using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitTimeModel
{
  public float Time { get; set; }
  public System.Action Callback { get; set; }
}