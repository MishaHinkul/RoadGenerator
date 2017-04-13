using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Базовая конфигурация сегментов в виде Икса
/// </summary>
public class XCenterTemplateCommand : BaseCommand
{
    [Inject]
    public RoadNetworkModel networkModel { get; private set; }

    public override void Execute()
    {
        CenterTemplateModel model = eventData.data as CenterTemplateModel;

        if (model == null)
        {
            return;
        }

        Quaternion rotation = Quaternion.Euler(0, 0, model.Angle);

        RoadPoint a = new RoadPoint();
        a.point = rotation * model.Center;
        RoadPoint b = new RoadPoint();
        b.point = rotation * (new Vector2(networkModel.Scale, 0) + model.Center);

        RoadPoint c = new RoadPoint();
        c.point = rotation * model.Center;
        RoadPoint d = new RoadPoint();
        d.point = rotation * (new Vector2(0, networkModel.Scale) + model.Center);

        RoadPoint e = new RoadPoint();
        e.point = rotation * model.Center;
        RoadPoint f = new RoadPoint();
        f.point = rotation * (new Vector2(-networkModel.Scale, 0) + model.Center);

        RoadPoint g = new RoadPoint();
        g.point = rotation * model.Center;
        RoadPoint h = new RoadPoint();
        h.point = rotation * (new Vector2(0, -networkModel.Scale) + model.Center);

        RoadSegment rA = new RoadSegment(a, b, 0);
        RoadSegment rB = new RoadSegment(c, d, 0);
        RoadSegment rC = new RoadSegment(e, f, 0);
        RoadSegment rD = new RoadSegment(g, h, 0);

        networkModel.roadSegments.AddRange(new RoadSegment[] { rA, rB, rC, rD });

        Intersection iA = new Intersection(new List<RoadPoint>() { rA.PointA, rB.PointA, rC.PointA, rD.PointA });
        networkModel.roadIntersections.Add(iA);
    }
}
