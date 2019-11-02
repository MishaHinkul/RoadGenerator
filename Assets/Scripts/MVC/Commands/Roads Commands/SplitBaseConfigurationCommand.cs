using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitBaseConfigurationCommand : BaseCommand
{
  private CallbackUnlit.FlagValue waitFalg;
  public override void Execute()
  {
    Retain();
    Executor.StartCoroutine(SplitForLevel());
  }

  private IEnumerator SplitForLevel()
  {
    yield return Executor.StartCoroutine(SplitLevel(0));
    yield return Executor.StartCoroutine(SplitLevel(1));
    yield return Executor.StartCoroutine(SplitLevel(2));
    yield return Executor.StartCoroutine(SplitLevel(3));

    Release();
  }

  private IEnumerator SplitLevel(int level)
  {

    CallbackUnlit.PushFlag();
    WaitUntil waitUntil = new WaitUntil(CallbackUnlit.PeekFlagAnonym());

    SplitSegmentForLevelModel segmentForLevelModel = new SplitSegmentForLevelModel(level, CallbackUnlit.PeekFlagTrue);
    dispatcher.Dispatch(EventGlobal.E_SplitSegmentForLevel, segmentForLevelModel);

    yield return waitUntil;
    CallbackUnlit.PopFlag();
  }

  private bool GetWaitFlagValue()
  {
    return waitFalg.Value;
  }

  [Inject]
  public ICoroutineExecutor Executor { get; private set; }
}