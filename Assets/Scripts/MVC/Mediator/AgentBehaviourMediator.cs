using strange.extensions.mediation.impl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentBehaviourMediator : EventMediator
{
  public override void OnRegister()
  {
    BehaviourView.LoadView();
  }

  public override void OnRemove()
  {
    BehaviourView.RemoveView();
  }

  [Inject]
  public AgentBehaviurView BehaviourView { get; set; }
}