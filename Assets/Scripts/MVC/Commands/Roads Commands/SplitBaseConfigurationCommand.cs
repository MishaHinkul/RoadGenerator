using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitBaseConfigurationCommand : BaseCommand
{
  public override void Execute()
  {
    Retain();

    SplitLevel(0);
    SplitLevel(1);
    SplitLevel(2);
    SplitLevel(3);

    Release();
  }

  private void SplitLevel(int level)
  {
    SplitSegmentForLevelModel segmentForLevelModel = new SplitSegmentForLevelModel(level);
    dispatcher.Dispatch(EventGlobal.E_SplitSegmentForLevel, segmentForLevelModel);
  }
}