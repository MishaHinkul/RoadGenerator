using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCameraCommand : BaseCommand
{
  private const float SPEED_MOVE = 3;
  private const float SPEED_CHANGE_POSITION = 0.6f;
  private const float ANGLE_CONSTRAINT = 15f;

  public override void Execute()
  {
    if (eventData.data == null)
    {
      return;
    }
    Vector3 hitPosition = (Vector3)eventData.data;
    Vector3 center = new Vector3(Screen.width / 2, Screen.height / 2, 0);
    Ray ray = CameraSettings.CameraObj.ScreenPointToRay(center);
    RaycastHit hitCenter;

    if (!Physics.Raycast(ray, out hitCenter))
    {
      return;
    }

    Vector3 direction = hitPosition - hitCenter.point;
    float distance = direction.magnitude;
    direction.Normalize();
    if (CameraSettings.Constraint.IsConstraint)
    {
      //Если движемся в сторону блокировки
      if (Vector3.Angle(direction, CameraSettings.Constraint.Direction) < ANGLE_CONSTRAINT)
      {
        return;
      }
    }
    SetCameraSettings(direction, distance);
  }

  private void SetCameraSettings(Vector3 directionNormalize, float distance)
  {
    Vector3 desiredPosition = CameraSettings.Focus.CurrentPosition + directionNormalize * distance;

    CameraSettings.Focus.SpeedMove = SPEED_MOVE;
    CameraSettings.Focus.SpeedChangePosition = SPEED_CHANGE_POSITION;
    CameraSettings.Focus.DesiredPosition = desiredPosition;
    CameraSettings.LerpMove = true;
  }


  [Inject]
  public CameraSettings CameraSettings { get; private set; }
}