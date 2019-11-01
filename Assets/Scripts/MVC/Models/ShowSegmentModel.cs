using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowSegmentnModel
{
  public ShowSegmentnModel()
  {
  }
  public ShowSegmentnModel(RoadSegment roadSegment)
  {
    Segment = roadSegment;
  }
  public ShowSegmentnModel(RoadSegment roadSegment, System.Func<bool> callback) : this(roadSegment)
  {
    Callback = callback;
  }

  public System.Func<bool> Callback { get; set; }
  public RoadSegment Segment { get; set; }
}