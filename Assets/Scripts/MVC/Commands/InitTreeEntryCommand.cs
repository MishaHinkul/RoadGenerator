using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Устанавливаем 3 случайных вьезда в город
/// </summary>
public class InitTreeEntryCommand : BaseCommand
{
    [Inject]
    public RoadNetworkModel networkMode { get; private set; }

    [Inject]
    public TreeEntryModel entryModel { get; private set; }

    public override void Execute()
    {
        //Первое пересечение это из которого строится вся карта
        Intersection mainIntersection = networkMode.RoadIntersections[0];
        int entryCount = 3;
        for (int i = 0; i < entryCount; i++)
        {        
            bool addEntry = false;
            do
            {
                int randomIndex = Random.Range(0, mainIntersection.Points.Count);
                RoadPoint pointA = mainIntersection.Points[randomIndex];
                RoadPoint pointB = pointA.mySegement.GetOther(pointA);
                Vector3 position = new Vector3(pointB.point.x, networkMode.RoadNetworkTransform.position.y, pointB.point.y);
                if (!entryModel.Entrances.Contains(position))
                {
                    entryModel.Entrances.Add(position);
                    addEntry = true;
                }   
            }
            while (!addEntry);
        }      
    }
}