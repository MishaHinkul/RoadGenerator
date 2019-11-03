using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugView : BaseView
{
  [SerializeField]
  private Button generadeButton;


  internal void LoadView()
  {
    generadeButton.onClick.RemoveAllListeners();
    generadeButton.onClick.AddListener(DispatchGeneradeRoads);
  }

  internal void RemoveView()
  {
    generadeButton.onClick.RemoveAllListeners();
  }

  private void DispatchGeneradeRoads()
  {
    dispatcher.Dispatch(EventGlobal.E_GeneradeRoads);
  }
}