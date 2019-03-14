using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSettings
{
  public CameraSettings()
  {
    Constraint = new CameraConstraint();
    Focus = new CameraFocusPositionSettigs();
    Distance = new CameraScaleSettings();
  }


  public CameraConstraint Constraint { get; set; }
  public CameraFocusPositionSettigs Focus { get; set; }
  public CameraScaleSettings Distance { get; set; }
  public bool LerpMove { get; set; }
  public bool LerpScale { get; set; }
  public Camera CameraObj { get; set; }
}



public class CameraFocusPositionSettigs
{
  public float SpeedMove { get; set; }
  public float SpeedChangePosition { get; set; }
  public float StopLerpDistance { get; set; }
  public Vector3 DesiredPosition { get; set; }
  public Vector3 CurrentPosition { get; set; }
}

public class CameraScaleSettings
{
  public float DesiredDistance { get; set; }
  public float SpeedChangeDistance { get; set; }
  public float Speed { get; set; }
  public float CurrentDistance { get; set; }
  public float StopLerpDistance { get; set; }
  public Vector2 MinMaxDistance { get; set; }
}

public class CameraConstraint
{
  public void SetArr()
  {
    ConstraintArr[0] = ConstraintTopRight;
    ConstraintArr[1] = ConstraintTopLeft;
    ConstraintArr[2] = ConstraintBottomRight;
    ConstraintArr[3] = ConstraintBottomLeft;
  }


  public CameraConstraint()
  {
    ConstraintArr = new Vector3[4];
  }

  public Vector3 ConstraintTopLeft { get; set; }
  public Vector3 ConstraintTopRight { get; set; }
  public Vector3 ConstraintBottomLeft { get; set; }
  public Vector3 ConstraintBottomRight { get; set; }
  public Vector3[] ConstraintArr { get; set; }

  /// <summary>
  /// Направление в которой быыла заблокирована камера
  /// </summary>
  public Vector3 direction;

  /// <summary>
  /// Заблокирована ли камера
  /// </summary>
  public bool isConstraint;
}