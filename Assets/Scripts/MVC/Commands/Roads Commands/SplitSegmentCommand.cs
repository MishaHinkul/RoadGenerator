using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Разбить указаный сегмент
/// </summary>
public class SplitSegmentCommand : BaseCommand
{
  private const float RANGE_MIN = 0.3f;
  private const float RANGE_MAX = 0.6f;
  private SplitSegmentModel model = null; 

  public override void Execute()
  {
    model = eventData.data as SplitSegmentModel;
    if (model == null)
    {
      return;
    }
    Vector3 pBegin = model.Segment.Begin.WorldPosition;
    Vector3 pEnd = model.Segment.End.WorldPosition;
    Vector3 pointBeginNewSegment = GetBeginNewSegment(pBegin, pEnd);

    float lenghtSegment = GetLenghtEnd(model.Segment.Level);
    Vector3 perp = model.Segment.GetWorldPerp();
    Vector3 pointEndNewSegment = GetEndNewSegment(lenghtSegment, perp, pointBeginNewSegment);

    RoadSegment newSegment = GetNewSegment(pointBeginNewSegment, pointEndNewSegment, model.Segment.Level);


    //Теперь создадим сегмент в противоположнем направлении
    Vector3 newPointEndInversion = GetEndNewSegment(lenghtSegment, -perp, pointBeginNewSegment);
    RoadSegment newSegmentInversion = GetNewSegment(pointBeginNewSegment, newPointEndInversion, model.Segment.Level);

    bool withinSegment = SegmentWithin(newSegment, NetworkModel.CloseCutoff);
    bool withinInversionSegment = SegmentWithin(newSegmentInversion, NetworkModel.CloseCutoff);

    //Проверить, какие сегменты добавлять и добавлять
    bool segment1 = false;
    bool segment2 = false;
    WaitUntil wait = new WaitUntil(CallbackUnlit.PeekFlag);

    //Если мы не влазим в шасштабы сети
    if (!withinSegment)
    {
      segment1 = ClearWidthSegment(model.Segment, newSegment);
    }
    if (!withinInversionSegment)
    {
      segment2 = ClearWidthSegment(model.Segment, newSegmentInversion);
    }

    AddIntersection(model.Segment, newSegment, newSegmentInversion, pointBeginNewSegment, segment1, segment2);
    CallbackUnlit.Execute(model.Callback);
  }
  private Vector3 GetBeginNewSegment(Vector3 pBegin, Vector3 pEnd)
  {
    float length = GetLenghtBegin(pBegin, pEnd);
    //Получили направление сегмента дороги
    Vector3 direction = (pBegin - pEnd).normalized;
    //Получить новую точку, относмтельно которой начнется новый сегмент
    return pEnd + (direction * length);
  }

  private float GetLenghtBegin(Vector3 pBegin, Vector3 pEnd)
  {
    float splitDistance = Random.Range(RANGE_MIN, RANGE_MAX);

    float length = Vector3.Distance(pBegin, pEnd);
    length *= splitDistance; // длинна, части сегмента
    length = (int)length; // пусть будут только целые числа, чтобы правильно наложить меш и не масштабировать его

    return length;
  }

  private float GetLenghtEnd(int level)
  {
    //Например: scale = 100, segment.Level = 0, рендом = 1.5
    // Длинна будущего сегмента = 66.66
    float newLength = NetworkModel.Scale / ((level + 1) * Random.Range(1f, 2f));
    return (int)newLength; // пусть будут только целые числа, чтобы правильно наложить меш и не масштабировать его
  }

  private Vector3 GetEndNewSegment(float newLength, Vector3 per, Vector3 pointBeginNewSegment)
  {
    //Определяем где будет конец нового сегмента
    return pointBeginNewSegment + (per * newLength);
  }

  private RoadSegment GetNewSegment(Vector3 pointBeginNewSegment, Vector3 pointEndNewSegment, int level)
  {
    //Создаем новый сегмент
    return new RoadSegment(new RoadPoint(new Vector2(pointBeginNewSegment.x, pointBeginNewSegment.z), null),
                                         new RoadPoint(new Vector2(pointEndNewSegment.x, pointEndNewSegment.z), null),
                                         level + 1);
  }

