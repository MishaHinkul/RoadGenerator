using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowRoadSegmentCommand : BaseCommand
{
  private GameObject roadPrefab = null;
  private GameObject roadPrefabEnd = null;
  private ShowSegmentnModel model = null;

  public override void Execute()
  {
    LoadResources();
    if (!ValidationPrefab())
    {
      return;
    }
    model = eventData.data as ShowSegmentnModel;
    if (model == null)
    {
      return;
    }
    NetworkModel.WithRoad = roadPrefab.transform.lossyScale.z;

    Retain();
    Executor.StartCoroutine(Visualization());
  }

  private IEnumerator Visualization()
  {
    RoadPoint roadPointA = null;
    RoadPoint roadPointB = null;
    Vector3 forward;
    Vector3 instPosition;
    Vector3 globalPosition;
    float iteration;
    float step = NetworkModel.WithRoad;

    WaitForSeconds wait = new WaitForSeconds(0.5f);

    roadPointA = model.Segment.Begin;
    roadPointB = model.Segment.End;
    forward = GetForward(roadPointA, roadPointB);
    globalPosition = new Vector3(roadPointA.Point.x, NetworkModel.RoadIntersectionTransform.position.y, roadPointA.Point.y);
    iteration = Vector2.Distance(roadPointA.Point, roadPointB.Point) / step;

    for (float pos = step; pos < iteration; pos += step)
    {
      instPosition = globalPosition + (forward * pos);
      if (!Contains(instPosition))  //Проверка для того чтобы не создать объект на перекрестке, они созданы ранее
      {
        InstancePrefab(roadPrefab, instPosition, forward);
      }
      yield return wait;
    }
    InstanceDeadLock(roadPointA, roadPointB, forward);

    if (model.Callback != null)
    {
      model.Callback();
    }
    Release();
  }

  private void LoadResources()
  {
    roadPrefab = Resources.Load<GameObject>("ROAD_straight");
    roadPrefabEnd = Resources.Load<GameObject>("ROAD_deadlock");
  }

  private bool ValidationPrefab()
  {
    return !(roadPrefab == null || NetworkModel.RoadNetworkTransform == null);
  }

  private Vector3 GetForward(RoadPoint pointA, RoadPoint pointB)
  {
    Vector2 directionSegment = pointB.Point - pointA.Point;
    Vector3 forward = new Vector3(directionSegment.x, NetworkModel.RoadIntersectionTransform.position.y, directionSegment.y);
    forward = forward.normalized; //Направление в котором будем создавать тайлы дороги

    return forward;
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

  [Inject]
  public ICoroutineExecutor Executor { get; private set; }
}