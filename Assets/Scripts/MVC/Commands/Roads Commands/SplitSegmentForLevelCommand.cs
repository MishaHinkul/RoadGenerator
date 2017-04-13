using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitSegmentForLevelCommand : BaseCommand
{
    [Inject]
    public RoadNetworkModel networkModel { get; private set; }

    public override void Execute()
    {
        int level = (int)eventData.data;

        List<RoadSegment> segments = new List<RoadSegment>(networkModel.roadSegments);
        for (int i = 0; i < segments.Count; i++)
        {
            if (segments[i].Level == level)
            {
                dispatcher.Dispatch(EventGlobal.E_SplitSegment, segments[i]);
            }
        }
    }
}