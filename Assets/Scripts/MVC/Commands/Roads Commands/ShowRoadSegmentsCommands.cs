using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowRoadSegmentsCommands : BaseCommand
{
    [Inject]
    public RoadNetworkModel networkModel { get; private set; }

    public override void Execute()
    {
        GameObject roadPrefab = Resources.Load<GameObject>("ROAD_straight");
     

        if (roadPrefab == null || networkModel.RoadNetworkTransform == null)
        {
            return;
        }
        networkModel.WithRoad = roadPrefab.transform.lossyScale.z;

        for (int i = 0; i < networkModel.RoadSegments.Count; i++)
        {
            RoadPoint roadPointA = networkModel.RoadSegments[i].Begin;
            RoadPoint roadPointB = networkModel.RoadSegments[i].End;

            Vector3 globalPosition = new Vector3(roadPointA.Point.x, networkModel.RoadIntersectionTransform.position.y, roadPointA.Point.y);
            Vector2 directionSegment = roadPointB.Point - roadPointA.Point;
            Vector3 forward = new Vector3(directionSegment.x, networkModel.RoadIntersectionTransform.position.y, directionSegment.y);

            float step = roadPrefab.transform.lossyScale.z;
            float iteration = Vector2.Distance(roadPointA.Point, roadPointB.Point) / step;

            forward = forward.normalized; //Направление в котором будм создавать тайлы дороги

            for (float pos = step; pos < iteration; pos += step)
            {
                Vector3 instPosition = globalPosition + (forward * pos);
                //Проверка для того чтобы не создать объект на пересечении
                if (!Contains(instPosition))
                {
                    GameObject instacGO = GameObject.Instantiate<GameObject>(roadPrefab, instPosition, Quaternion.LookRotation(forward));
                    instacGO.transform.parent = networkModel.RoadNetworkTransform;
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
        for (int i = 0; i < networkModel.ViewIntersection.Count; i++)
        {
            if (networkModel.ViewIntersection[i] == pos)
            {
                return true;
            }
        }
        return false;
    }
}