  private bool ClearWidthSegment(RoadSegment rootSegment, RoadSegment newSegment)
  {
    bool segment = false;
    Vector2 intersectionPoint = Vector3.zero;
    RoadSegment intersectionSegment = null;
    int intersectionCount = SegmentIntersection(newSegment, out intersectionPoint, out intersectionSegment, rootSegment);

    if (intersectionCount <= 1)
    {
      NetworkModel.RemoveAllSaveSegmentIsEqual(newSegment);
      NetworkModel.SaveSegment(newSegment);
      segment = true;
    }

    if (intersectionCount == 1)
    {
      RoadSegment[] segmentsA = PatchSegment(intersectionSegment, new RoadPoint(intersectionPoint, intersectionSegment));
      RoadSegment[] segmentsB = PatchSegment(newSegment, new RoadPoint(intersectionPoint, newSegment));

      //Убираем короткие сегменты дороги
      bool patchA = segmentsA[0].SegmentLength() > NetworkModel.ShortCutOff;
      bool patchB = segmentsA[1].SegmentLength() > NetworkModel.ShortCutOff;
      bool patchC = segmentsB[0].SegmentLength() > NetworkModel.ShortCutOff;
      bool patchD = segmentsB[1].SegmentLength() > NetworkModel.ShortCutOff;

      List<RoadPoint> points = new List<RoadPoint>();
      if (patchA)
      {
        points.Add(segmentsA[0].End);
      }
      else
      {
        NetworkModel.RemoveAllSaveSegmentIsEqual(segmentsA[0]);
      }

      if (patchB)
      {
        points.Add(segmentsA[1].End);
      }
      else
      {
        NetworkModel.RemoveAllSaveSegmentIsEqual(segmentsA[1]);
      }

      if (patchC)
      {
        points.Add(segmentsB[0].End);
      }
      else
      {
        NetworkModel.RemoveAllSaveSegmentIsEqual(segmentsB[0]);
      }

      if (patchD)
      {
        points.Add(segmentsB[1].End);
      }
      else
      {
        NetworkModel.RemoveAllSaveSegmentIsEqual(segmentsB[1]);
      }

      //Все оставшиеся точки, и формируют перекрестки и т.д и состовляют пересечения в нашей сети
      Intersection newIntersection = new Intersection(points);
      NetworkModel.SaveIntersection(newIntersection);
    }

    return segment;
  }

  private Intersection AddIntersection(RoadSegment rootSegment, 
                                       RoadSegment newSegment,
                                       RoadSegment newSegmentInversion,
                                       Vector3 pointBeginNewSegment,
                                       bool segment1, 
                                       bool segment2)
  {
    if (!segment1 && !segment2)
    {
      return null;
    }

    Intersection inter = null;
    RoadSegment[] rootPatchs = PatchSegment(rootSegment, new RoadPoint(new Vector2(pointBeginNewSegment.x, pointBeginNewSegment.z), rootSegment));

    if (segment1 && segment2)
    {
       inter = new Intersection(new List<RoadPoint>{rootPatchs[0].End,
                                                    rootPatchs [1].End,
                                                    newSegment.Begin,
                                                    newSegmentInversion.Begin});
    }
    else if (segment1)
    {
      inter = new Intersection(new List<RoadPoint>{rootPatchs[0].End,
                                                   rootPatchs[1].End,
                                                   newSegment.Begin});
    }
    else if (segment2)
    {
      inter = new Intersection(new List<RoadPoint>{rootPatchs[0].End,
                                                   rootPatchs[1].End,
                                                   newSegmentInversion.Begin});
      
    }
    NetworkModel.SaveIntersection(inter);
    return inter;
  }


  /// <summary>
  /// Разбить один сигмент, на 2, и заменить его в нашей сети дорог
  /// </summary>
  /// <param name="segment"></param>
  /// <param name="newPoint">
  /// Точка, относительно которой будет делится сегмент
  /// </param>
  /// <returns>
  /// Получить, сегменты дороги. Их всегда 2
  /// </returns>
  private RoadSegment[] PatchSegment(RoadSegment segment, RoadPoint newPoint)
  {
    NetworkModel.RemoveAllSaveSegmentIsEqual(segment);

    RoadSegment left = new RoadSegment(segment.Begin, new RoadPoint(newPoint.Point), segment.Level);
    RoadSegment right = new RoadSegment(segment.End, new RoadPoint(newPoint.Point), segment.Level);

    NetworkModel.SaveSegment(left);
    NetworkModel.SaveSegment(right);

    return new RoadSegment[] { left, right };
  }

  /// <summary>
  /// Находится ли сегмент внутри
  /// </summary>
  /// <param name="segment"></param>
  /// <param name="max"></param>
  /// <returns></returns>
  private bool SegmentWithin(RoadSegment segment, float max)
  {
    RoadSegment seg = null;
    for (int i = 0; i < NetworkModel.GetRoadSegmentCount(); i++)
    {
      seg = NetworkModel.GetRoadSegment(i);
      bool amax = DistPointSegment(seg.Begin, segment) < max;
      bool bmax = DistPointSegment(seg.End, segment) < max;

      bool amin = MinPointDistance(seg, segment, max / 1.0f);

      if (amax || bmax || amin)
      {
        return true;
      }
    }
    return false;
  }

