using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadNetworkModel
{
  public enum KindIntersection
  {
    Non,
    Four,
    T
  }
  public RoadNetworkModel() 
  {
    RoadIntersections = new List<Intersection>();
    RoadSegments = new List<RoadSegment>();
    ViewObjects = new Dictionary<HVector3, GameObject>();
    ShortCutOff = 5f;
    CloseCutoff = 7f;
  }

  public KindIntersection GetKindIntersection(Intersection intersection)
  {
    if (intersection == null)
    {
      return KindIntersection.Non;
    }

    switch (intersection.Points.Count)
    {
      case 3:
        return KindIntersection.T;
      case 4:
        return KindIntersection.Four;
      default:
        return KindIntersection.Non;
    }
  }
  public Intersection WorldPositionToIntersection(Vector3 pos)
  {
    Vector3 intersectionPos;
    for (int i = 0; i < RoadIntersections.Count; i++)
    {
      intersectionPos = GetWorldPositionIntersection(RoadIntersections[i]);
      if (intersectionPos.Equals(pos))
      {
        return RoadIntersections[i];
      }
    }

    return null;
  }

  public Vector3 GetWorldPositionIntersection(Intersection intersection)
  {
    RoadPoint roadPointA = intersection.Points[0]; ;
    return new Vector3(roadPointA.Point.x, RoadIntersectionTransform.position.y, roadPointA.Point.y);
  }

  public Quaternion LookRotationWorldIntersection(Intersection intersection)
  {
    switch (GetKindIntersection(intersection))
    {
      case KindIntersection.Four:
        return LookRotationWorldForPoint(intersection.Points[0]);
      case KindIntersection.T:
        return LookRotationWorldTIntersection(intersection.Points);
      default:
        return Quaternion.identity;
    }
  }

  private Quaternion LookRotationWorldTIntersection(List<RoadPoint> intersectionList)
  {
    RoadPoint segment1PointA = null;
    RoadPoint segment1PointB = null;
    RoadPoint segment2PointB = null;
    RoadPoint segment2PointA = null;
    Vector2 vectorSegment1;
    Vector2 vectorSegment2;
    int indexDirectionPoint = 0; // точка относительно которой будем расчитывать вращение

    for (int j = 0; j < intersectionList.Count - 1; j++)
    {
      segment1PointA = intersectionList[j];
      segment1PointB = segment1PointA.MySegement.GetOther(segment1PointA);

      segment2PointA = intersectionList[j + 1];
      segment2PointB = segment2PointA.MySegement.GetOther(segment2PointA);

      //2 вектора должны смотреть в разные стороны
      vectorSegment1 = segment1PointB.Point - segment1PointA.Point;
      vectorSegment2 = segment2PointA.Point - segment2PointB.Point;

      //Если между двумя векторами угл 180 градусов, значит относительно 3 точки мы поворачиваем правильно префаб перекрестка
      if (Vector2.Dot(vectorSegment1.normalized, vectorSegment2.normalized) == 1)
      {
        indexDirectionPoint = j - 1;

        if (indexDirectionPoint < 0)
        {
          indexDirectionPoint = intersectionList.Count - 1;
          break;
        }
      }
    }
    return LookRotationWorldForPoint(intersectionList[indexDirectionPoint]);
  }

  /// <summary>
  /// Получить вращение по направлению сегмента
  /// </summary>
  /// <param name="roadPoint"></param>
  /// <returns></returns>
  private Quaternion LookRotationWorldForPoint(RoadPoint roadPoint)
  {
    RoadPoint roadPointA = roadPoint;
    RoadPoint roadPointB = roadPointA.MySegement.GetOther(roadPointA);

    Vector2 direction = roadPointB.Point - roadPointA.Point;
    Vector3 forward = new Vector3(direction.x, RoadIntersectionTransform.position.y, direction.y);

    return Quaternion.LookRotation(forward);
  }


  #region Segment
  public Vector3 GetWorldForwadSegment(RoadSegment roadSegment)
  {
    Vector2 local = roadSegment.GetForward();
    Vector3 forward = new Vector3(local.x, RoadIntersectionTransform.position.y, local.y);
    return forward.normalized;
  }

  public Vector3 GetWorldPositionBeginSegment(RoadSegment roadSegment)
  {
    return GetWorldPositionPoint(roadSegment.Begin);
  }

  public Vector3 GetWorldPositionEndSegment(RoadSegment roadSegment)
  {
    return GetWorldPositionPoint(roadSegment.End);
  }

  public Vector3 GetWorldPositionPoint(RoadPoint point)
  {
    return new Vector3(point.Point.x,
                      RoadIntersectionTransform.position.y,
                      point.Point.y);
  }

  public float GetWithSegment(RoadSegment roadSegment)
  {
    return roadSegment.SegmentLength() / WithRoad;
  }

  #endregion


  #region Property
  /// <summary>
  /// Список всех дорог, в нашей сети
  /// </summary>
  public List<RoadSegment> RoadSegments { get; private set; }

  /// <summary>
  /// Список всех пересечений в нашей сети
  /// </summary>
  public List<Intersection> RoadIntersections { get; private set; }

  /// <summary>
  /// Объект в сцене, родитель всей сети
  /// </summary>
  public Transform RoadNetworkTransform { get; set; }

  /// <summary>
  /// Объект в сцене, родитель для всех пересечений
  /// </summary>
  public Transform RoadIntersectionTransform { get; set; }

  public Dictionary<HVector3, GameObject> ViewObjects { get; private set; }

  /// <summary>
  /// Масштаб сети дорог
  /// </summary>
  public float Scale { get; set; }

  /// <summary>
  /// Ширина элемента дороши (Ширины префаба)
  /// </summary>
  public float WithRoad { get; set; }

  /// <summary>
  /// Минимальная длинна сегмента дороги, иначе он будет удален из сети
  /// </summary>
  public float ShortCutOff { get; private set; }

  public float CloseCutoff { get; private set; }
  #endregion
}