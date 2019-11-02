using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitBaseConfigurationCommand : BaseCommand
{
  public override void Execute()
  {
    SplitLevel(0);
    //yield return Executor.StartCoroutine(SplitLevel(1));
    //yield return Executor.StartCoroutine(SplitLevel(2));
    //yield return Executor.StartCoroutine(SplitLevel(3));
  }

  private void SplitLevel(int level)
  {
    SplitSegmentForLevelModel segmentForLevelModel = new SplitSegmentForLevelModel(level);
    dispatcher.Dispatch(EventGlobal.E_SplitSegmentForLevel, segmentForLevelModel);
  }
}