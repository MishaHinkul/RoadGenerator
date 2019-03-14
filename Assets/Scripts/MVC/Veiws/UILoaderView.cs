using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class UILoaderView : BaseView
{
  public void LoadView()
  {
    DontDestroyOnLoad(gameObject);
  }

  public void RemoveView()
  {
  }
}