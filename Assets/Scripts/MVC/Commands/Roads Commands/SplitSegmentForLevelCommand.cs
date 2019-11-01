using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitSegmentForLevelCommand : BaseCommand
{
  public override void Execute()
  {
    Retain();
    Executor.StartCoroutine(Split());
  }

  private IEnumerator Split()
  {
    SplitSegmentForLevelModel model = eventData.data as SplitSegmentForLevelModel;

    SplitSegmentModel segmentModel = null;
    WaitUntil wait = new WaitUntil(CallbackUnlit.Unlit);
    List<RoadSegment> segments = new List<RoadSegment>(NetworkModel.RoadSegments);

    for (int i = 0; i < segments.Count; i++)
    {
      if (segments[i].Level == model.Level)
      {
        segmentModel = new SplitSegmentModel();
        segmentModel.Segment = segments[i];
        segmentModel.Callback = CallbackUnlit.Unlit;

        dispatcher.Dispatch(EventGlobal.E_SplitSegment, segmentModel);

        yield return wait;
      }
    }

    CallbackUnlit.Execute(model.Callback);
    Release();
  }

  [Inject]
  public RoadNetworkModel NetworkModel { get; private set; }

  [Inject]
  public ICoroutineExecutor Executor { get; private set; }
}