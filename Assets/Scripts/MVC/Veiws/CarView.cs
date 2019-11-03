using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarView : BaseView
{
  public void LoadView()
  {
    FollowPath = GetComponent<PathFollowerView>();
  }

  public void RemoveView()
  {
  }

  public void StarMove(Path path, System.Action callbackFinished = null)
  {
    if (path == null || FollowPath == null)
    {
      return;
    }
    FollowPath.StartMove(path, callbackFinished);
  }


  public PathFollowerView FollowPath { get; private set; }
}