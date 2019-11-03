using strange.extensions.mediation.impl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuMediator : EventMediator
{
  public override void OnRegister()
  {
    MenuView.LoadView();
  }

  public override void OnRemove()
  {
    MenuView.RemoveView();
  }


  [Inject]
  public MainMenuView MenuView { get; set; }
}