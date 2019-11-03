using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateSettingsCameraCommand : BaseCommand
{
  public override void Execute()
  {
    dispatcher.Dispatch(EventGlobal.E_CameraUpdateSettings);
  }
}