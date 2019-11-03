using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowDrawLineRoadsCommand : BaseCommand
{
    [Inject]
    public RoadNetworkModel networkModel { get; private set; }

    [Inject]
    public ICoroutineExecutor coroutineExecutor { get; private set; }

    [Inject]
    public CameraSettings cameraSettingsModel { get; private set; }


    public override void Execute()
    {
        coroutineExecutor.StartCoroutine(ShowLine());
    }

    private IEnumerator ShowLine()
    {
        while(true)
        {
            Debug.DrawLine(cameraSettingsModel.Constraint.ConstraintTopLeft, cameraSettingsModel.Constraint.ConstraintTopRight, Color.red);
            Debug.DrawLine(cameraSettingsModel.Constraint.ConstraintBottomLeft, cameraSettingsModel.Constraint.ConstraintBottomRight, Color.red);
            //foreach (RoadSegment segment in networkModel.roadSegments)
            //{
            //    segment.DebugDriwLine();
            //}
            yield return null;
        }
    }
}