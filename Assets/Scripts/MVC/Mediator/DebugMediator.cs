using strange.extensions.mediation.impl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugMediator : EventMediator
{
  public override void PreRegister()
  {
  }

  public override void OnRegister()
  {
    DebugView.LoadView();
  }

  public override void OnRemove()
  {
    DebugView.RemoveView();
  }


  [Inject]
  public DebugView DebugView { get; set; }
}