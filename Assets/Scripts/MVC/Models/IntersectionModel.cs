using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Пересечения, которые и формируют перекрестки
/// </summary>
public class Intersection
{
  private List<RoadPoint> points = null; //список должен быть отсортирован в круговом порядке
 

  public bool IsThisOne(Intersection inter)
  {
    int c = 0;
    foreach (RoadPoint p in inter.Points)
    {
      if (points.Exists(f => f == p))
      {
        c++;
      }
    }
    if (c == points.Count && c == inter.Points.Count)
    {
      return true;
    }

    return false;
  }


  public Intersection(List<RoadPoint> points)
  {
    this.points = points;
  }

  public List<RoadPoint> Points
  {
    get
    {
      return points;
    }
  }
}