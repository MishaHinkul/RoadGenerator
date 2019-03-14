using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCameraCommand : BaseCommand
{
  private const float SPEED_MOVE = 3;
  private const float SPEED_CHANGE_POSITION = 0.6f;
  private const float STOP_LERP_DISTANCE = 0.05f;

  [Inject]
  public CameraSettings cameraSettings { get; private set; }

  public override void Execute()
  {
    if (eventData.data == null)
    {
      return;
    }
    Vector3 hitPosition = (Vector3)eventData.data;
    Vector3 center = new Vector3(Screen.width / 2, Screen.height / 2, 0);

    Ray ray = cameraSettings.CameraObj.ScreenPointToRay(center);
    RaycastHit hitCenter;
    if (Physics.Raycast(ray, out hitCenter))
    {
      Vector3 direction = hitPosition - hitCenter.point;
      direction.Normalize();
      if (cameraSettings.Constraint.isConstraint)
      {
        //Если движемся в сторону блокировки
        if (Vector3.Angle(direction, cameraSettings.Constraint.direction) < 15)
        {
          return;
        }
      }
      float distance = Vector3.Distance(hitPosition, hitCenter.point);
      Vector3 desiredPosition = cameraSettings.Focus.CurrentPosition + direction * distance;

      cameraSettings.Focus.SpeedMove = SPEED_MOVE;
      cameraSettings.Focus.SpeedChangePosition = SPEED_CHANGE_POSITION;
      cameraSettings.Focus.DesiredPosition = desiredPosition;
      cameraSettings.LerpMove = true;
    }
  }
}