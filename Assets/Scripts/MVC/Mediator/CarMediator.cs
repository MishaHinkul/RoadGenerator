using strange.extensions.mediation.impl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMediator : EventMediator
{
  public override void OnRegister()
  {
    CarView.LoadView();
  }

  public override void OnRemove()
  {
    CarView.RemoveView();
  }


  [Inject]
  public CarView CarView { get; set; }
}