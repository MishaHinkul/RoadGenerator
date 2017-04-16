using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCarsCommand : BaseCommand
{
    [Inject]
    public ICoroutineExecutor coroutineExecutor { get; private set; }

    private const float  SPACING = 5f;

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
        //while(true)
        //{
            yield return new WaitForSeconds(SPACING);
            GameObject instanceGO = GameObject.Instantiate<GameObject>(carPrefab);
            if (instanceGO != null)
            {
                instanceGO.name = "Car";
                dispatcher.Dispatch(EventGlobal.E_InitCar, instanceGO);
            }
        //}
    }
}