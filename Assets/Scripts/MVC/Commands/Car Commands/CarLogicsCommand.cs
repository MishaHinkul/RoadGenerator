using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarLogicCommand : BaseCommand
{
    [Inject]
    public TreeEntryModel entryModel { get; private set; }

    [Inject]
    public PopulationsModel populationModel { get; private set; }

    [Inject]
    public GraphModel graphModel { get; private set; }

    [Inject]
    public SettingsModel settingsModel { get; private set; }

    public override void Execute()
    {
        GameObject model = eventData.data as GameObject;
        if (model == null)
        {
            return;
        }
        CarView carView = model.gameObject.GetComponent<CarView>();
        if (carView == null)
        {
            return;
        }

        //Определяем ввъезд в город
        int indexEntry = Random.Range(0, entryModel.Entrances.Count);
        Vector3 entryPosition = entryModel.Entrances[indexEntry];
        model.transform.position = entryPosition;

        //К какой станции ехать
        int indexGasStation = Random.Range(0, populationModel.buildings.Count);
        Vector3 gasStationPosition = populationModel.buildings[indexGasStation].transform.position;

        List<Vertex> pathVertex = graphModel.graph.GetPathAstart(gasStationPosition, entryPosition);
        Path path = new Path(pathVertex);
        //Изначально выключен, для избежания мерцания. Так как
            // 1. Создаем префаб в сцене 
            // 2. Ждем конца кадра, чтобы стренж успел все запустить
            // 3. И только потом устанавливаем позицию на одном из вьъездом 
        MeshRenderer mesh = carView.GetComponent<MeshRenderer>();
        if (mesh != null)
        {
            mesh.enabled = true;
        }
        carView.StarMove(path, () => 
        {
            //По достижению конца пути
            WaitTimeModel waitTimeModel = new WaitTimeModel();
            waitTimeModel.time = settingsModel.stopGassStationTime;
            waitTimeModel.callback = () =>
            {
                //По окончанию ожидания 
                dispatcher.Dispatch(EventGlobal.E_LeaveСity, carView);
            };
            dispatcher.Dispatch(EventGlobal.E_WaitTime, waitTimeModel);
        });
    }
}