using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowRoadSegmentsCommands : BaseCommand
{
  private GameObject roadPrefab = null;
  private GameObject roadPrefabEnd = null;

  public override void Execute()
  {
    LoadResources();
    if (!Validation())
    {
      return;
    }
    NetworkModel.WithRoad = roadPrefab.transform.lossyScale.z;

    RoadPoint roadPointA = null;
    RoadPoint roadPointB = null;

    for (int i = 0; i < NetworkModel.RoadSegments.Count; i++)
    {
      roadPointA = NetworkModel.RoadSegments[i].Begin;
      roadPointB = NetworkModel.RoadSegments[i].End;

      Vector3 globalPosition = new Vector3(roadPointA.Point.x, NetworkModel.RoadIntersectionTransform.position.y, roadPointA.Point.y);
      Vector2 directionSegment = roadPointB.Point - roadPointA.Point;
      Vector3 forward = new Vector3(directionSegment.x, NetworkModel.RoadIntersectionTransform.position.y, directionSegment.y);

      float step = roadPrefab.transform.lossyScale.z;
      float iteration = Vector2.Distance(roadPointA.Point, roadPointB.Point) / step;

      forward = forward.normalized; //Направление в котором будм создавать тайлы дороги

      InstanceDeadLock(roadPointA, roadPointB, forward);

      for (float pos = step; pos < iteration; pos += step)
      {
        Vector3 instPosition = globalPosition + (forward * pos);

        //Проверка для того чтобы не создать объект на пересечении
        if (!Contains(instPosition))
        {
          InstancePrefab(roadPrefab, instPosition, forward);
        }
      }
    }
  }

  private void LoadResources()
  {
    roadPrefab = Resources.Load<GameObject>("ROAD_straight");
    roadPrefabEnd = Resources.Load<GameObject>("ROAD_deadlock");
  }

  private bool Validation()
  {
    return !(roadPrefab == null || NetworkModel.RoadNetworkTransform == null);
  }

  private void InstanceDeadLock(RoadPoint roadPointA, RoadPoint roadPointB, Vector3 forward)
  {
    Vector3 worldPosA = roadPointA.WorldPosition;
    Vector3 worldPosB = roadPointB.WorldPosition;

    if (!Contains(worldPosA))
    {
      InstancePrefab(roadPrefabEnd, worldPosA, forward);
    }
    if (!Contains(worldPosB))
    {
      InstancePrefab(roadPrefabEnd, worldPosB, -forward);
    }
  }

  private void InstancePrefab(GameObject template, Vector3 position, Vector3 forward)
  {
    GameObject.Instantiate<GameObject>(template,
                                       position,
                                       Quaternion.LookRotation(forward),
                                       NetworkModel.RoadNetworkTransform);
  }

  /// <summary>
  /// Есть ли объект пересечения в заданых координатах
  /// </summary>
  /// <param name="pos"></param>
  /// <returns></returns>
  public bool Contains(Vector3 pos)
  {
    return NetworkModel.ViewIntersection.Contains(new HVector3(pos));
  }


  [Inject]
  public RoadNetworkModel NetworkModel { get; private set; }
}