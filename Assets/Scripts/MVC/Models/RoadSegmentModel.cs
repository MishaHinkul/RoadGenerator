using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadSegment
{
  public RoadPoint GetOther(RoadPoint main)
  {
    return this.Begin.Equals(main) ? this.End : this.Begin;
  }

  public float SegmentLength()
  {
    return Vector2.Distance(this.Begin.Point, this.End.Point);
  }

  public Vector3 GetPoint(bool first)
  {
    if (first)
    {
      return new Vector3(this.Begin.Point.x, 0, this.Begin.Point.y);
    }
    return new Vector3(this.End.Point.x, 0, this.End.Point.y);
  }

  public void DebugDriwLine()
  {
    Debug.DrawLine(GetPoint(true), GetPoint(false), Color.blue);
    Debug.DrawRay(GetPoint(true), Vector3.up, Color.red);
    Debug.DrawRay(GetPoint(false), Vector3.up, Color.red);
  }

  public bool IsEqual(RoadSegment segment)
  {
    if (this.Begin.Equals(segment.Begin) && this.End.Equals(segment.End))
    {
      return true;
    }
    else if (this.Begin.Equals(segment.End) && this.End.Equals(segment.Begin))
    {
      return true;
    }
    return false;
  }


  public RoadSegment(RoadPoint a, RoadPoint b, int level)
  {
    Begin = new RoadPoint(a.Point, this);
    End = new RoadPoint(b.Point, this);
    Level = level;
  }


  public RoadPoint Begin { get; private set; }
  public RoadPoint End { get; private set; }
  public int Level { get; private set; }
}