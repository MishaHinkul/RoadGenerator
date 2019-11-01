using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Базовая конфигурация сегментов в виде Икса
/// </summary>
public class XCenterTemplateCommand : BaseCommand
{
  public override void Execute()
  {
    CenterTemplateModel model = eventData.data as CenterTemplateModel;
    if (model == null)
    {
      return;
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

    NetworkModel.RoadSegments.AddRange(new RoadSegment[] { rA, rB, rC, rD });

    Intersection i = new Intersection(new List<RoadPoint>() { rA.Begin, rB.Begin, rC.Begin, rD.Begin });
    NetworkModel.RoadIntersections.Add(i);

    //Show
    ShowSegmentnModel modelA = new ShowSegmentnModel(rA);
    ShowSegmentnModel modelB = new ShowSegmentnModel(rB);
    ShowSegmentnModel modelC = new ShowSegmentnModel(rC);
    ShowSegmentnModel modelD = new ShowSegmentnModel(rD);

    dispatcher.Dispatch(EventGlobal.E_ShowSegment, modelA);
    dispatcher.Dispatch(EventGlobal.E_ShowSegment, modelB);
    dispatcher.Dispatch(EventGlobal.E_ShowSegment, modelC);
    dispatcher.Dispatch(EventGlobal.E_ShowSegment, modelD);

    ShowIntersectionModel modelI = new ShowIntersectionModel(i);
    dispatcher.Dispatch(EventGlobal.E_ShowIntersection, modelI);
  }


  [Inject]
  public RoadNetworkModel NetworkModel { get; private set; }
}