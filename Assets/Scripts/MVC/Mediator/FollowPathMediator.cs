using strange.extensions.mediation.impl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPathMediator : EventMediator
{
    [Inject]
    public PathFollowerView view { get; set; }

    public override void OnRegister()
    {
        view.LoadView();
    }

    public override void OnRemove()
    {
        view.RemoveView();
    }
}
