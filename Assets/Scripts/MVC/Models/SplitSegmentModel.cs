using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitSegmentModel
{
  public System.Func<bool> Callback { get; set; }
  public RoadSegment Segment { get; set; }
}
