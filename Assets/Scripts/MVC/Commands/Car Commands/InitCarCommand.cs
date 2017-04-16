using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitCarCommand : BaseCommand
{
    [Inject]
    public TreeEntryModel entryModel { get; private set; }

    [Inject]
    public PopulationsModel populationModel { get; private set; }

    [Inject]
    public GraphModel graphModel { get; private set; }

    public override void Execute()
    {
        GameObject model = eventData.data as GameObject;
        if (model == null)
        {
            return;
        }
        PathFollowerView follwPath = model.gameObject.GetComponent<PathFollowerView>();
        if (follwPath == null)
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
        Debug.Log(pathVertex[pathVertex.Count - 1].transform.position);
        dispatcher.Dispatch(EventGlobal.E_Dubug_ShowPath, pathVertex);
        Path path = new Path(pathVertex);
        follwPath.StartMove(path);
    }
}