using System;

/// <summary>
/// Класс для хранения соседних вершин и их стоймости
/// </summary>
[System.Serializable]
public class Edge : IComparable<Edge>
{
    public float cost;
    public Vertex vertex;

    public Edge(Vertex vertex = null, float cost = 1f)
    {
        this.vertex = vertex;
        this.cost = cost;
    }

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

    public bool Equals(Edge other)
    {
        return (other.vertex.id == this.vertex.id);
    }

    /// <summary>
    /// Сравнение двух объектов
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object obj)
    {
        Edge other = (Edge)obj;
        return (other.vertex.id == this.vertex.id);
    }

    //Необходимо для Equals(Edge other)
    public override int GetHashCode()
    {
        return this.vertex.GetHashCode();
    }
}