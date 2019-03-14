using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableNavigationColliderCommand : BaseCommand
{
    [Inject]
    public GraphModel graphModel { get; private set; }
    public override void Execute()
    {
        for (int i = 0; i < graphModel.Graph.vertices.Count; i++)
        {
            if (graphModel.Graph.vertices[i] != null)
            {
                Collider collider = graphModel.Graph.vertices[i].GetComponent<Collider>();
                if (collider != null)
                {
                    collider.enabled = false;
                }
            }
        }
    }
}