  /// <summary>
  /// Расстояние от точки до сегмента
  /// ( http://geomalgorithms.com/a02-_lines.html#Distance-to-Ray-or-Segment )
  /// </summary>
  /// <returns></returns>
  /// <param name="P">P.</param>
  /// <param name="S">S.</param>
  private float DistPointSegment(RoadPoint P, RoadSegment S)
  {

    Vector2 v = S.End.Point - S.Begin.Point;
    Vector2 w = P.Point - S.Begin.Point;

    float c1 = Vector2.Dot(w, v);
    if (c1 <= 0) // перед S.PointA
    {
      return Vector2.Distance(P.Point, S.Begin.Point);
    }

    float c2 = Vector2.Dot(v, v);
    if (c2 <= c1) // после S.PointB
    {
      return Vector2.Distance(P.Point, S.End.Point);
    }

    float b = c1 / c2;
    Vector2 Pb = S.Begin.Point + (v * b);
    return Vector2.Distance(P.Point, Pb);
  }


  /// <summary>
  /// Находятся ли сегменты, на минимальной дистанции друг от друга
  /// </summary>
  /// <param name="a"></param>
  /// <param name="b"></param>
  /// <param name="min"></param>
  /// <returns></returns>
  private bool MinPointDistance(RoadSegment a, RoadSegment b, float min)
  {
    if (Vector2.Distance(a.Begin.Point, b.Begin.Point) < min)
    {
      return true;
    }
    if (Vector2.Distance(a.Begin.Point, b.End.Point) < min)
    {
      return true;
    }
    if (Vector2.Distance(a.End.Point, b.Begin.Point) < min)
    {
      return true;
    }
    if (Vector2.Distance(a.End.Point, b.End.Point) < min)
    {
      return true;
    }
    return false;
  }


  /// <summary>
  /// Получить количество пересечений для указаного сегмента
  /// </summary>
  /// <param name="segment">
  /// Для кого ищем пересечение
  /// </param>
  /// <param name="intersectionPoint">
  /// Координаты последнего пересечения
  /// </param>
  /// <param name="intersectionSegment">
  /// Сегмент с которым пересеклись послндними
  /// </param>
  /// <param name="skipInFound">
  /// Игнорировать данный сегмент при поиске пересечений
  /// </param>
  /// <returns></returns>
  private int SegmentIntersection(RoadSegment segment, out Vector2 intersectionPoint, out RoadSegment intersectionSegment, RoadSegment skipInFound)
  {
    intersectionPoint = Vector2.zero;
    intersectionSegment = null;

    Vector2 tmp = Vector2.zero;
    Vector2 interTmp = Vector3.zero;

    int count = 0;
    RoadSegment currentSegment = null;

    float ignoreDistance = 0.01f;
    for (int i = 0; i < NetworkModel.GetRoadSegmentCount(); i++)
    {
      currentSegment = NetworkModel.GetRoadSegment(i);
      if (currentSegment.IsEqual(skipInFound))
      {
        continue;
      }
      else if (Vector2.Distance(currentSegment.Begin.Point, segment.Begin.Point) < ignoreDistance ||
              Vector2.Distance(currentSegment.End.Point, segment.End.Point) < ignoreDistance)
      {
        continue;
      }
      else if (Vector2.Distance(currentSegment.Begin.Point, segment.End.Point) < ignoreDistance ||
               Vector2.Distance(currentSegment.End.Point, segment.Begin.Point) < ignoreDistance)
      {
        continue;
      }
      else if (IntersectionTwoSegments(segment, currentSegment, out interTmp, out tmp) != 0)
      {
        intersectionSegment = currentSegment;
        intersectionPoint = new Vector2(interTmp.x, interTmp.y);
        count++;
      }
    }
    return count;
  }


