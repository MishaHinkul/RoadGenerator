using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowRoadItemsCommand : BaseCommand
{
  public override void Execute()
  {
    Retain();
    Executor.StartCoroutine(Visual());
  }

  private IEnumerator Visual()
  {
    WaitForSeconds wait = new WaitForSeconds(SettingsModel.SpeedVisualizeAlgorithm);
    WaitUntil waitUntil = new WaitUntil(CallbackUnlit.PeekFlag);
    Intersection intersection = null;
    RoadSegment roadSegment = null;
    ShowSegmentnModel showSegmentnModel = new ShowSegmentnModel();

    for (int i = 0; i < NetworkModel.RoadItems.Count; i++)
    {
      intersection = NetworkModel.RoadItems[i] as Intersection;
      if (intersection != null)
      {
        dispatcher.Dispatch(EventGlobal.E_ShowIntersection, intersection);
        yield return wait;
      }
      else
      {
        roadSegment = NetworkModel.RoadItems[i] as RoadSegment;
        showSegmentnModel.Segment = roadSegment;
        showSegmentnModel.Callback = CallbackUnlit.PeekFlagTrue;
        CallbackUnlit.PushFlag();
        dispatcher.Dispatch(EventGlobal.E_ShowSegment, showSegmentnModel);
        yield return waitUntil;
        CallbackUnlit.PopFlag();
      }
    }
    int c = NetworkModel.RoadIntersections.Count;
    Release();
  }


  [Inject]
  public ICoroutineExecutor Executor { get; private set; }

  [Inject]
  public RoadNetworkModel NetworkModel { get; private set; }

  [Inject]
  public SettingsModel SettingsModel { get; private set; }
}