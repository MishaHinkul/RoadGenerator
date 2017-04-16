using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableNavigationColliderCommand : BaseCommand
{
    [Inject]
    public GraphModel graphModel { get; private set; }
    public override void Execute()
    {
        for (int i = 0; i < graphModel.graph.vertices.Count; i++)
        {
            if (graphModel.graph.vertices[i] != null)
            {
                Collider collider = graphModel.graph.vertices[i].GetComponent<Collider>();
                if (collider != null)
                {
                    collider.enabled = false;
                }
            }
        }
    }
}
