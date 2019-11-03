using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowRoadSegmentCommand : BaseCommand
{
  private ShowSegmentnModel model = null;

  public override void Execute()
  {
    model = eventData.data as ShowSegmentnModel;
    if (model == null)
    {
      return;
    }

    Retain();
    Executor.StartCoroutine(Visualization());
  }

  private IEnumerator Visualization()
  {
    Vector3 forward;
    Vector3 instPosition;
    Vector3 globalPosition;
    float iteration;
    float step = NetworkModel.WithRoad;

    WaitForSeconds wait = new WaitForSeconds(SettingsModel.SpeedVisualizeAlgorithm);

    forward = NetworkModel.GetWorldForwadSegment(model.Segment);
    globalPosition = NetworkModel.GetWorldPositionBeginSegment(model.Segment);
    iteration = NetworkModel.GetWithSegment(model.Segment);

    if (InstanceDeadLock(model.Segment.Begin, forward))
    {
      yield return wait;
    }
    for (float pos = step; pos < iteration; pos += step)
    {

      instPosition = globalPosition + (forward * pos);
      if (NetworkModel.WorldPositionToIntersection(instPosition) == null)
      {
        InstancePrefab(Prefabs.Road, instPosition, forward);
        yield return wait;
      }
    }
    if (InstanceDeadLock(model.Segment.End, -forward))
    {
      yield return wait;
    }

    CallbackUnlit.Execute(model.Callback);
    Release();
  }

  private GameObject InstanceDeadLock(RoadPoint point, Vector3 forward)
  {
    Vector3 wordldPosition = NetworkModel.GetWorldPositionPoint(point);
    if (NetworkModel.WorldPositionToIntersection(wordldPosition) == null)
    {
      return InstancePrefab(Prefabs.DeadLock, NetworkModel.GetWorldPositionPoint(point), forward);
    }

    return null;
  }

  private GameObject InstancePrefab(GameObject template, Vector3 position, Vector3 forward)
  {
    return GameObject.Instantiate<GameObject>(template,
                                              position,
                                              Quaternion.LookRotation(forward),
                                              NetworkModel.RoadNetworkTransform);
  }


  [Inject]
  public RoadNetworkModel NetworkModel { get; private set; }

  [Inject]
  public ICoroutineExecutor Executor { get; private set; }

  [Inject]
  public SettingsModel SettingsModel { get; private set; }

  [Inject]
  public RoadPrefabsModel Prefabs { get; private set; }
}