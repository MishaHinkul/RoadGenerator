using strange.extensions.mediation.impl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentBehaviourMediator : EventMediator
{
    [Inject]
    public AgentBehaviurView view { get; set; }

    public override void OnRegister()
    {
        view.LoadView();
    }

    public override void OnRemove()
    {
        view.RemoveView();
    }
}
