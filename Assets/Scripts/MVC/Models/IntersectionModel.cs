using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Пересечения, которые и формируют перекрестки
/// </summary>
public class Intersection
{
    public List<RoadPoint> Points; //список должен быть отсортирован в круговом порядке

    public Intersection(List<RoadPoint> points)
    {
        this.Points = points;
    }
    public bool IsThisOne(Intersection inter)
    {
        int c = 0;
        foreach (RoadPoint p in inter.Points)
        {
            if (this.Points.Exists(f => f == p))
            {
                c++;
            }
        }
        if (c == this.Points.Count && c == inter.Points.Count)
        {
            return true;
        }
        return false;
    }
}
