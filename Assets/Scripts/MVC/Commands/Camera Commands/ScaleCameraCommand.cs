using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleCameraCommand : BaseCommand
{
    [Inject]
    public CameraSettings cameraSettings { get; private set; }
    public override void Execute()
    {
        if (eventData.data == null)
        {
            return;
        }
        float model = (float)eventData.data;
        if (model == 0)
        {
            return;
        }



        cameraSettings.distance.min_max_Distance = new Vector2(6, 14);
        cameraSettings.distance.speed = 25;
        model *= cameraSettings.distance.speed;
        cameraSettings.distance.desiredDistance = cameraSettings.distance.currentDistance + model;
        cameraSettings.distance.desiredDistance = Mathf.Clamp(cameraSettings.distance.desiredDistance, 
                                                              cameraSettings.distance.min_max_Distance.x, 
                                                              cameraSettings.distance.min_max_Distance.y);
        cameraSettings.distance.speedChangeDistance = 0.15f;
        cameraSettings.lerpScale = true;
    }
}
