using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;

public class UILoaderMediator : EventMediator
{
	[Inject]
	public UILoaderView view { get; set; }

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
