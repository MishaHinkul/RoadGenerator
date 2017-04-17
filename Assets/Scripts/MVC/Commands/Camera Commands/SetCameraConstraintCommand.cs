using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCameraConstraintCommand : BaseCommand
{
    [Inject]
    public RoadNetworkModel networkModel { get; private set; }

    [Inject]
    public CameraSettings cameraSettingsModel { get; private set; }

    public override void Execute()
    {
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
