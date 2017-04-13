using strange.extensions.mediation.impl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugMediator : EventMediator
{
    [Inject]
    public DebugView view { get; set; }

    public override void PreRegister()
    {

    }
    public override void OnRegister()
    {
        view.LoadView();
    }

    public override void OnRemove()
    {
        view.RemoveView();
    }

}
