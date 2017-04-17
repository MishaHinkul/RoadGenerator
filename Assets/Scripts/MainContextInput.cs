using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using strange.extensions.command.api;
using strange.extensions.command.impl;
using strange.extensions.context.api;
using strange.extensions.context.impl;
using strange.extensions.mediation.api;
using strange.extensions.mediation.impl;
using strange.extensions.dispatcher.eventdispatcher.impl;
using strange.extensions.dispatcher.eventdispatcher.api;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System;

public class MainContextInput : MainContextRoot
{
    public MainContextInput(MonoBehaviour contextView) : base(contextView)
    {
    }

    public override IContext Start()
	{
		IContext c =  base.Start();
		return c;
	}

	public void Update()
	{
		if(dispatcher != null)
		{
			UpdateInput();
			dispatcher.Dispatch(EventGlobal.E_AppUpdate, Time.deltaTime);
		}
		else
		{
			Debug.LogError("Update ERROR!!! dispatcher == null");
		}
	}

	public void FixedUpdate()
	{
		if(dispatcher != null)
		{
			dispatcher.Dispatch(EventGlobal.E_AppFixedUpdate, Time.deltaTime);
		}
		else
		{
            Debug.LogError("FixedUpdate ERROR!!! dispatcher == null");
		}
	}

	public void LateUpdate()
	{
		if(dispatcher != null)
		{
			dispatcher.Dispatch(EventGlobal.E_AppLateUpdate, Time.deltaTime);
		}
		else
		{
            Debug.LogError("LateUpdate ERROR!!! dispatcher == null");
		}
	}

    public void OnApplicationFocus(bool focus)
	{
		if(!focus)
		{         
		}
	}
  
    /// <summary>
    /// Обработка ввода с всей игры
    /// </summary>
	public void UpdateInput()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit mouseHit;
            if (Physics.Raycast(ray, out mouseHit))
            {
                dispatcher.Dispatch(EventGlobal.E_CameraMove, mouseHit.point);
            }
        }
        dispatcher.Dispatch(EventGlobal.E_CameraScale, Input.GetAxis("Mouse ScrollWheel"));
    }
}