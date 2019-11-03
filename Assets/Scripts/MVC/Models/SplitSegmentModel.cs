using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitSegmentModel
{
  public SplitSegmentModel(RoadSegment roadSegment)
  {
    Segment = roadSegment;
  }
  public SplitSegmentModel(RoadSegment roadSegment, System.Func<bool> callback) : this(roadSegment)
  {
    Callback = callback;
  }
  public System.Func<bool> Callback { get; set; }
  public RoadSegment Segment { get; set; }
}