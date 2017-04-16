using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path
{

    public List<Vertex> nodes;
    List<PathSegment> segments;

    public Path()
    {
        segments = GetSegments();
    }

    public Path(List<Vertex> pathVetex)
    {
        if (pathVetex != null)
        {
            nodes = pathVetex;
            segments = GetSegments();
        }     
    }

    /// <summary>
    /// Получить ближайший сегмент
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    private PathSegment GetNearestSegment(Vector3 position)
    {
        float nearestDistance = Mathf.Infinity;
        float distance = nearestDistance;
        Vector3 centroid = Vector3.zero;
        PathSegment segment = new PathSegment();
        foreach (PathSegment s in segments)
        {
            centroid = (s.a + s.b) / 2.0f;
            distance = Vector3.Distance(position, centroid);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                segment = s;
            }
        }
        return segment;
    }

    /// <summary>
    /// Получить сегменты пути
    /// </summary>
    /// <returns></returns>
    public List<PathSegment> GetSegments()
    {
        List<PathSegment> segments = new List<PathSegment>();
        for (int i = 0; i < nodes.Count - 1; i++)
        {
            Vector3 src = nodes[i].transform.position;
            Vector3 dst = nodes[i + 1].transform.position;
            PathSegment segment = new PathSegment(src, dst);
            segments.Add(segment);
        }
        return segments;
    }

    /// <summary>
    /// Смешение точки в внутреннном представлении
    /// </summary>
    /// <param name="position"></param>
    /// <param name="lastParam"></param>
    /// <returns></returns>
    public float GetParam(Vector3 position, float lastParam)
    {
        //Найти ближайший к агенту сегмент
        float param = 0f;
        PathSegment currentSegment = null;
        float tempParam = 0f;

        // Мы находим текущий сегмент в пути, где агент
        foreach (PathSegment ps in segments)
        {
            tempParam += Vector3.Distance(ps.a, ps.b);

            if (lastParam <= tempParam)
            {
                currentSegment = ps;
                break;
            }
        }

        if (currentSegment == null)
            return 0f;

        //Вычмсляем направление движения из текущей позиции

        Vector3 segmentDirection = currentSegment.b - currentSegment.a;
        segmentDirection.Normalize();
        Vector3 currPos = position - currentSegment.a;
        //Найти точку в сегменте
        //Мы используем векторные проекции, чтобы найти точку над сегментом
        Vector3 pointInSegment = Vector3.Project(currPos, segmentDirection);
        // Текущий параметр - это сумма расстояний до последнего узла
        // плюс длина проекции с последнего шага

        //Вернуть следующую позицию на линии маршрута
        param = tempParam - Vector3.Distance(currentSegment.a, currentSegment.b);
        param += pointInSegment.magnitude;
        return param;
    }

    /// <summary>
    /// Преобразуем внутреннее представление, в координаты в игре
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    public Vector3 GetPosition(float param)
    {
        Vector3 position = Vector3.zero;

        PathSegment currentSegment = null;
        float tempParam = 0f;

        // Мы находим текущий сегмент в пути, где агент
        foreach (PathSegment ps in segments)
        {
            tempParam += Vector3.Distance(ps.a, ps.b);
            if (param <= tempParam)
            {
                currentSegment = ps;
                break;
            }
        }

        if (currentSegment == null)
            return Vector3.zero;

        Vector3 segmentDirection = currentSegment.b - currentSegment.a;
        segmentDirection.Normalize();
        tempParam -= Vector3.Distance(currentSegment.a, currentSegment.b);
        tempParam = param - tempParam;
        position = currentSegment.a + segmentDirection * tempParam;
        return position;
    }
}
