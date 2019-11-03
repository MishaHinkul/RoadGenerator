using strange.extensions.mediation.impl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentMediator : EventMediator
{
  public override void OnRegister()
  {
    AgentView.LoadView();
  }

  public override void OnRemove()
  {
    AgentView.RemoveView();
  }


  [Inject]
  public AgentView AgentView { get; set; }
}