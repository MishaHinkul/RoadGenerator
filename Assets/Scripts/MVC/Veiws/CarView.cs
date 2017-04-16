using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarView : BaseView
{
    //public CarModel model { get; set; }

    public PathFollowerView followPath { get; private set; }
    public void LoadView()
    {
        followPath = GetComponent<PathFollowerView>();
    }

    public void RemoveView()
    {

    }

    /// <summary>
    /// Начать движение по указаному пути
    /// </summary>
    public void StarMove(Path path, System.Action callbackFinished = null)
    {
        if (path == null || followPath == null)
        {
            return;
        }
        dispatcher.Dispatch(EventGlobal.E_Dubug_ShowPath, path.nodes);
        followPath.StartMove(path, callbackFinished);
    }

}