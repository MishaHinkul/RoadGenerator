using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarLogicCommand : BaseCommand
{
  private CarView carView = null;

  public override void Execute()
  {
    GameObject model = eventData.data as GameObject;
    carView = GetCarView(model);
    if (carView == null)
    {
      return;
    }

    Vector3 entryPosition = EntryModel.GetRendomeEntry();
    Vector3 gasStationPosition = PopulationModel.GetRendomeBuilding();
    Path path = GraphModel.CalculatePath(gasStationPosition, entryPosition);

    model.transform.position = entryPosition;
    carView.StarMove(path, CarFinished);
  }

  private CarView GetCarView(GameObject model)
  {
    if (model == null)
    {
      return null;
    }
    CarView carView = model.gameObject.GetComponent<CarView>();
    if (carView != null)
    {
      //Изначально меш выключен, для избежания мерцания. Так как
      // 1. Создаем префаб в сцене 
      // 2. Ждем конца кадра, чтобы стренж успел все запустить
      // 3. И только потом устанавливаем позицию на одном из вьъездом
      MeshRenderer mesh = carView.GetComponent<MeshRenderer>();
      if (mesh != null)
      {
        mesh.enabled = true;
      }
    }

    return carView;
  }

  private void CarFinished()
  {
    WaitTimeModel waitTimeModel = GetTimeModel();
    dispatcher.Dispatch(EventGlobal.E_WaitTime, waitTimeModel);
  }

  private WaitTimeModel GetTimeModel()
  {
    WaitTimeModel model = new WaitTimeModel();
    model.Time = SettingsModel.StopGassStationTime;
    model.Callback = DispathLeaveСity;

    return model;
  }

  private void DispathLeaveСity()
  {
    dispatcher.Dispatch(EventGlobal.E_LeaveСity, carView);
  }

  [Inject]
  public EntryModel EntryModel { get; private set; }

  [Inject]
  public SettingsModel SettingsModel { get; private set; }

  [Inject]
  public PopulationsModel PopulationModel { get; private set; }

  [Inject]
  public GraphModel GraphModel { get; private set; }
}