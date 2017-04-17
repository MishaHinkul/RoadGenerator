using UnityEngine;
using System.Collections;

public enum EventGlobal
{
    None,
    E_AppStart,
    E_AppUpdate,
    E_AppFixedUpdate,
    E_AppLateUpdate,

    E_WaitTime,

    //Roads
    E_GeneradeRoads,
    E_SplitSegmentForLevel,
    E_SplitSegment,
    E_SetTemplate,

    //Car
    E_CarLogics,
    E_LeaveСity,

    //Camera
    E_CameraMove,
    E_CameraScale,
    E_CameraUpdateSettings,

    //Debug
    E_Dubug_ShowPath
}
