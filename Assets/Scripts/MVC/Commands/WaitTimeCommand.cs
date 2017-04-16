using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitTimeCommand : BaseCommand
{
    [Inject]
    public ICoroutineExecutor coroutineExecutor { get; private set; }

    private WaitTimeModel model = null;

    public override void Execute()
    {
        model = eventData.data as WaitTimeModel;
        if (model == null || model.time <= 0)
        {
            return;
        }
        coroutineExecutor.StartCoroutine(Wait());


    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(model.time);
        if (model.callback != null)
        {
            model.callback();
        }
    }
}