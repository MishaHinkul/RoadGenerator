using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Базовая конфигурация сегментов в виде Икса
/// </summary>
public class XCenterTemplateCommand : BaseCommand
{
  private CenterTemplateModel model = null;
  public override void Execute()
  {
     model = eventData.data as CenterTemplateModel;
    if (model == null)
    {
      model = GetDefaultMode();
    }

    Quaternion rotation = Quaternion.Euler(0, 0, model.Angle);

    RoadPoint a = new RoadPoint();
    a.Point = rotation * model.Center;
    RoadPoint b = new RoadPoint();
    b.Point = rotation * (new Vector2(NetworkModel.Scale, 0) + model.Center);

    RoadPoint c = new RoadPoint();
    c.Point = rotation * model.Center;
    RoadPoint d = new RoadPoint();
    d.Point = rotation * (new Vector2(0, NetworkModel.Scale) + model.Center);

    RoadPoint e = new RoadPoint();
    e.Point = rotation * model.Center;
    RoadPoint f = new RoadPoint();
    f.Point = rotation * (new Vector2(-NetworkModel.Scale, 0) + model.Center);

    RoadPoint g = new RoadPoint();
    g.Point = rotation * model.Center;
    RoadPoint h = new RoadPoint();
    h.Point = rotation * (new Vector2(0, -NetworkModel.Scale) + model.Center);

    RoadSegment rA = new RoadSegment(a, b, 0);
    RoadSegment rB = new RoadSegment(c, d, 0);
    RoadSegment rC = new RoadSegment(e, f, 0);
    RoadSegment rD = new RoadSegment(g, h, 0);

    Intersection i = new Intersection(new List<RoadPoint>() { rA.Begin, rB.Begin, rC.Begin, rD.Begin });

    NetworkModel.SaveIntersection(i);
    NetworkModel.SaveSegment(rA, rB, rC, rD);
  }

  private CenterTemplateModel GetDefaultMode()
  {
    return new CenterTemplateModel(Vector2.zero, 270);
  }

  [Inject]
  public RoadNetworkModel NetworkModel { get; private set; }
}