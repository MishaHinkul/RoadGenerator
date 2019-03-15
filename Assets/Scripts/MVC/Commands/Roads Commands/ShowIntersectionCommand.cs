using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowIntersectionCommand : BaseCommand
{
  private GameObject intersection = null;
  private GameObject intersectionT = null;

  public override void Execute()
  {
    LoadResources();
    if (!Validation())
    {
      return;
    }

    GameObject intersectionGO = null;
    RoadPoint roadPointA = null;
    RoadPoint roadPointB = null;
    Quaternion rotation;
    Vector3 position;

    for (int i = 0; i < NetworkModel.RoadIntersections.Count; i++)
    {
      //networkModel.roadIntersections[i].Points - координаты у всех одинаковые
      roadPointA = NetworkModel.RoadIntersections[i].Points[0];
      roadPointB = roadPointA.MySegement.GetOther(roadPointA);
      position = new Vector3(roadPointA.Point.x, NetworkModel.RoadIntersectionTransform.position.y, roadPointA.Point.y);

      NetworkModel.ViewIntersection.Add(position);
      switch (NetworkModel.RoadIntersections[i].Points.Count)
      {
        case 3: //T - образный перекресток
          rotation = LookRotationForTIntersection(NetworkModel.RoadIntersections[i].Points);
          intersectionGO = GameObject.Instantiate<GameObject>(intersectionT, position, rotation, NetworkModel.RoadIntersectionTransform);
          break;
        case 4: //Перекресток
          rotation = LookRotation(roadPointA);
          intersectionGO = GameObject.Instantiate<GameObject>(intersection, position, rotation, NetworkModel.RoadIntersectionTransform);
          break;
      }
    }
  }

  private void LoadResources()
  {
    intersection = Resources.Load<GameObject>("ROAD_intersection");
    intersectionT = Resources.Load<GameObject>("ROAD_intersection_T");
  }

  private bool Validation()
  {
    return !(intersection == null || intersectionT == null || NetworkModel.RoadIntersectionTransform == null);
  }

  /// <summary>
  /// Получить вращение по направлению сегмента
  /// </summary>
  /// <param name="roadPoint"></param>
  /// <returns></returns>
  private Quaternion LookRotation(RoadPoint roadPoint)
  {
    RoadPoint roadPointA = roadPoint;
    RoadPoint roadPointB = roadPointA.MySegement.GetOther(roadPointA);

    Vector2 direction = roadPointB.Point - roadPointA.Point;
    Vector3 forward = new Vector3(direction.x, NetworkModel.RoadIntersectionTransform.position.y, direction.y);

    return Quaternion.LookRotation(forward);
  }


  /// <summary>
  /// Получить вращение для Т образного пересечения
  /// </summary>
  private Quaternion LookRotationForTIntersection(List<RoadPoint> intersectionList)
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
    return LookRotation(intersectionList[indexDirectionPoint]);
  }


  [Inject]
  public RoadNetworkModel NetworkModel { get; private set; }
}