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
    Vector3 forward;
    Vector3 instPosition;
    Vector3 globalPosition;
    float iteration;
    float step = NetworkModel.WithRoad;

    WaitForSeconds wait = new WaitForSeconds(SettingsModel.SpeedVisualizeAlgorithm);

    forward = NetworkModel.GetWorldForwad(model.Segment);
    globalPosition = NetworkModel.GetWorldPositionBeginSegment(model.Segment);
    iteration = NetworkModel.GetWithSegment(model.Segment);

    for (float pos = step; pos < iteration; pos += step)
    {
      instPosition = globalPosition + (forward * pos);
      if (!NetworkModel.CointainsViewSegmentPoint(instPosition))  //Проверка для того чтобы не создать объект на перекрестке, они созданы ранее
      {
        InstancePrefab(roadPrefab, instPosition, forward);
      }
      yield return wait;
    }
    InstanceDeadLock(model.Segment, forward);

    CallbackUnlit.Execute(model.Callback);
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

  private void InstanceDeadLock(RoadSegment roadSegment, Vector3 forward)
  {
    if (!NetworkModel.ContainsViewBeginSegment(roadSegment))
    {
      InstancePrefab(roadPrefabEnd, NetworkModel.GetWorldPositionBeginSegment(roadSegment), forward);
    }
    if (!NetworkModel.ContainsViewEndSegment(roadSegment))
    {
      InstancePrefab(roadPrefabEnd, NetworkModel.GetWorldPositionEndSegment(roadSegment), -forward);
    }
  }

  private void InstancePrefab(GameObject template, Vector3 position, Vector3 forward)
  {
    GameObject.Instantiate<GameObject>(template,
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
}