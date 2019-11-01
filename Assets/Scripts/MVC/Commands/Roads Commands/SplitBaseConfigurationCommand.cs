using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitBaseConfigurationCommand : BaseCommand
{
  public override void Execute()
  {
    Retain();
    Executor.StartCoroutine(SplitForLevel());
  }

  private IEnumerator SplitForLevel()
  {
    SplitSegmentForLevelModel level0 = new SplitSegmentForLevelModel(0, CallbackUnlit.Unlit);
    SplitSegmentForLevelModel level1 = new SplitSegmentForLevelModel(1, CallbackUnlit.Unlit);
    SplitSegmentForLevelModel level2 = new SplitSegmentForLevelModel(2, CallbackUnlit.Unlit);
    SplitSegmentForLevelModel level3 = new SplitSegmentForLevelModel(3, CallbackUnlit.Unlit);

    WaitUntil waitUntil = new WaitUntil(CallbackUnlit.Unlit);

    dispatcher.Dispatch(EventGlobal.E_SplitSegmentForLevel, level0);
    yield return waitUntil;
    //dispatcher.Dispatch(EventGlobal.E_SplitSegmentForLevel, level0);
    //yield return waitUntil;

    dispatcher.Dispatch(EventGlobal.E_SplitSegmentForLevel, level1);
    yield return waitUntil;
    //dispatcher.Dispatch(EventGlobal.E_SplitSegmentForLevel, level1);
    //yield return waitUntil;

    dispatcher.Dispatch(EventGlobal.E_SplitSegmentForLevel, level2);
    yield return waitUntil;

    dispatcher.Dispatch(EventGlobal.E_SplitSegmentForLevel, level3);
    yield return waitUntil;
    Release();
  }

  [Inject]
  public ICoroutineExecutor Executor { get; private set; }
}