using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;

public class UILoaderMediator : EventMediator
{
  public override void PreRegister()
  {
  }

  public override void OnRegister()
  {
    ViewLoader.LoadView();
  }

  public override void OnRemove()
  {
    ViewLoader.RemoveView();
  }


  [Inject]
  public UILoaderView ViewLoader { get; set; }
}