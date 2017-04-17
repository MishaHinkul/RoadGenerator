using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Представляем игровой мир в виде точек видимости
/// </summary>
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
            VertexVisibility vertexVisibility = vertices[i] as VertexVisibility;
            vertexVisibility.id = i;
            vertexVisibility.FindNeighbours(vertices);
        }
    }

    public override Vertex GetNearestVertex(Vector3 position)
    {
        Vertex vertex = null;
        float distance = Mathf.Infinity;
        float distanceNear = distance;
        Vector3 posVertex = Vector3.zero;
        for (int i = 0; i < vertices.Count; i++)
        {
            posVertex = vertices[i].transform.position;
            distance = Vector3.Distance(position, posVertex);
            if (distance < distanceNear)
            {
                distanceNear = distance;
                vertex = vertices[i];
            }
        }
        return vertex;
    }

    public override Vertex[] GetNeighbours(Vertex v)
    {
        List<Edge> edges = v.neighbours;
        Vertex[] neighbours = new Vertex[edges.Count];
        for (int i = 0; i < edges.Count; i++)
        {
            neighbours[i] = edges[i].vertex;
        }
        return neighbours;
    }
}
