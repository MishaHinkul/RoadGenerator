using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowIntersectionCommand : BaseCommand
{
    [Inject]
    public RoadNetworkModel networkModel { get; private set; }

    public override void Execute()
    {
        GameObject intersection = Resources.Load<GameObject>("ROAD_intersection");
        GameObject intersectionT = Resources.Load<GameObject>("ROAD_intersection_T");

        if (intersection == null || intersectionT == null || networkModel.RoadIntersectionTransform == null)
        {
            return;
        }

        GameObject intersectionGO = null;
        for (int i = 0; i < networkModel.RoadIntersections.Count; i++)
        {
            //networkModel.roadIntersections[i].Points - координаты у всех одинаковые

            RoadPoint roadPointA = networkModel.RoadIntersections[i].Points[0];
            RoadPoint roadPointB = roadPointA.mySegement.GetOther(roadPointA);
            Vector3 position = new Vector3(roadPointA.point.x, networkModel.RoadIntersectionTransform.position.y, roadPointA.point.y);
            networkModel.ViewIntersection.Add(position);
            Quaternion rotation;

            //Перекресток
            if (networkModel.RoadIntersections[i].Points.Count == 4)
            {
                rotation = LookRotation(roadPointA);

                intersectionGO = GameObject.Instantiate<GameObject>(intersection, position, rotation);
            }
            //T - образный перекресток
            else if (networkModel.RoadIntersections[i].Points.Count == 3)
            {
                intersectionGO = GameObject.Instantiate<GameObject>(intersectionT);
                intersectionGO.transform.position = position;
                rotation = LookRotationForTIntersection(networkModel.RoadIntersections[i].Points);
                intersectionGO.transform.rotation = rotation;
            }   

            if (intersectionGO != null)
            {
                intersectionGO.transform.parent = networkModel.RoadIntersectionTransform;
            }
        }
    }

    /// <summary>
    /// Получить вращение по направлению сегмента
    /// </summary>
    /// <param name="roadPoint"></param>
    /// <returns></returns>
    private Quaternion LookRotation(RoadPoint roadPoint)
    {
        RoadPoint roadPointA = roadPoint;
        RoadPoint roadPointB = roadPointA.mySegement.GetOther(roadPointA);

        Vector2 direction = roadPointB.point - roadPointA.point;
        Vector3 forward = new Vector3(direction.x, networkModel.RoadIntersectionTransform.position.y, direction.y);

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
            segment1PointB = segment1PointA.mySegement.GetOther(segment1PointA);

            segment2PointA = intersectionList[j + 1];
            segment2PointB = segment2PointA.mySegement.GetOther(segment2PointA);

            //2 вектора должны смотреть в разные стороны
            vectorSegment1 = segment1PointB.point - segment1PointA.point;
            vectorSegment2 = segment2PointA.point - segment2PointB.point;

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
}