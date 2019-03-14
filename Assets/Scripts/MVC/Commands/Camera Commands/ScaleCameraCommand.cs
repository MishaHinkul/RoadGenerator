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



        cameraSettings.Distance.MinMaxDistance = new Vector2(6, 14);
        cameraSettings.Distance.Speed = 25;
        model *= cameraSettings.Distance.Speed;
        cameraSettings.Distance.DesiredDistance = cameraSettings.Distance.CurrentDistance + model;
        cameraSettings.Distance.DesiredDistance = Mathf.Clamp(cameraSettings.Distance.DesiredDistance, 
                                                              cameraSettings.Distance.MinMaxDistance.x, 
                                                              cameraSettings.Distance.MinMaxDistance.y);
        cameraSettings.Distance.SpeedChangeDistance = 0.15f;
        cameraSettings.LerpScale = true;
    }
}