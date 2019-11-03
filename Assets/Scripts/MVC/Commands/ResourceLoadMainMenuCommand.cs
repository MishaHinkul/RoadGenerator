using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceLoadMainMenuCommand : BaseCommand
{
  public override void Execute()
  {
    GameObject mainMenu = Resources.Load<GameObject>("UI/Menu");
    if (mainMenu == null)
    {
      Debug.LogError("Main Menu - not found");
    }
    GameObject.Instantiate<GameObject>(mainMenu);
  }
}