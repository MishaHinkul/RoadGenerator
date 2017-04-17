using GPWiki;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Базовый класс для любого графа
/// </summary>
public class Graph : MonoBehaviour
{
    public GameObject vertexPrefab;
    public List<Vertex> vertices;
    protected List<List<Vertex>> neighbours;
    protected List<List<float>> costs;

    public virtual void Start()
    {

    }

    public virtual void Load()
    {

    }

    /// <summary>
    /// Получить размер графа
    /// </summary>
    /// <returns></returns>
    public virtual int GetSize()
    {
        if (ReferenceEquals(vertices, null))
        {
            return 0;
        }
        return vertices.Count;
    }

    /// <summary>
    /// Поиск ближаешей вершины для заданой позиции
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public virtual Vertex GetNearestVertex(Vector3 position)
    {
        return null;
    }

    /// <summary>
    /// Получение вершины по Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public virtual Vertex GetVertextObj(int id)
    {
        if (ReferenceEquals(vertices, null) || vertices.Count == 0)
        {
            return null;
        }
        if (id < 0 || id >= vertices.Count)
        {
            return null;
        }
        return vertices[id];
    }

    /// <summary>
    /// Получить соседей
    /// </summary>
    /// <param name="vertex"></param>
    /// <returns></returns>
    public virtual Vertex[] GetNeighbours(Vertex vertex)
    {
        if (ReferenceEquals(neighbours, null) || neighbours.Count == 0)
        {
            return new Vertex[0];
        }
        if (vertex.id < 0 || vertex.id >= neighbours.Count)
        {
            return new Vertex[0];
        }
        return neighbours[vertex.id].ToArray();
    }

    public virtual Edge[] GetEdges (Vertex vertex)
    {
        return vertices[vertex.id].neighbours.ToArray();
    }

    private List<Vertex> BuildPath(int srcId, int dstId, ref int[] prevList)
    {
        List<Vertex> path = new List<Vertex>();
        int prev = dstId;
        do
        {
            path.Add(vertices[prev]);
            prev = prevList[prev];
        }
        while (prev != srcId);
        return path;
    }

    //A*
    public delegate float Heuristic(Vertex а, Vertex b);

    public float EuclidDist(Vertex a, Vertex b)
    {
        Vector3 posA = a.transform.position;
        Vector3 posB = b.transform.position;

        return Vector3.Distance(posA, posB);
    }

    public float ManhattanDist(Vertex a, Vertex b)
    {
        Vector3 posA = a.transform.position;
        Vector3 posB = b.transform.position;
        return Mathf.Abs(posA.x - posB.x) + Mathf.Abs(posA.y - posB.y);
    }

    public List<Vertex> GetPathAstart(Vector3 begin, Vector3 end, Heuristic h = null)
    {
        if (ReferenceEquals(h, null))
        {
            h = EuclidDist;
        }

        Vertex src = GetNearestVertex(begin);
        Vertex dst = GetNearestVertex(end);
        GPWiki.BinaryHeap<Edge> frontier = new GPWiki.BinaryHeap<Edge>();
        Edge[] edges;
        Edge node, child;
        int size = vertices.Count;
        float[] distValue = new float[size];
        int[] previous = new int[size];
        node = new Edge(src, 0);
        frontier.Add(node);
        distValue[src.id] = 0;
        previous[src.id] = src.id;
        for (int i = 0; i < size; i++)
        {
            if (i == src.id)
            {
                continue;
            }
            distValue[i] = Mathf.Infinity;
            previous[i] = -1;
        }
        while (frontier.Count != 0)
        {
            node = frontier.Remove();
            int nodeId = node.vertex.id;
            if (ReferenceEquals(node.vertex, dst))
            {
                return BuildPath(src.id, node.vertex.id, ref previous);
            }
            edges = GetEdges(node.vertex);
            foreach (Edge e in edges)
            {
                int eId = e.vertex.id;
                if (previous[eId] != -1)
                {
                    continue;
                }
                float cost = distValue[nodeId] + e.cost;
                // key point
                cost += h(node.vertex, e.vertex);
                if (cost < distValue[e.vertex.id])
                {
                    distValue[eId] = cost;
                    previous[eId] = nodeId;
                    frontier.Remove(e);
                    child = new Edge(e.vertex, cost);
                    frontier.Add(child);
                }
            }
        }
        return new List<Vertex>();
    }

    // Деикстрв
    public List<Vertex> GetPathDijkstra(Vector3 begin, Vector3 end)
    {
        if (begin == null || end == null)
        {
            return new List<Vertex>();
        }
        Vertex src = GetNearestVertex(begin);
        Vertex dst = GetNearestVertex(end);
        GPWiki.BinaryHeap<Edge> frontier = new GPWiki.BinaryHeap<Edge>();
        Edge[] edges;
        Edge node, child;
        int size = vertices.Count;
        float[] distValue = new float[size];
        int[] previous = new int[size];
        node = new Edge(src, 0);
        frontier.Add(node);
        distValue[src.id] = 0;
        previous[src.id] = src.id;
        for (int i = 0; i < size; i++)
        {
            if (i == src.id)
            {
                continue;
            }
            distValue[i] = Mathf.Infinity;
            previous[i] = -1;
        }
        while (frontier.Count != 0)
        {
            node = frontier.Remove();
            int nodeId = node.vertex.id;
            // выход при необходимости
            if (ReferenceEquals(node.vertex, dst))
            {
                return BuildPath(src.id, node.vertex.id, ref previous);
            }
            edges = GetEdges(node.vertex);
            foreach (Edge edge in edges)
            {
                int edgeId = edge.vertex.id;
                if (previous[edgeId] != -1)
                {
                    continue;
                }
                float cost = distValue[nodeId] + edge.cost;
                if (cost < distValue[edge.vertex.id])
                {
                    distValue[edgeId] = cost;
                    previous[edgeId] = nodeId;
                    frontier.Remove(edge);
                    child = new Edge(edge.vertex, cost);
                    frontier.Add(child);
                }
            }
        }
        return new List<Vertex>();
    }
}