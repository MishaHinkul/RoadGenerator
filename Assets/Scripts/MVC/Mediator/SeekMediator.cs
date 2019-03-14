using strange.extensions.mediation.impl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekMediator : EventMediator
{
  public override void OnRegister()
  {
    SeeView.LoadView();
  }

  public override void OnRemove()
  {
    SeeView.RemoveView();
  }


  [Inject]
  public SeekView SeeView { get; set; }
}