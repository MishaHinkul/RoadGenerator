using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleCameraCommand : BaseCommand
{
  public override void Execute()
  {
    if (eventData.data == null)
    {
      return;
    }
    float modelData = (float)eventData.data;
    if (modelData == 0)
    {
      return;
    }

    modelData *= CameraSettings.Distance.Speed;
    CameraSettings.Distance.DesiredDistance = CameraSettings.Distance.CurrentDistance + modelData;
    CameraSettings.Distance.DesiredDistance = Mathf.Clamp(CameraSettings.Distance.DesiredDistance,
                                                          CameraSettings.Distance.MinMaxDistance.x,
                                                          CameraSettings.Distance.MinMaxDistance.y);

    CameraSettings.LerpScale = true;
  }


  [Inject]
  public CameraSettings CameraSettings { get; private set; }
}