using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Определяет одну из частей дороги (начало или конец)
/// </summary>
public class RoadPoint
{
    public Vector2 point { get; set; }
    public RoadSegment mySegement { get; set; } //родитель данной точки

    public RoadPoint()
    {
    }

    public RoadPoint(Vector2 point, RoadSegment segment = null)
    {
        this.point = new Vector2(point.x, point.y);
        this.mySegement = segment;
    }

    public override bool Equals(object other)
    {
        //Просто проверка на расстояние игнорируя сегмент
        if (Vector2.Distance((other as RoadPoint).point, this.point) < 0.01f)
        {
            return true;
        }
        return false;
    }
}