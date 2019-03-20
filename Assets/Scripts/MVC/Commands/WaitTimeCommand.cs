using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitTimeCommand : BaseCommand
{
  private WaitTimeModel model = null;

  public override void Execute()
  {
    model = eventData.data as WaitTimeModel;
    if (!Validation())
    {
      return;
    }
    CoroutineExecutor.StartCoroutine(Wait());
  }

  private bool Validation()
  {
    return model != null && model.Callback != null;
  }

  private IEnumerator Wait()
  {
    if (model.Time >= 0)
    {
      yield return new WaitForSeconds(model.Time);
    }
    model.Callback();
  }


  [Inject]
  public ICoroutineExecutor CoroutineExecutor { get; private set; }
}