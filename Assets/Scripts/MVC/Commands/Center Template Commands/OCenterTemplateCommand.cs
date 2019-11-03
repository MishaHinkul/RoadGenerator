using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Базовая конфигурация сегментов в виде Кольца
/// </summary>
public class OCenterTemplateCommand : BaseCommand
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

    model.Angle = 360f / 8f;
    RoadSegment last = null;
    RoadSegment first = null;
    for (int i = 0; i < 8; i++)
    {
      Quaternion rotationA = Quaternion.Euler(0, 0, model.Angle * i);
      Quaternion rotationB = Quaternion.Euler(0, 0, model.Angle * (i + 1));

      RoadPoint a = new RoadPoint();
      a.Point = rotationA * (new Vector2(networkModel.Scale / 2.5f, 0) + model.Center);
      RoadPoint b = new RoadPoint();
      b.Point = rotationB * (new Vector2(networkModel.Scale / 2.5f, 0) + model.Center);

      RoadSegment rA = new RoadSegment(a, b, 0);
      networkModel.SaveSegment(rA);
      if (first == null)
        first = rA;

      if (last != null)
      {
        Intersection iA = new Intersection(new List<RoadPoint>() { rA.Begin, last.Begin });
        networkModel.SaveIntersection(iA);
      }
      last = rA;
    }

    Intersection iB = new Intersection(new List<RoadPoint>() { first.Begin, last.Begin });
    networkModel.SaveIntersection(iB);
  }
}
