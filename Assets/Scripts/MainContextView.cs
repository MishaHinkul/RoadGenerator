using UnityEngine;
using System.Collections;
using strange.extensions.context.impl;
using strange.extensions.dispatcher.eventdispatcher.impl;
using strange.extensions.dispatcher.eventdispatcher.api;

public class MainContextView : strange.extensions.context.impl.ContextView
{
  public static bool isPauseDisable = false;
  public static MainContextView instance = null;
  public static IEventDispatcher strangeDispatcher = null;
  private bool isRunContext = false;

  private void Start()
  {
    GameObject.DontDestroyOnLoad(this);
    instance = this;
    isRunContext = false;
  }

  public static void DispatchStrangeEvent(object eventType)
  {
    if (strangeDispatcher == null && instance != null && instance.context != null)
    {
      if ((instance.context as MainContextInput).dispatcher != null)
      {
        strangeDispatcher = (instance.context as MainContextInput).dispatcher;
      }
    }

    if (strangeDispatcher != null)
    {
      strangeDispatcher.Dispatch(eventType);
    }
    else
    {
      Debug.LogError("strangeDispatcher Not Redy");
    }
  }

  public static void DispatchStrangeEvent(object eventType, object data)
  {
    if (strangeDispatcher == null && instance != null && instance.context != null)
    {
      if ((instance.context as MainContextInput).dispatcher != null)
      {
        strangeDispatcher = (instance.context as MainContextInput).dispatcher;
      }
    }

    if (strangeDispatcher != null)
    {
      strangeDispatcher.Dispatch(eventType, data);
    }
    else
    {
      Debug.LogError("strangeDispatcher Not Redy");
    }
  }

  /// Remove a previously registered observer with exactly one argument from this Dispatcher
  public static void AddListenerStrangeEvent(object evt, EventCallback callback)
  {
    if (strangeDispatcher == null && instance != null && instance.context != null)
    {
      if ((instance.context as MainContextInput).dispatcher != null)
      {
        strangeDispatcher = (instance.context as MainContextInput).dispatcher;
      }
    }

    if (strangeDispatcher != null)
    {
      strangeDispatcher.AddListener(evt, callback);
    }
    else
    {
      Debug.LogError("strangeDispatcher Not Redy");
    }
  }

  public static void AddListenerStrangeEvent(object evt, EmptyCallback callback)
  {
    if (strangeDispatcher == null && instance != null && instance.context != null)
    {
      if ((instance.context as MainContextInput).dispatcher != null)
      {
        strangeDispatcher = (instance.context as MainContextInput).dispatcher;
      }
    }

    if (strangeDispatcher != null)
    {
      strangeDispatcher.AddListener(evt, callback);
    }
    else
    {
      Debug.LogError("strangeDispatcher Not Redy");
    }
  }

  /// Remove a previously registered observer with exactly no arguments from this Dispatcher
  public static void RemoveListenerStrangeEvent(object evt, EmptyCallback callback)
  {
    if (strangeDispatcher == null && instance != null && instance.context != null)
    {
      if ((instance.context as MainContextInput).dispatcher != null)
      {
        strangeDispatcher = (instance.context as MainContextInput).dispatcher;
      }
    }

    if (strangeDispatcher != null)
    {
      strangeDispatcher.RemoveListener(evt, callback);
    }
    else
    {
      Debug.LogError("strangeDispatcher Not Redy");
    }
  }

  /// Remove a previously registered observer with exactly no arguments from this Dispatcher
  public static void RemoveListenerStrangeEvent(object evt, EventCallback callback)
  {
    if (strangeDispatcher == null && instance != null && instance.context != null)
    {
      if ((instance.context as MainContextInput).dispatcher != null)
      {
        strangeDispatcher = (instance.context as MainContextInput).dispatcher;
      }
    }

    if (strangeDispatcher != null)
    {
      strangeDispatcher.RemoveListener(evt, callback);
    }
    else
    {
      Debug.LogError("strangeDispatcher Not Redy");
    }
  }


  /// Returns true if the provided observer is already registered
  public static bool HasListenerStrangeEvent(object evt, EventCallback callback)
  {
    if (strangeDispatcher == null && instance != null && instance.context != null)
    {
      if ((instance.context as MainContextInput).dispatcher != null)
      {
        strangeDispatcher = (instance.context as MainContextInput).dispatcher;
      }
    }

    if (strangeDispatcher != null)
    {
      return strangeDispatcher.HasListener(evt, callback);
    }
    else
    {
      Debug.LogError("strangeDispatcher Not Redy");
    }
    return false;
  }

  private void Update()
  {

    if (!isRunContext)
    {
      isRunContext = true;
      MonoBehaviour view = this;
      if (view != null)
      {
        context = new MainContextInput(view);
        context.Start();
      }
      else
      {
        Debug.LogError("MonoBehaviour == NULL & MainContextInput == NULL! ERROR context Not Started");
      }
    }
    else
    {
      if (context != null)
      {
        (context as MainContextInput).Update();
        if (strangeDispatcher == null)
        {
          if ((context as MainContextInput).dispatcher != null)
          {
            strangeDispatcher = (context as MainContextInput).dispatcher;
          }
        }
      }
    }
  }

  private void FixedUpdate()
  {
    if (context != null)
    {
      (context as MainContextInput).FixedUpdate();
    }
  }

  private void LateUpdate()
  {
    if (context != null)
    {
      (context as MainContextInput).LateUpdate();
    }
  }

  private void OnApplicationPause(bool pauseStatus)
  {
    if (!isRunContext || instance == null || strangeDispatcher == null)
    {
      return;
    }
  }

  public void OnApplicationFocus(bool focus)
  {
    if (context != null)
    {
      (context as MainContextInput).OnApplicationFocus(focus);
    }
  }
}