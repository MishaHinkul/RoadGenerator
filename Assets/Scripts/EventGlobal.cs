using UnityEngine;
using System.Collections;

public enum EventGlobal
{
    None,
    E_AppStart,
    E_AppUpdate,
    E_AppFixedUpdate,
    E_AppLateUpdate,

    //Roads
    E_GeneradeRoads,
    E_SplitSegmentForLevel,
    E_SplitSegment,
    E_SetTemplate,

    //Car
    E_InitCar,

    //Debug
    E_Dubug_ShowPath
}
