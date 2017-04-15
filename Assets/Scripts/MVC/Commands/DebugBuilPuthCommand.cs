using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugBuilPuthCommand : BaseCommand
{
    [Inject]
    public GraphModel gragpModel { get; private set; }

    [Inject]
    public RoadNetworkModel roadNetwork { get; private set; }

    public override void Execute()
    {
        LineRenderer line = GameObject.FindObjectOfType<LineRenderer>();

        //Vector3 begin = new Vector3(roadNetwork.roadSegments[0].PointA.point.x, roadNetwork.roadNetworkTransform.position.y, roadNetwork.roadSegments[0].PointA.point.y);
        //Vector3 end = new Vector3(roadNetwork.roadSegments[roadNetwork.roadSegments.Count - 1].PointB.point.x, 
        //                            roadNetwork.roadNetworkTransform.position.y,
        //                            roadNetwork.roadSegments[roadNetwork.roadSegments.Count - 1].PointB.point.y);

        Vector3 begin = new Vector3(17, 0, 1);
        Vector3 end = new Vector3(-9, 0, -9);
        List<Vertex> path =  gragpModel.graph.GetPathAstart(begin, end);

        line.numPositions = path.Count;

        for (int i = 0; i < line.numPositions; i++)
        {
            line.SetPosition(i, (path[i] as VertexVisibility).transform.position);
        }


    }
}