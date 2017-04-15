﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneradeGasStationCommand : BaseCommand
{
    [Inject]
    public RoadNetworkModel networkModel { get; private set; }

    [Inject]
    public PopulationsModel populationModel { get; private set; }

    public override void Execute()
    {
        int gasStationCpunt = 3;
        List<RoadSegment> gasSegment = new List<RoadSegment>();

        //Определяем случайные сегменты относительно которых будет создаваться заправка
        for (int i = 0; i < gasStationCpunt; i++)
        {
            bool addEntry = false;
            do
            {
                int randomIndex = Random.Range(0, networkModel.roadSegments.Count);
                RoadSegment segment = networkModel.roadSegments[randomIndex];
                if (!gasSegment.Contains(segment))
                {
                    gasSegment.Add(segment);
                    addEntry = true;
                }
            }
            while (!addEntry);
        }

        //Создаем заправочные станции
        
        for (int i = 0; i < gasSegment.Count; i++)
        {
            Vector2 start = gasSegment[i].PointA.point;
            Vector2 end = gasSegment[i].PointB.point;
            Vector2 dir = (end - start).normalized;
            float distance = Vector2.Distance(start, end);

            Vector2 current = start;

            float level = 2.0f - (gasSegment[i].Level / 3f);//0,0.33,0.66,1
            float width = Random.Range(1.75f, 2f) * level;
            float length = Random.Range(1.75f, 2f) * level;
            float height = Random.Range(2.5f, 10f) * level;

            Vector2 per = new Vector2(-dir.y, dir.x);
            Vector2 roadOffset = per.normalized * (networkModel.WithRoad * 1.25f + length);
            float factor = distance * 0.5f;

            Vector2 tc = start + (dir * factor) + roadOffset;
            Vector3 center = new Vector3(tc.x, 0, tc.y);
            GameObject build = GameObject.CreatePrimitive(PrimitiveType.Cube);
            if (build != null)
            {
                build.transform.position = center;
                build.name = "Gass Station " + i;
                build.transform.localScale = new Vector3(distance, build.transform.lossyScale.y, build.transform.lossyScale.z);

                populationModel.buildings.Add(build);
            }       
        }
    }
}