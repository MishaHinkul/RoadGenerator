using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCarsCommand : BaseCommand
{
  private GameObject carPrefab = null;

  public override void Execute()
  {
    LoadResource();
    if (Validation())
    {
      CoroutineExecutor.StartCoroutine(Spawn());
    }
    else
    {
      Debug.LogError("Car Prefub - not found");
    }
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
    GameObject instanceGO = GameObject.Instantiate<GameObject>(carPrefab);
    if (instanceGO != null)
    {
      instanceGO.name = "Car";
    }

    return instanceGO;
  }

  private void LoadResource()
  {
    carPrefab = Resources.Load<GameObject>("Car");
  }

  private bool Validation()
  {
    return carPrefab != null;
  }

  [Inject]
  public ICoroutineExecutor CoroutineExecutor { get; private set; }

  [Inject]
  public SettingsModel SettingsModel { get; private set; }
}