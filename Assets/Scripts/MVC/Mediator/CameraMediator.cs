using strange.extensions.mediation.impl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMediator : EventMediator
{
    [Inject]
    public CameraView view { get; set; }

    public override void OnRegister()
    {
        dispatcher.AddListener(EventGlobal.E_CameraUpdateSettings, OnUpdateCameraSettings);
        view.LoadView();
    }

    public override void OnRemove()
    {
        dispatcher.RemoveListener(EventGlobal.E_CameraUpdateSettings, OnUpdateCameraSettings);
        view.RemoveView();
    }

    public void OnUpdateCameraSettings()
    {
        view.OnUpdateCameraSettings();
    }
}
