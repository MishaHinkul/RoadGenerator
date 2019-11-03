using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadGraphCommand : BaseCommand
{
  public override void Execute()
  {
    GraphVisiЬility graph = GameObject.FindObjectOfType<GraphVisiЬility>();
    if (graph != null)
    {
      graph.Load();
      GraphModel.Graph = graph;
    }
  }

  [Inject]
  public GraphModel GraphModel { get; private set; }
}