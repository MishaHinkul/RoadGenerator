using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitSegmentForLevelCommand : BaseCommand
{
  public override void Execute()
  {
    SplitSegmentForLevelModel model = eventData.data as SplitSegmentForLevelModel;

    SplitSegmentModel segmentModel = null;
    List<RoadSegment> segments = new List<RoadSegment>(NetworkModel.GetRoadSegmentList());

    for (int i = 0; i < segments.Count; i++)
    {
      if (segments[i].Level == model.Level)
      {
        segmentModel = new SplitSegmentModel(segments[i]);
        dispatcher.Dispatch(EventGlobal.E_SplitSegment, segmentModel);
      }
    }

    CallbackUnlit.Execute(model.Callback);
  }

  [Inject]
  public RoadNetworkModel NetworkModel { get; private set; }
}