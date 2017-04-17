using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraView : BaseView
{
    private Transform focus;
    private Camera cameraCache;

    private Vector3 moveVel;
    private float distanceVel;

    private bool lerpMove = false;
    private bool lerpScale = false;

    //Cosntraint
    private Vector3[] screenPoints;
   


    [Inject]
    public CameraSettings cameraSettings { get; private set; }


    internal void LoadView()
    {
        cameraCache = GetComponent<Camera>();
        focus = transform.parent;
        cameraSettings.cameraObj = cameraCache;
        cameraSettings.focus.currentPosition = focus.position;

        screenPoints = new Vector3[] 
        {
            //Порядок должен быть точно такой как у массива констреинтов
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
        if (cameraCache != null && focus != null)
        {

            #region Move
            if (lerpMove)
            {
                if (DefaultStopMoveLerp())
                {
                    LerpMove();
                }
                else
                {
                    lerpMove = false;
                }
            }
            #endregion

            #region Scale
            if (lerpScale)
            {
                if (DefaultStopScaleLerp())
                {
                    LerpScale();
                }
                else
                {
                    lerpScale = false;
                }
            }
            #endregion
        }
    }

    /// <summary>
    /// Обновить данный камеры
    /// </summary>
    public void OnUpdateCameraSettings()
    {
        focus.position = cameraSettings.focus.currentPosition;

        lerpMove = cameraSettings.lerpMove;
        lerpScale = cameraSettings.lerpScale;
    }

    //Методы применимые по умолчанию, определяющие остановку Lerp
    private bool DefaultStopMoveLerp()
    {
        return Vector3.Distance(cameraSettings.focus.currentPosition, cameraSettings.focus.desiredPosition) > cameraSettings.focus.stopLerpDistance;
    }

    private bool DefaultStopScaleLerp()
    {
        return Mathf.Abs(cameraSettings.distance.desiredDistance - cameraSettings.distance.currentDistance) > cameraSettings.distance.stopLerpDistance;
    }

    /// Сглаживание данных камеры между текущими и желаемыми
    private void LerpMove()
    {
        cameraSettings.focus.currentPosition = Vector3.SmoothDamp(cameraSettings.focus.currentPosition,
                                               cameraSettings.focus.desiredPosition,
                                               ref moveVel,
                                               cameraSettings.focus.speedChangePosition);
        if (Constraint())
        {
            Vector3 currentDirection = cameraSettings.focus.desiredPosition - cameraSettings.focus.currentPosition;
            currentDirection.Normalize();
            cameraSettings.constraint.isConstraint = true;
            cameraSettings.constraint.direction = currentDirection;
        }
        else
        {
            cameraSettings.constraint.isConstraint = false;
        }
        focus.position = cameraSettings.focus.currentPosition;
       
    }

    private void LerpScale()
    {
        cameraSettings.distance.currentDistance = Mathf.SmoothDamp(cameraSettings.distance.currentDistance,
                cameraSettings.distance.desiredDistance,
                ref distanceVel,
                cameraSettings.distance.speedChangeDistance); // сглаживание смены дистанции
    }

    /// <summary>
    /// Выходим ли за рамки карты 
    /// </summary>
    /// <returns></returns>
    private bool Constraint()
    {
        Ray ray;
        RaycastHit hit;
        for (int i = 0; i < screenPoints.Length; i++)
        {
            ray = cameraSettings.cameraObj.ScreenPointToRay(screenPoints[i]);
            if (Physics.Raycast(ray, out hit))
            {
                //Если выходим за рамки карты
                if (hit.point.sqrMagnitude > cameraSettings.constraint.constraintArr[0].sqrMagnitude)
                {
                    return true;
                }
            }
        }
        return false;
    }          
}