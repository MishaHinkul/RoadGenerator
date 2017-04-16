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

    //Debug
    E_Dubug_ShowPath
}
