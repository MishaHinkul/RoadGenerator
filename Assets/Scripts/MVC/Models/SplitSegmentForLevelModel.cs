using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitSegmentForLevelModel
{
  public SplitSegmentForLevelModel(int level)
  {
    Level = level;
  }

  public SplitSegmentForLevelModel(int level, System.Func<bool> callback) : this(level)
  {
    Callback = callback;
  }

  public System.Func<bool> Callback { get; set; }
  public int Level { get; set; }
}
