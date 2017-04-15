using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Класс для хранения соседних вершин с их стоймостями 
/// </summary>
public class Edge : IComparable<Edge>
{
    public float cost;
    public Vertex vertex;

    public Edge(Vertex vertex = null, float cost = 1f)
    {
        this.vertex = vertex;
        this.cost = cost;
    }

    /// <summary>
    /// Метод сравнения
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public int CompareTo(Edge other)
    {
        float result = cost - other.cost;
        int idA = vertex.GetInstanceID();
        int idB = other.vertex.GetInstanceID();
        if (idA == idB)
        {
            return 0;
        }
        return (int)result;
    }

    /// <summary>
    /// Сравнение вдух робер
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(Edge other)
    {
        return (other.vertex.id == this.vertex.id);
    }

    public override bool Equals(object obj)
    {
        Edge other = (Edge)obj;
        return Equals(other);
    }

    //Нужна для Equals(object obj)
    public override int GetHashCode()
    {
        return this.vertex.GetHashCode();
    }
}
