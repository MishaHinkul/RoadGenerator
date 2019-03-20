using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Устанавливаем 3 случайных вьезда в город
/// </summary>
public class InitTreeEntryCommand : BaseCommand
{
  private const int ENTRY_COUNT = 3;

  public override void Execute()
  {
    for (int i = 0; i < ENTRY_COUNT; i++)
    {
      SetEntry();
    }
  }

  private void SetEntry()
  {
    bool addEntry = false;
    Vector3 position;
    do
    {
      position = GetPosition();
      if (!EntryModel.Entrances.Contains(position))
      {
        EntryModel.Entrances.Add(position);
        addEntry = true;
      }
    }
    while (!addEntry);
  }

  private Vector3 GetPosition()
  {
    Intersection mainIntersection = NetworkMode.RoadIntersections[0]; //Первое пересечение, это из которого строится вся карта
    int randomIndex = Random.Range(0, mainIntersection.Points.Count);
    RoadPoint pointA = mainIntersection.Points[randomIndex];
    RoadPoint pointB = pointA.MySegement.GetOther(pointA);

    return new Vector3(pointB.Point.x, NetworkMode.RoadNetworkTransform.position.y, pointB.Point.y);
  }


  [Inject]
  public RoadNetworkModel NetworkMode { get; private set; }

  [Inject]
  public EntryModel EntryModel { get; private set; }
}