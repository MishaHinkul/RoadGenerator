using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableNavigationColliderCommand : BaseCommand
{
  public override void Execute()
  {
    for (int i = 0; i < GraphModel.Graph.vertices.Count; i++)
    {
      DisableCollider(GraphModel.Graph.vertices[i]);
    }
  }

  private void DisableCollider(Vertex vertex)
  {
    if (vertex == null)
    {
      return;
    }
    Collider collider = vertex.GetComponent<Collider>();
    if (collider != null)
    {
      collider.enabled = false;
    }
  }

  [Inject]
  public GraphModel GraphModel { get; private set; }
}