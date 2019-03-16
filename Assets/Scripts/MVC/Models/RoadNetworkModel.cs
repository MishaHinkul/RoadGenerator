using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadNetworkModel
{
  public RoadNetworkModel() 
  {
    RoadIntersections = new List<Intersection>();
    RoadSegments = new List<RoadSegment>();
    ViewIntersection = new List<HVector3>();
    ShortCutOff = 5f;
    CloseCutoff = 7f;
  }


  /// <summary>
  /// Список всех дорог, в нашей сети
  /// </summary>
  public List<RoadSegment> RoadSegments { get; private set; }

  /// <summary>
  /// Список всех пересечений в нашей сети
  /// </summary>
  public List<Intersection> RoadIntersections { get; private set; }

  /// <summary>
  /// Объект в сцене, родитель все сети
  /// </summary>
  public Transform RoadNetworkTransform { get; set; }

  /// <summary>
  /// Объект в сцене, родитель для всех пересечений
  /// </summary>
  public Transform RoadIntersectionTransform { get; set; }

  public List<HVector3> ViewIntersection { get; private set; }

  /// <summary>
  /// Масштаб сети дорог
  /// </summary>
  public float Scale { get; set; }

  /// <summary>
  /// Ширина элемента дороши (Ширины префаба)
  /// </summary>
  public float WithRoad { get; set; }

  /// <summary>
  /// Минимальная длинна сегмента дороги, иначе он будет удален из сети
  /// </summary>
  public float ShortCutOff { get; private set; }

  public float CloseCutoff { get; private set; }
}