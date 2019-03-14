using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneradeRoadsCommand : BaseCommand
{
    [Inject]
    public RoadNetworkModel networkModel { get; private set; }

    [Inject]
    public SettingsModel settingsModel { get; private set; }

    [Inject]
    public CameraSettings cameraSettingsModel { get; private set; }

    public override void Execute()
    {
        networkModel.Scale = settingsModel.Scale;

        if (networkModel.RoadNetworkTransform == null)
        {
            GameObject roadGO = GameObject.Find("Roads Network");
            if (roadGO != null)
            {
                networkModel.RoadNetworkTransform = roadGO.transform;
                if (networkModel.RoadIntersectionTransform == null)
                {
                    networkModel.RoadIntersectionTransform = networkModel.RoadNetworkTransform.Find("Road Intersections");
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
}
