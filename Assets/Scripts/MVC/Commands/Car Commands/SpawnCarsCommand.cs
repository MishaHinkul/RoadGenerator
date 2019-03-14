using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCarsCommand : BaseCommand
{
    [Inject]
    public ICoroutineExecutor coroutineExecutor { get; private set; }

    [Inject]
    public SettingsModel settingsModel { get; private set; }

    private GameObject carPrefab = null;

    public override void Execute()
    {
        carPrefab = Resources.Load<GameObject>("Car");
        if (carPrefab != null)
        {
            coroutineExecutor.StartCoroutine(Spawn());
        }
        else
        {
            Debug.LogError("Car Prefub - not found");
        }
    }

    private IEnumerator Spawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(settingsModel.CarSpawnTime);
            GameObject instanceGO = GameObject.Instantiate<GameObject>(carPrefab);
            yield return new WaitForEndOfFrame();
            if (instanceGO != null)
            {
                instanceGO.name = "Car";
                dispatcher.Dispatch(EventGlobal.E_CarLogics, instanceGO);
            }
        }
    }
}