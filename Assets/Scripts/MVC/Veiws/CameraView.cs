using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraView : BaseView
{
  private Transform focus = null;
  private Camera cameraCache = null;
  private Vector3 moveVel;
  private float distanceVel;
  private bool lerpMove = false;
  private bool lerpScale = false;
  private Vector3[] screenPoints = null;   //Cosntraint


  internal void LoadView()
  {
    cameraCache = GetComponent<Camera>();
    focus = transform.parent;

    cameraSettings.CameraObj = cameraCache;
    cameraSettings.Focus.CurrentPosition = focus.position;
    cameraSettings.Distance.CurrentDistance = cameraCache.orthographicSize;
    cameraSettings.Distance.DesiredDistance = cameraCache.orthographicSize;

    screenPoints = new Vector3[]
    {
      //Порядок должен быть точно такой как у массива констрэинтов
      new Vector3(Screen.width, Screen.height, 0), //RightTop
      new Vector3(0, Screen.height, 0), //LeftTop
      new Vector3(Screen.width, 0, 0), //RightBottom
      Vector3.zero, //LeftBottom
    };
  }

  internal void RemoveView()
  {
  }

  private void LateUpdate()
  {
    if (cameraCache == null || focus == null)
    {
      return;
    }

    Move();
    Scale();
  }

  private void Move()
  {
    if (!lerpMove)
    {
      return;
    }
    if (DefaultStopMoveLerp())
    {
      LerpMove();
    }
    else
    {
      lerpMove = false;
    }
  }

  //Методы применимые по умолчанию, определяющие остановку Lerp
  private bool DefaultStopMoveLerp()
  {
    return Vector3.Distance(cameraSettings.Focus.CurrentPosition, cameraSettings.Focus.DesiredPosition) > cameraSettings.Focus.StopLerpDistance;
  }

  /// Сглаживание данных камеры между текущими и желаемыми
  private void LerpMove()
  {
    cameraSettings.Focus.CurrentPosition = Vector3.SmoothDamp(cameraSettings.Focus.CurrentPosition,
                                           cameraSettings.Focus.DesiredPosition,
                                           ref moveVel,
                                           cameraSettings.Focus.SpeedChangePosition);
    if (ConstraintMapRange())
    {
      Vector3 currentDirection = cameraSettings.Focus.DesiredPosition - cameraSettings.Focus.CurrentPosition;
      currentDirection.Normalize();
      cameraSettings.Constraint.IsConstraint = true;
      cameraSettings.Constraint.Direction = currentDirection;
    }
    else
    {
      cameraSettings.Constraint.IsConstraint = false;
    }
    focus.position = cameraSettings.Focus.CurrentPosition;

  }

  private void Scale()
  {
    if (!lerpScale)
    {
      return;
    }
    if (DefaultStopScaleLerp())
    {
      LerpScale();
    }
    else
    {
      lerpScale = false;
    }
  }

  private bool DefaultStopScaleLerp()
  {
    return Mathf.Abs(cameraSettings.Distance.DesiredDistance - cameraSettings.Distance.CurrentDistance) > cameraSettings.Distance.StopLerpDistance;
  }

  private void LerpScale()
  {
    cameraSettings.Distance.CurrentDistance = Mathf.SmoothDamp(cameraSettings.Distance.CurrentDistance,
                                              cameraSettings.Distance.DesiredDistance,
                                              ref distanceVel,
                                              cameraSettings.Distance.SpeedChangeDistance); // сглаживание смены дистанции
    cameraCache.orthographicSize = cameraSettings.Distance.CurrentDistance;
  }

  private bool ConstraintMapRange()
  {
    Ray ray;
    RaycastHit hit;
    for (int i = 0; i < screenPoints.Length; i++)
    {
      ray = cameraSettings.CameraObj.ScreenPointToRay(screenPoints[i]);
      if (Physics.Raycast(ray, out hit))
      {
        //Если выходим за рамки карты
        if (hit.point.sqrMagnitude > cameraSettings.Constraint.ConstraintArr[0].sqrMagnitude)
        {
          return true;
        }
      }
    }

    return false;
  }

  public void OnUpdateCameraSettings()
  {
    focus.position = cameraSettings.Focus.CurrentPosition;
    lerpMove = cameraSettings.LerpMove;
    lerpScale = cameraSettings.LerpScale;
  }


  [Inject]
  public CameraSettings cameraSettings { get; private set; }
}