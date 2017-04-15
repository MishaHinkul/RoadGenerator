using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneradeRoadsCommand : BaseCommand
{
    [Inject]
    public RoadNetworkModel networkModel { get; private set; }

    public override void Execute()
    {
        networkModel.Scale = 18;

        if (networkModel.roadNetworkTransform == null)
        {
            GameObject roadGO = GameObject.Find("Roads Network");
            if (roadGO != null)
            {
                networkModel.roadNetworkTransform = roadGO.transform;
                if (networkModel.roadIntersectionTransform == null)
                {
                    networkModel.roadIntersectionTransform = networkModel.roadNetworkTransform.FindChild("Road Intersections");
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
