using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitSegmentForLevelCommand : BaseCommand
{
  public override void Execute()
  {
    int level = (int)eventData.data;

    List<RoadSegment> segments = new List<RoadSegment>(NetworkModel.RoadSegments);
    for (int i = 0; i < segments.Count; i++)
    {
      if (segments[i].Level == level)
      {
        dispatcher.Dispatch(EventGlobal.E_SplitSegment, segments[i]);
      }
    }
  }


  [Inject]
  public RoadNetworkModel NetworkModel { get; private set; }
}