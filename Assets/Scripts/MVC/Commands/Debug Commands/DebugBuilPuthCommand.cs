using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugBuilPuthCommand : BaseCommand
{
    [Inject]
    public GraphModel gragpModel { get; private set; }

    [Inject]
    public RoadNetworkModel roadNetwork { get; private set; }

    [Inject]
    public TreeEntryModel entryMode { get; private set; }

    public override void Execute()
    {
        LineRenderer line = GameObject.FindObjectOfType<LineRenderer>();

        //Vector3 begin = new Vector3(roadNetwork.roadSegments[0].PointA.point.x, roadNetwork.roadNetworkTransform.position.y, roadNetwork.roadSegments[0].PointA.point.y);
        //Vector3 end = new Vector3(roadNetwork.roadSegments[roadNetwork.roadSegments.Count - 1].PointB.point.x, 
        //                            roadNetwork.roadNetworkTransform.position.y,
        //                            roadNetwork.roadSegments[roadNetwork.roadSegments.Count - 1].PointB.point.y);


        List<Vertex> path = eventData.data as List<Vertex>;

        line.numPositions = path.Count;

        for (int i = 0; i < line.numPositions; i++)
        {
            line.SetPosition(i, (path[i] as VertexVisibility).transform.position);
        }
    }
}