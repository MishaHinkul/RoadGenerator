using UnityEngine;
using System.Collections;

public enum EventGlobal
{
    None,
    E_AppStart,
    E_AppUpdate,
    E_AppFixedUpdate,
    E_AppLateUpdate,

    E_Slot_PointerDrag,
    E_Slot_PointerEnter,
    E_Slot_PointerExit,
    E_Slot_PointerUp,

    //Inventory
    E_Inventory_EquepAsset,
    E_Inventory_DequepAsset
}
