using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Сегмент дороги состоит из 2 точек. Точка начала и точка конца дороги
/// </summary>
public class RoadSegment
{
    public RoadPoint PointA { get; private set; }
    public RoadPoint PointB { get; private set; }

    public int Level { get; private set; }

    public RoadSegment(RoadPoint a, RoadPoint b, int level)
    {
        this.PointA = new RoadPoint(a.point, this);
        this.PointB = new RoadPoint(b.point, this);

        this.Level = level;
    }

    /// <summary>
    /// Получить на основе одной точки - другую
    /// </summary>
    /// <param name="main"></param>
    /// <returns></returns>
	public RoadPoint GetOther(RoadPoint main)
    {
        return this.PointA.Equals(main) ? this.PointB : this.PointA;
    }

    public float SegmentLength()
    {
        return Vector2.Distance(this.PointA.point, this.PointB.point);
    }

    /// <summary>
    /// Конвертация Vector2 в Vector3
    /// </summary>
    /// <param name="first">вернуть первую или вторую точку</param>
    /// <returns></returns>
    public Vector3 GetVector3(bool first)
    {
        if (first)
            return new Vector3(this.PointA.point.x, 0, this.PointA.point.y);
        else
            return new Vector3(this.PointB.point.x, 0, this.PointB.point.y);
    }

    public void DebugDriwLine()
    {
        Debug.DrawLine(GetVector3(true), GetVector3(false), Color.blue);
        Debug.DrawRay(GetVector3(true), Vector3.up, Color.red);
        Debug.DrawRay(GetVector3(false), Vector3.up, Color.red);
    }

    /// <summary>
    /// Определяет, равен ли этот экземпляр указанному сегменту
    /// </summary>
    /// <returns><c>true</c> if this instance is equal the specified segment; otherwise, <c>false</c>.</returns>
    /// <param name="segment">Segment.</param>
    public bool IsEqual(RoadSegment segment)
    {
        if (this.PointA.Equals(segment.PointA) && this.PointB.Equals(segment.PointB))
        {
            return true;
        }
        else if (this.PointA.Equals(segment.PointB) && this.PointB.Equals(segment.PointA))
        {
            return true;
        }
        return false;
    }
}