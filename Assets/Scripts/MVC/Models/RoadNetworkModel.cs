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
    /// Масштаб сети дорог
    /// </summary>
	public float Scale { get; set; }

    /// <summary>
    /// Минимальная длинна сегмента дороги, иначе он будет удален из сети
    /// </summary>
	public float ShortCutOff = 5.5f;
    public float CloseCutoff = 7.5f;

    public void Init()
    {
        roadIntersections = new List<Intersection>();
        roadSegments = new List<RoadSegment>();
    }

}
