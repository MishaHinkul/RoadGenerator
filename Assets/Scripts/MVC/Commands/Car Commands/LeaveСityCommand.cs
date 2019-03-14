using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Покинуть город через один из вьездов. (На случай зомби апокалипсиса)
/// </summary>
public class LeaveСityCommand : BaseCommand
{
    [Inject]
    public TreeEntryModel entryModel { get; private set; }

    [Inject]
    public GraphModel graphModel { get; private set; }

    public override void Execute()
    {
        CarView model = eventData.data as CarView;
        if (model == null)
        {
            return;
        }

        Vector3 carPosition = model.gameObject.transform.position;

        int entryIndex = Random.Range(0, entryModel.Entrances.Count);
        Vector3 entryPosition = entryModel.Entrances[entryIndex];

        List<Vertex> pathVertex = graphModel.Graph.GetPathAstart(entryPosition, carPosition);
        Path path = new Path(pathVertex);
        model.StarMove(path, () =>
        {
            //По достижению конца пути
            GameObject.Destroy(model.gameObject);
        });
    }
}