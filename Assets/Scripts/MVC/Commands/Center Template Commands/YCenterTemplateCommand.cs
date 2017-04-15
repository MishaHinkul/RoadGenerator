using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Базовая конфигурация сегментов в виде Игрика
/// </summary>
public class YCenterTemplateCommand : BaseCommand
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

        Quaternion localA = Quaternion.Euler(0, 0, 120f);
        RoadPoint c = new RoadPoint();
        c.point = rotation * model.Center;
        RoadPoint d = new RoadPoint();
        d.point = localA * rotation * (new Vector2(networkModel.Scale, 0) + model.Center);

        Quaternion localB = Quaternion.Euler(0, 0, 240f);
        RoadPoint e = new RoadPoint();
        e.point = rotation * model.Center;
        RoadPoint f = new RoadPoint();
        f.point = localB * rotation * (new Vector2(networkModel.Scale, 0) + model.Center);

        RoadSegment rA = new RoadSegment(a, b, 0);
        RoadSegment rB = new RoadSegment(c, d, 0);
        RoadSegment rC = new RoadSegment(e, f, 0);

        networkModel.roadSegments.AddRange(new RoadSegment[] { rA, rB, rC });

        Intersection iA = new Intersection(new List<RoadPoint>() { rA.PointA, rB.PointA, rC.PointA });
        networkModel.RoadIntersections.Add(iA);
    }
}