using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadGraphCommand : BaseCommand
{
    [Inject]
    public GraphModel graphModel { get; private set; }
    public override void Execute()
    {
        GraphVisiЬility graph = GameObject.FindObjectOfType<GraphVisiЬility>();
        if (graph != null)
        {
            graph.Load();
            graphModel.graph = graph;
        }
    }
}