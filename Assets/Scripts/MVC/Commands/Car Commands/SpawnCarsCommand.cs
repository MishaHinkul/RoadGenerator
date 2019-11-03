using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCarsCommand : BaseCommand
{
  public override void Execute()
  {
    CoroutineExecutor.StartCoroutine(Spawn());
  }

  private IEnumerator Spawn()
  {
    WaitForSeconds waitTime = new WaitForSeconds(SettingsModel.CarSpawnTime);
    WaitForEndOfFrame waitFrame = new WaitForEndOfFrame();
    while (true)
    {
      yield return waitTime;
      GameObject instanceGO = InstanceCar();
      yield return waitFrame;
      dispatcher.Dispatch(EventGlobal.E_CarLogics, instanceGO);
    }
  }

  private GameObject InstanceCar()
  {
    return GameObject.Instantiate<GameObject>(Prefabs.Car);
  }


  [Inject]
  public ICoroutineExecutor CoroutineExecutor { get; private set; }

  [Inject]
  public SettingsModel SettingsModel { get; private set; }

  [Inject]
  public RoadPrefabsModel Prefabs { get; private set; }
}