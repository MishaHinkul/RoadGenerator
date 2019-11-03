using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializePrefabModelCommand : BaseCommand
{
  public override void Execute()
  {
    LoadResources();
  }

  private void LoadResources()
  {
    if (Prefabs.Road == null)
    {
      Prefabs.Road = Resources.Load<GameObject>("ROAD_straight");
    }
    if (Prefabs.DeadLock == null)
    {
      Prefabs.DeadLock = Resources.Load<GameObject>("ROAD_deadlock");
    }
    if (Prefabs.IntersectionFour == null)
    {
      Prefabs.IntersectionFour = Resources.Load<GameObject>("ROAD_intersection");
    }
    if (Prefabs.IntersectionT == null)
    {
      Prefabs.IntersectionT = Resources.Load<GameObject>("ROAD_intersection_T");
    }
    if (Prefabs.GassStation == null)
    {
      Prefabs.GassStation = Resources.Load<GameObject>("GassStation");
    }
    if (Prefabs.Car == null)
    {
      Prefabs.Car = Resources.Load<GameObject>("Car");
    }
  }

  [Inject]
  public RoadPrefabsModel Prefabs { get; private set; }
}
