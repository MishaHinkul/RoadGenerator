using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneradeGasStationCommand : BaseCommand
{
  private const int GAS_STATION_COUNT = 3;

  private List<RoadSegment> gasSegment = new List<RoadSegment>();

  public override void Execute()
  {
    RoadSegment segment = null;
    GameObject build = null;

    for (int i = 0; i < GAS_STATION_COUNT; i++)
    {
      segment = SetGassStationSegment();
      build = BuildGassStation(segment);
      PopulationModel.Buildings.Add(build);
    }
  }

  private RoadSegment SetGassStationSegment()
  {
    int randomIndex = 0;
    bool addEntry = false;
    RoadSegment segment = null;
    do
    {
      randomIndex = Random.Range(0, NetworkModel.GetRoadSegmentCount());
      segment = NetworkModel.GetRoadSegment(randomIndex);
      if (!gasSegment.Contains(segment))
      {
        gasSegment.Add(segment);
        addEntry = true;
      }
    }
    while (!addEntry);

    return segment;
  }

  private GameObject BuildGassStation(RoadSegment segment)
  {
    Vector2 start = segment.Begin.Point;
    Vector2 end = segment.End.Point;
    Vector2 dir = (end - start).normalized;
    float distance = Vector2.Distance(start, end);

    float level = 2.0f - (segment.Level / 3f);//0,0.33,0.66,1
    float length = Random.Range(1.5f, 2f) * level;
    float height = Random.Range(3f, 6f) * level;

    Vector2 per = new Vector2(-dir.y, dir.x);
    Vector2 roadOffset = per.normalized * (NetworkModel.WithRoad * 1.5f + length);
    float factor = distance * 0.5f;
    float wight = distance * 0.3f;

    Vector2 tc = start + (dir * factor) + roadOffset;
    Vector3 center = new Vector3(tc.x, 0, tc.y);


    GameObject build = GameObject.CreatePrimitive(PrimitiveType.Cube);
    build.transform.position = center;
    build.name = "Gass Station";
    build.transform.localScale = new Vector3(factor, height, wight);

    return build;
  }


  [Inject]
  public RoadNetworkModel NetworkModel { get; private set; }

  [Inject]
  public PopulationsModel PopulationModel { get; private set; }
}