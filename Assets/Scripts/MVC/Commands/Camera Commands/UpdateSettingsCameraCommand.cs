using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateSettingsCameraCommand : BaseCommand
{
    [Inject]
    public CameraSettings cameraSettings { get; private set; }
    public override void Execute()
    {
        dispatcher.Dispatch(EventGlobal.E_CameraUpdateSettings);
    }
}