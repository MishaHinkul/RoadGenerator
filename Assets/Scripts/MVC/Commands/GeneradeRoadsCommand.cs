using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneradeRoadsCommand : BaseCommand
{
  public override void Execute()
  {
    NetworkModel.Scale = SettingsModel.Scale;

    if (NetworkModel.RoadNetworkTransform == null)
    {
      GameObject roadGO = GameObject.Find("Roads Network");
      if (roadGO != null)
      {
        NetworkModel.RoadNetworkTransform = roadGO.transform;
        if (NetworkModel.RoadIntersectionTransform == null)
        {
          NetworkModel.RoadIntersectionTransform = NetworkModel.RoadNetworkTransform.Find("Road Intersections");
        }
      }
      else
      {
        Debug.LogError("GameObject: Roads Network - not found");
      }
    }
    CenterTemplateModel templateModel = new CenterTemplateModel(Vector2.zero, 270);
    dispatcher.Dispatch(EventGlobal.E_SetTemplate, templateModel);
  }


  [Inject]
  public RoadNetworkModel NetworkModel { get; private set; }

  [Inject]
  public SettingsModel SettingsModel { get; private set; }

  [Inject]
  public CameraSettings CameraSettingsModel { get; private set; }
}