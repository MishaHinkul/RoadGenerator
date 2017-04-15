using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphVisiЬility : Graph
{
    /// <summary>
    /// Определяем связи между вершиными
    /// </summary>
    public override void Load()
    {
        Vertex[] verts = GameObject.FindObjectsOfType<Vertex>();
        vertices = new List<Vertex>(verts);

        for (int i = 0; i < vertices.Count; i++)
        {
            VertexVisibility vV = vertices[i] as VertexVisibility;
            vV.id = i;
            vV.FindNeighbours(vertices);
        }
    }

    public override Vertex GetNearestVertex(Vector3 position)
    {
        Vertex vertex = null;
        float dist = Mathf.Infinity;
        float distNear = dist;
        Vector3 posVertex = Vector3.zero;
        for (int i = 0; i < vertices.Count; i++)
        {
            posVertex = vertices[i].transform.position;
            dist = Vector3.Distance(position, posVertex);
            if (dist < distNear)
            {
                distNear = dist;
                vertex = vertices[i];
            }
        }
        return vertex;
    }


    public override Vertex[] GetNeighbours(Vertex v)
    {
        List<Edge> edges = v.neighbours;
        Vertex[] ns = new Vertex[edges.Count];
        for (int i = 0; i < edges.Count; i++)
        {
            ns[i] = edges[i].vertex;
        }
        return ns;
    }
}
