using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSettings 
{
    public CameraFocusPositionSettigs focus;
    public CameraScaleSettings distance;
    public CameraAngleSettings cameraAngle;
    public CameraConstraint constraint;

    public bool lerpMove = false;
    public bool lerpScale = false;

    public Camera cameraObj = null;

    public CameraSettings()
    {
        constraint = new CameraConstraint();
    }

}




[System.Serializable]
public struct CameraFocusPositionSettigs
{
    [Header("Focus Motion:")]
    /// <summary>
    /// Скорость движение
    /// </summary>
    public float speedMove;

    /// <summary>
    /// Скорость с которой изменяется позиция
    /// </summary>
    public float speedChangePosition;

    public Vector3 desiredPosition;

    public Vector3 currentPosition;

    public float stopLerpDistance;
}

[System.Serializable]
public struct CameraScaleSettings
{
    [Header("Camera Distance:")]
    /// <summary>
    /// Желаемая дистанция
    /// </summary>
    public float desiredDistance;

    /// <summary>
    /// Минимальная и максимальная допустимые дистанции
    /// </summary>
    public Vector2 min_max_Distance;

    /// <summary>
    /// Скорость с которой изменяется дистанция камеры
    /// </summary>
    public float speedChangeDistance;

    public float currentDistance;

    public float stopLerpDistance;

    public Vector3 localPosition;
}


/// <summary>
/// Структура, хранящяя настройки угла камеры
/// </summary>
[System.Serializable]
public struct CameraAngleSettings
{
    [Header("Camera Angle:")]
    /// <summary>
    /// Желаемый угл наклона камеры
    /// </summary>
    public Vector3 desiredAngle;

    /// <summary>
    /// Минимальная и максимальная угл камеры
    /// </summary>
    public Vector2 min_max_Angle;

    public Vector3 currentAngle;

    /// <summary>
    /// Скорость с которой изменяется угол камеры
    /// </summary>
    public float speedChangeAngle;

    public float stopLerpAngle;

}

public class CameraConstraint
{
    public Vector3 constraintTopLeft;
    public Vector3 constraintTopRight;

    public Vector3 constraintBottomLeft;
    public Vector3 constraintBottomRight;

    public Vector3[] constraintArr;

    public CameraConstraint()
    {
        constraintArr = new Vector3[4];
    }

    public void SetArr()
    {
        constraintArr[0] = constraintTopRight;
        constraintArr[1] = constraintTopLeft;
        constraintArr[2] = constraintBottomRight;
        constraintArr[3] = constraintBottomLeft;
    }

    /// <summary>
    /// Направление в которой быыла заблокирована камера
    /// </summary>
    public Vector3 direction;

    /// <summary>
    /// Заблокирована ли камера
    /// </summary>
    public bool isConstraint;
}