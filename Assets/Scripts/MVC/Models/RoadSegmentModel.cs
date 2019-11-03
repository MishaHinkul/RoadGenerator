using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadSegment : RoadItem
{
  public RoadPoint GetOther(RoadPoint main)
  {
    return this.Begin.Equals(main) ? this.End : this.Begin;
  }

  public float SegmentLength()
  {
    return Vector2.Distance(this.Begin.Point, this.End.Point);
  }

  public Vector3 GetWorldPosition(bool first)
  {
    if (first)
    {
      return Begin.WorldPosition;
    }
    return End.WorldPosition;
  }

  public Vector3 GetWorldPerp()
  {
    //Получили направление для новой точки (перепендикуляр)
    return Vector3.Cross(Begin.WorldPosition - End.WorldPosition, Vector3.down).normalized;// * (Random.Range (0f, 1f) < 0.5f ? -1 : 1);
  }

  public void DebugDriwLine()
  {
    Debug.DrawLine(GetWorldPosition(true), GetWorldPosition(false), Color.blue);
    Debug.DrawRay(GetWorldPosition(true), Vector3.up, Color.red);
    Debug.DrawRay(GetWorldPosition(false), Vector3.up, Color.red);
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

  public Vector2 GetForward()
  {
    return End.Point - Begin.Point;
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