using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleCameraCommand : BaseCommand
{
  private const float MIN_DISTANCE = 6f;
  private const float MAX_DISTANCE = 14f;
  private const float SPEED_CHANGE_DISTANCE = 0.15f;
  private const float SPEED = 25;

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



    CameraSettings.Distance.MinMaxDistance = new Vector2(MIN_DISTANCE, MAX_DISTANCE);
    CameraSettings.Distance.Speed = SPEED;
    modelData *= CameraSettings.Distance.Speed;
    CameraSettings.Distance.DesiredDistance = CameraSettings.Distance.CurrentDistance + modelData;
    CameraSettings.Distance.DesiredDistance = Mathf.Clamp(CameraSettings.Distance.DesiredDistance,
                                                          CameraSettings.Distance.MinMaxDistance.x,
                                                          CameraSettings.Distance.MinMaxDistance.y);
    CameraSettings.Distance.SpeedChangeDistance = SPEED_CHANGE_DISTANCE;
    CameraSettings.LerpScale = true;
  }


  [Inject]
  public CameraSettings CameraSettings { get; private set; }
}