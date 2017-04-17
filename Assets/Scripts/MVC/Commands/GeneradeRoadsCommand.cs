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
        networkModel.Scale = settingsModel.scale;

        if (networkModel.roadNetworkTransform == null)
        {
            GameObject roadGO = GameObject.Find("Roads Network");
            if (roadGO != null)
            {
                networkModel.roadNetworkTransform = roadGO.transform;
                if (networkModel.roadIntersectionTransform == null)
                {
                    networkModel.roadIntersectionTransform = networkModel.roadNetworkTransform.FindChild("Road Intersections");

                    //Определяем область ограничения движения камеры, в мире
                    cameraSettingsModel.constraint.constraintTopRight = new Vector3(networkModel.roadIntersectionTransform.position.x - networkModel.Scale,
                                                                                  networkModel.roadIntersectionTransform.position.y,
                                                                                  networkModel.roadIntersectionTransform.position.z - networkModel.Scale
                                                                                  );

                    cameraSettingsModel.constraint.constraintTopLeft = new Vector3(networkModel.roadIntersectionTransform.position.x + networkModel.Scale,
                                                              networkModel.roadIntersectionTransform.position.y,
                                                              networkModel.roadIntersectionTransform.position.z - networkModel.Scale);

                    cameraSettingsModel.constraint.constraintBottomRight = new Vector3(networkModel.roadIntersectionTransform.position.x - networkModel.Scale,
                                                                                 networkModel.roadIntersectionTransform.position.y,
                                                                                 networkModel.roadIntersectionTransform.position.z + networkModel.Scale
                                                                                 );

                    cameraSettingsModel.constraint.constraintBottomLeft = new Vector3(networkModel.roadIntersectionTransform.position.x + networkModel.Scale,
                                                              networkModel.roadIntersectionTransform.position.y,
                                                              networkModel.roadIntersectionTransform.position.z + networkModel.Scale);

                    cameraSettingsModel.constraint.SetArr();
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
