using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowIntersectionCommand : BaseCommand
{
  public override void Execute()
  {
    Intersection model = eventData.data as Intersection;
    if (model == null)
    {
      return;
    }

    Quaternion rotation = NetworkModel.LookRotationWorldIntersection(model);
    Vector3 position = NetworkModel.GetWorldPositionIntersection(model);
    RoadNetworkModel.KindIntersection kindIntersection = NetworkModel.GetKindIntersection(model);

    switch (kindIntersection)
    {
      case RoadNetworkModel.KindIntersection.T:
        GameObject t = GameObject.Instantiate<GameObject>(Prefabs.IntersectionT, position, rotation, NetworkModel.RoadIntersectionTransform);
        break;
      case RoadNetworkModel.KindIntersection.Four:
        GameObject four = GameObject.Instantiate<GameObject>(Prefabs.IntersectionFour, position, rotation, NetworkModel.RoadIntersectionTransform);
        break;
    }
  }

  [Inject]
  public RoadNetworkModel NetworkModel { get; private set; }

  [Inject]
  public RoadPrefabsModel Prefabs { get; private set; }
}