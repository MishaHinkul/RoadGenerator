using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Клас для предствавления точки видимости, из которой потом сформируется Граф
/// </summary>
public class VertexVisibility : Vertex
{
    private void Awake()
    {
        neighbours = new List<Edge>();
    }

    public void FindNeighbours(List<Vertex> vertices)
    {
        Collider collider = gameObject.GetComponent<Collider>();
        collider.enabled = false;
        Vector3 direction = Vector3.zero;
        Vector3 origin = transform.position;
        Vector3 target = Vector3.zero;
        RaycastHit[] hits;
        Ray ray;
        float distance = 0f;
        for (int i = 0; i < vertices.Count; i++)
        {
            if (vertices[i] == this)
            {
                continue;
            }
            target = vertices[i].transform.position;
            direction = target - origin;
            distance = direction.magnitude;
            ray = new Ray(origin, direction);
            //Ищем с помощью лучей соседей, если нам не мешает другая геометрия с коллайдерами
            hits = Physics.RaycastAll(ray, distance);
            if (hits.Length == 1)
            {
                if (hits[0].collider != null)
                {
                    if (hits[0].collider.gameObject.tag.Equals("Vertex"))
                    {
                        Edge edge = new Edge();
                        edge.cost = distance;
                        GameObject go = hits[0].collider.gameObject;
                        Vertex vertex = go.GetComponent<Vertex>();
                        if (vertex != vertices[i])
                        {
                            continue;
                        }
                        edge.vertex = vertex;
                        neighbours.Add(edge);
                    }
                }            
            }
        }
        collider.enabled = true;
    }
}