using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Определяет одну из частей дороги (начало или конец)
/// </summary>
public class RoadPoint
{
  private const float MIN_DISTANCE = 0.0001f;

  public override bool Equals(object other)
  {
    RoadPoint otherRoadPoint = other as RoadPoint;
    if (otherRoadPoint == null)
    {
      return false;
    }

    //Просто проверка на расстояние игнорируя сегмент
    if ((Point - otherRoadPoint.Point).sqrMagnitude < MIN_DISTANCE)
    {
      return true;
    }
    return false;
  }


  public RoadPoint()
  {
  }

  public RoadPoint(Vector2 point, RoadSegment segment = null)
  {
    Point = new Vector2(point.x, point.y);
    MySegement = segment;
  }

  public Vector2 Point { get; set; }
  public RoadSegment MySegement { get; set; } //родитель данной точки
  public Vector3 WorldPosition
  {
    get
    {
      return new Vector3(Point.x, 0, Point.y);
    }
  }
}