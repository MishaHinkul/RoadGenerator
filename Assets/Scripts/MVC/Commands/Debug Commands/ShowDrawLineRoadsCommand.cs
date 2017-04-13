using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowDrawLineRoadsCommand : BaseCommand
{
    [Inject]
    public RoadNetworkModel networkModel { get; private set; }

    [Inject]
    public ICoroutineExecutor coroutineExecutor { get; private set; }


    public override void Execute()
    {
        coroutineExecutor.StartCoroutine(ShowLine());
    }

    private IEnumerator ShowLine()
    {
        while(true)
        {
            foreach (RoadSegment segment in networkModel.roadSegments)
            {
                segment.DebugDriwLine();
            }
            yield return null;
        }
    }
}