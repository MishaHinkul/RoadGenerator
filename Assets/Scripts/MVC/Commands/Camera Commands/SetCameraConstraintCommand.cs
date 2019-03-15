﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCameraConstraintCommand : BaseCommand
{
  public override void Execute()
  {
    //Определяем область ограничения движения камеры, в мире
    CameraSettingsModel.Constraint.ConstraintTopRight = new Vector3(NetworkModel.RoadIntersectionTransform.position.x - NetworkModel.Scale,
                                                                    NetworkModel.RoadIntersectionTransform.position.y,
                                                                    NetworkModel.RoadIntersectionTransform.position.z - NetworkModel.Scale);

    CameraSettingsModel.Constraint.ConstraintTopLeft = new Vector3(NetworkModel.RoadIntersectionTransform.position.x + NetworkModel.Scale,
                                                                   NetworkModel.RoadIntersectionTransform.position.y,
                                                                   NetworkModel.RoadIntersectionTransform.position.z - NetworkModel.Scale);

    CameraSettingsModel.Constraint.ConstraintBottomRight = new Vector3(NetworkModel.RoadIntersectionTransform.position.x - NetworkModel.Scale,
                                                                       NetworkModel.RoadIntersectionTransform.position.y,
                                                                       NetworkModel.RoadIntersectionTransform.position.z + NetworkModel.Scale);

    CameraSettingsModel.Constraint.ConstraintBottomLeft = new Vector3(NetworkModel.RoadIntersectionTransform.position.x + NetworkModel.Scale,
                                                                      NetworkModel.RoadIntersectionTransform.position.y,
                                                                      NetworkModel.RoadIntersectionTransform.position.z + NetworkModel.Scale);

    CameraSettingsModel.Constraint.SetArr();
  }


  [Inject]
  public RoadNetworkModel NetworkModel { get; private set; }

  [Inject]
  public CameraSettings CameraSettingsModel { get; private set; }
}