using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowIntersectionCommand : BaseCommand
{
  private ShowIntersectionModel model = null;

  public override void Execute()
  {
    model = eventData.data as ShowIntersectionModel;
    if (model == null)
    {
      return;
    }

    Retain();
    Executor.StartCoroutine(Visual());
  }

  private IEnumerator Visual()
  {
    Quaternion rotation = NetworkModel.LookRotationWorldIntersection(model.Intersection);
    Vector3 position = NetworkModel.GetWorldPositionIntersection(model.Intersection);
    WaitForSeconds wait = new WaitForSeconds(SettingsModel.SpeedVisualizeAlgorithm);
    RoadNetworkModel.KindIntersection kindIntersection = NetworkModel.GetKindIntersection(model.Intersection);

    HVector3 hPos = new HVector3(position);
    if (NetworkModel.ViewObjects.Count > 0 && NetworkModel.ViewObjects.ContainsKey(hPos))
    {
      GameObject oldObj = NetworkModel.ViewObjects[hPos];
      if (oldObj != null)
      {
        GameObject.Destroy(oldObj);
      }
    }

    switch (kindIntersection)
    {
      case RoadNetworkModel.KindIntersection.T:
        GameObject t = GameObject.Instantiate<GameObject>(Prefabs.IntersectionT, position, rotation, NetworkModel.RoadIntersectionTransform);
        NetworkModel.ViewObjects.Add(hPos, t);
        yield return wait;
        break;
      case RoadNetworkModel.KindIntersection.Four:
        GameObject four = GameObject.Instantiate<GameObject>(Prefabs.IntersectionFour, position, rotation, NetworkModel.RoadIntersectionTransform);
        NetworkModel.ViewObjects.Add(hPos, four);
        yield return wait;
        break;
    }

    CallbackUnlit.Execute(model.Callback);
    Release();
  }


  [Inject]
  public ICoroutineExecutor Executor { get; private set; }

  [Inject]
  public RoadNetworkModel NetworkModel { get; private set; }

  [Inject]
  public SettingsModel SettingsModel { get; private set; }

  [Inject]
  public RoadPrefabsModel Prefabs { get; private set; }
}