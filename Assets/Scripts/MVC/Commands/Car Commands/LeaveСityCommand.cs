using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Покинуть город через один из вьездов. (На случай зомби апокалипсиса)
/// </summary>
public class LeaveСityCommand : BaseCommand
{
  private CarView carView = null;

  public override void Execute()
  {
    carView = eventData.data as CarView;
    if (carView == null)
    {
      return;
    }

    Vector3 carPosition = carView.gameObject.transform.position;
    Vector3 entryPosition = EmptryMode.GetRendomeEntry();
    Path path = GraphModel.CalculatePath(entryPosition, carPosition);

    carView.StarMove(path, DestroyCar);
  }

  private void DestroyCar()
  {
    GameObject.Destroy(carView.gameObject);
  }

  [Inject]
  public EntryModel EmptryMode { get; private set; }

  [Inject]
  public GraphModel GraphModel { get; private set; }
}