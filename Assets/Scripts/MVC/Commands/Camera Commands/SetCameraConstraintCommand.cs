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
        cameraSettingsModel.Constraint.ConstraintTopRight = new Vector3(networkModel.RoadIntersectionTransform.position.x - networkModel.Scale,
                                                                      networkModel.RoadIntersectionTransform.position.y,
                                                                      networkModel.RoadIntersectionTransform.position.z - networkModel.Scale
                                                                      );

        cameraSettingsModel.Constraint.ConstraintTopLeft = new Vector3(networkModel.RoadIntersectionTransform.position.x + networkModel.Scale,
                                                  networkModel.RoadIntersectionTransform.position.y,
                                                  networkModel.RoadIntersectionTransform.position.z - networkModel.Scale);

        cameraSettingsModel.Constraint.ConstraintBottomRight = new Vector3(networkModel.RoadIntersectionTransform.position.x - networkModel.Scale,
                                                                     networkModel.RoadIntersectionTransform.position.y,
                                                                     networkModel.RoadIntersectionTransform.position.z + networkModel.Scale
                                                                     );

        cameraSettingsModel.Constraint.ConstraintBottomLeft = new Vector3(networkModel.RoadIntersectionTransform.position.x + networkModel.Scale,
                                                  networkModel.RoadIntersectionTransform.position.y,
                                                  networkModel.RoadIntersectionTransform.position.z + networkModel.Scale);

        cameraSettingsModel.Constraint.SetArr();
    }
}
