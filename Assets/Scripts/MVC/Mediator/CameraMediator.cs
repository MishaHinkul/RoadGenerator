using strange.extensions.mediation.impl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMediator : EventMediator
{
  public override void OnRegister()
  {
    dispatcher.AddListener(EventGlobal.E_CameraUpdateSettings, OnUpdateCameraSettings);
    CameraView.LoadView();
  }

  public override void OnRemove()
  {
    dispatcher.RemoveListener(EventGlobal.E_CameraUpdateSettings, OnUpdateCameraSettings);
    CameraView.RemoveView();
  }

  public void OnUpdateCameraSettings()
  {
    CameraView.OnUpdateCameraSettings();
  }


  [Inject]
  public CameraView CameraView { get; set; }
}