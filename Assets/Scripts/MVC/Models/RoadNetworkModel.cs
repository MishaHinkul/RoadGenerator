using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadNetworkModel
{
    /// <summary>
    /// Список всех дорог, в нашей сети
    /// </summary>
    public List<RoadSegment> roadSegments { get; private set; }

    /// <summary>
    /// Список всех пересечений в нашей сети
    /// </summary>
	public List<Intersection> roadIntersections { get; private set; }

    public Transform roadNetworkTransform;
    public Transform roadIntersectionTransform;


    /// <summary>
    /// Храним координаты точек которые уже учавствовали в визуализайии сети дорог
    /// Чтобы не создать меш дважды для одного и того же места
    /// </summary>
    public List<Vector3> viewRoads { get; private set; }
    public List<Vector3> viewIntersection { get; private set; }

    /// <summary>
    /// Масштаб сети дорог
    /// </summary>
	public float Scale { get; set; }

    /// <summary>
    /// Минимальная длинна сегмента дороги, иначе он будет удален из сети
    /// </summary>
	public float ShortCutOff = 5f;
    public float CloseCutoff = 7f;

    public RoadNetworkModel()
    {
        roadIntersections = new List<Intersection>();
        roadSegments = new List<RoadSegment>();
        viewRoads = new List<Vector3>();
        viewIntersection = new List<Vector3>();

    }
}
