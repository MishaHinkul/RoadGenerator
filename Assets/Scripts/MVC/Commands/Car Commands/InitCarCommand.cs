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

        //Определяем стартовую позицию
        int indexEntry = Random.Range(0, entryModel.Entrances.Count);
        Vector3 beginPosition = entryModel.Entrances[indexEntry];
        model.transform.position = beginPosition;

        //Определяем путь к одной из заправок
        int indexGasStation = Random.Range(0, populationModel.buildings.Count);
        Vector3 endPosition = populationModel.buildings[indexGasStation].transform.position;

        List<Vertex> pathVertex = graphModel.graph.GetPathAstart(beginPosition, endPosition);
        pathVertex.Reverse(); //Чтобы путь был от начала в конец
        Path path = new Path(pathVertex);
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