  /// <summary>
  /// Найти 2D пересечение двух конечных сегментов
  /// ( http://geomalgorithms.com/a05-_intersect-1.html )
  /// </summary>
  /// <param name="segment1"></param>
  /// <param name="segment2"></param>
  /// <param name="I0">
  /// точка пересечения (если существует)
  /// </param>
  /// <param name="I1">
  ///  конечная точка перечечения сегмента [I0,I1] (если существует)
  /// </param>
  /// <returns>
  /// 0 = нет пересечений,  
  /// 1 = пересекаются в единственной точке I0,
  /// 2 = пересекаются в сегменте от I0 до I1
  /// </returns>
  int IntersectionTwoSegments(RoadSegment segment1, RoadSegment segment2, out Vector2 I0, out Vector2 I1)
  {
    Vector2 u = segment1.End.Point - segment1.Begin.Point;
    Vector2 v = segment2.End.Point - segment2.Begin.Point;
    Vector2 w = segment1.Begin.Point - segment2.Begin.Point;

    float dotProduct = Perp(u, v);

    I0 = Vector2.zero;
    I1 = Vector2.zero;

    // проверяем, параллельны ли они (включает либо точку)
    if (Mathf.Abs(dotProduct) < 0.01f)            // S1 и S2 паралельны
    {
      if (Perp(u, w) != 0 || Perp(v, w) != 0)
      {
        return 0; // Они не коллинеарны (не лежат на одной прамой, или паралельных прямых)
      }

      // они коллинеарны или вырождены
      // проверяем, являются ли они вырожденными точками
      float dotProductU = Vector2.Dot(u, u);
      float dotProductV = Vector2.Dot(v, v);

      // оба сегмента - это точки
      if (dotProductU == 0 && dotProductV == 0)
      {
        if (segment1.Begin.Point != segment2.Begin.Point)
        {
          return 0;
        }
        I0 = segment1.Begin.Point; // они - одна и та же точка
        return 1;
      }
      // S1 - одна точка
      if (dotProductU == 0)
      {
        if (InSegment(segment1.Begin, segment2) == 0) // но не в S2
        {
          return 0;
        }
        I0 = segment1.Begin.Point;
        return 1;
      }
      // S2 одна точка
      if (dotProductV == 0)
      {
        if (InSegment(segment2.Begin, segment1) == 0)  // но не в S1
        {
          return 0;
        }
        I0 = segment2.Begin.Point;
        return 1;
      }

      // они коллинеарные сегменты - получить перекрытие (или нет)


      float t0, t1;                   // конечные точки S1 в eqn для S2
      Vector2 w2 = segment1.End.Point - segment2.Begin.Point;
      if (v.x != 0)
      {
        t0 = w.x / v.x;
        t1 = w2.x / v.x;
      }
      else
      {
        t0 = w.y / v.y;
        t1 = w2.y / v.y;
      }
      if (t0 > t1)
      {                  // должно быть t0 меньше, чем t1
        float t = t0; t0 = t1; t1 = t;    // заменять, если нет
      }
      if (t0 > 1 || t1 < 0)
      {
        return 0;      //не перекрывать
      }
      t0 = t0 < 0 ? 0 : t0;               // ограничить в пределах 0
      t1 = t1 > 1 ? 1 : t1;               // ограничить в пределах 1
      if (t0 == t1)
      {                 //Пересечение точек
        I0 = segment2.Begin.Point + t0 * v;
        return 1;
      }

      // они перекрываются в допустимом сегменте
      I0 = segment2.Begin.Point + t0 * v;
      I1 = segment2.Begin.Point + t1 * v;
      return 2;
    }

    // отрезки наклонены и могут пересекаться в точке
    // получить параметр пересечения для S1
    float sI = Perp(v, w) / dotProduct;
    if (sI < 0 || sI > 1)                // нет пересечения с S1
    {
      return 0;
    }

    // получить параметр пересечения для S2
    float tI = Perp(u, w) / dotProduct;
    if (tI < 0 || tI > 1)                    // нет пересечения с S2
    {
      return 0;
    }

    I0 = segment1.Begin.Point + sI * u;              // вычисляем S1 точку пересечения
    {
      return 1;
    }
  }

  // inSegment(): determine if a point is inside a segment
  //    Input:  a point P, and a collinear segment S
  //    Return: 1 = P is inside S
  //            0 = P is  not inside S
  int InSegment(RoadPoint P, RoadSegment S)
  {
    if (S.Begin.Point.x != S.End.Point.x)
    {    // S is not  vertical
      if (S.Begin.Point.x <= P.Point.x && P.Point.x <= S.End.Point.x)
      {
        return 1;
      }
      if (S.Begin.Point.x >= P.Point.x && P.Point.x >= S.End.Point.x)
      {
        return 1;
      }
    }
    else
    {    // S is vertical, so test y  coordinate
      if (S.Begin.Point.y <= P.Point.y && P.Point.y <= S.End.Point.y)
      {
        return 1;
      }
      if (S.Begin.Point.y >= P.Point.y && P.Point.y >= S.End.Point.y)
      {
        return 1;
      }
    }
    return 0;
  }


  /// <summary>
  /// Перп скалярное произведение 
  /// </summary>
  /// <param name="u"></param>
  /// <param name="v"></param>
  /// <returns></returns>
  private float Perp(Vector2 u, Vector2 v)
  {
    return ((u).x * (v).y - (u).y * (v).x);
  }


  [Inject]
  public RoadNetworkModel NetworkModel { get; private set; }

  [Inject]
  public ICoroutineExecutor Executor { get; private set; }
}