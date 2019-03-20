using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphModel
{
  public Path CalculatePath(Vector3 begin, Vector3 end)
  {
    List<Vertex> pathVertex = Graph.GetPathAstart(begin, end);
    return new Path(pathVertex);
  }


  public Graph Graph { get; set; }
}