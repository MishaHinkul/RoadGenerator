using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowRoadSegmentsCommands : BaseCommand
{
    [Inject]
    public RoadNetworkModel networkModel { get; private set; }

    public override void Execute()
    {
        GameObject intersection = Resources.Load<GameObject>("ROAD_straight");
        for (int i = 0; i < networkModel.roadSegments.Count; i++)
        {
            RoadPoint roadPointA = networkModel.roadSegments[i].PointA;
            RoadPoint roadPointB = networkModel.roadSegments[i].PointB;

            Vector3 globalPosition = new Vector3(roadPointA.point.x, networkModel.roadIntersectionTransform.position.y, roadPointA.point.y);
            Vector2 directionSegment = roadPointB.point - roadPointA.point;
            Vector3 forward = new Vector3(directionSegment.x, networkModel.roadIntersectionTransform.position.y, directionSegment.y);

            float step = intersection.transform.lossyScale.z;
            float iteration = Vector2.Distance(roadPointA.point, roadPointB.point) / step;

            forward = forward.normalized; //Направление в котором будм создавать тайлы дороги

            for (float pos = step; pos < iteration; pos += step)
            {
                Vector3 instPosition = globalPosition + (forward * pos);
                //Проверка для того чтобы не создать объект на пересечении
                if (!Contains(instPosition))
                {
                    GameObject instacGO = GameObject.Instantiate<GameObject>(intersection, instPosition, Quaternion.LookRotation(forward));
                }
            }
        }
    }

    /// <summary>
    /// Есть ли объект пересечения в заданых координатах
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public bool Contains(Vector3 pos)
    {
        for (int i = 0; i < networkModel.viewIntersection.Count; i++)
        {
            if (networkModel.viewIntersection[i] == pos)
            {
                return true;
            }
        }
        return false;
    }
}