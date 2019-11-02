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
    ViewIntersection = new LinkedList<HVector3>();
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

  public Vector3 GetWorldForwad(RoadSegment roadSegment)
  {
    Vector2 local = roadSegment.GetForward();
    Vector3 forward = new Vector3(local.x, RoadIntersectionTransform.position.y, local.y);
    return forward.normalized;
  }

  public Vector3 GetWorldPositionBeginSegment(RoadSegment roadSegment)
  {
    return new Vector3(roadSegment.Begin.Point.x, 
                       RoadIntersectionTransform.position.y, 
                       roadSegment.Begin.Point.y);
  }

  public Vector3 GetWorldPositionEndSegment(RoadSegment roadSegment)
  {
    return new Vector3(roadSegment.End.Point.x,
                       RoadIntersectionTransform.position.y,
                       roadSegment.End.Point.y);
  }

  public float GetWithSegment(RoadSegment roadSegment)
  {
    return roadSegment.SegmentLength() / WithRoad;
  }

  public bool ContainsViewBeginSegment(RoadSegment roadSegment)
  {
    return CointainsViewSegmentPoint(GetWorldPositionBeginSegment(roadSegment));
  }

  public bool ContainsViewEndSegment(RoadSegment roadSegment)
  {
    return CointainsViewSegmentPoint(GetWorldPositionEndSegment(roadSegment));
  }

  /// <summary>
  /// Есть ли объект пересечения в заданых координатах
  /// </summary>
  /// <param name="pos"></param>
  /// <returns></returns>
  public bool CointainsViewSegmentPoint(Vector3 pos)
  {
    return ViewIntersection.Contains(new HVector3(pos));
  }

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

  public LinkedList<HVector3> ViewIntersection { get; private set; }

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
}