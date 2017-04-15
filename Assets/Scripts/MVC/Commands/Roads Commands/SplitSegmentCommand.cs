using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Разбить указаный сегмент
/// </summary>
public class SplitSegmentCommand : BaseCommand
{
    [Inject]
    public RoadNetworkModel networkModel { get; private set; }

    public override void Execute()
    {
        RoadSegment rootSegment = eventData.data as RoadSegment;

        if (rootSegment == null)
        {
            return;
        }

        //Коэфициент на относительно которого мы разобем текущий сегмент
        float splitDistance = Random.Range(0.3f, 0.6f);

        //Начало и конец сегмента, положение в 3д мире
        Vector3 p1 = new Vector3(rootSegment.PointA.point.x, 0, rootSegment.PointA.point.y);
        Vector3 p2 = new Vector3(rootSegment.PointB.point.x, 0, rootSegment.PointB.point.y);

        float length = Vector3.Distance(p1, p2);
        length *= splitDistance; // длинна, части сегмента

        length = (int)length; // пусть будут только целые числа, чтобы правильно наложить меш и не масштабировать его

        //Получили направление сегмента дороги
        Vector3 direction = (p1 - p2).normalized;

        //Получить новую точку, относмтельно которой начнется новый сегмент
        Vector3 pointBeginNewSegment = p2 + (direction * length);

        //Получили направление для новой точки (перепендикуляр)
        Vector3 per = Vector3.Cross(p1 - p2, Vector3.down).normalized;// * (Random.Range (0f, 1f) < 0.5f ? -1 : 1);

        //Например: scale = 100, segment.Level = 0, рендом = 1.5
        // Длинна будущего сегмента = 66.66
        float newLength = networkModel.Scale / ((rootSegment.Level + 1) * Random.Range(1f, 2f));
        newLength = (int)newLength; // пусть будут только целые числа, чтобы правильно наложить меш и не масштабировать его

        //Определяем где будет конец нового сегмента
        Vector3 pointEndNewSegment = pointBeginNewSegment + (per * newLength);

        //Создаем новый сегмент
        RoadSegment newSegment = new RoadSegment(new RoadPoint(new Vector2(pointBeginNewSegment.x, pointBeginNewSegment.z), null),
                                                  new RoadPoint(new Vector2(pointEndNewSegment.x, pointEndNewSegment.z), null),
                                                  rootSegment.Level + 1);


        //Теперь создадим сегмент в противоположнем направлении

        Vector3 perA = Vector3.Cross(p1 - p2, Vector3.down).normalized * -1;
        Vector3 newPointEndOther = pointBeginNewSegment + (perA * newLength);

        RoadSegment newSegmentInversion = new RoadSegment(new RoadPoint(new Vector2(pointBeginNewSegment.x, pointBeginNewSegment.z), null),
                                                       new RoadPoint(new Vector2(newPointEndOther.x, newPointEndOther.z), null),
                                                       rootSegment.Level + 1);

        //Проверить, какие сегменты добавлять и добавлять
        bool segment1 = false;
        bool segment2 = false;

        bool withinSegment = SegmentWithin(newSegment, networkModel.CloseCutoff);
        bool withinInversionSegment = SegmentWithin(newSegmentInversion, networkModel.CloseCutoff);

        //Если мы не влазим в шасштабы сети
        if (!withinSegment)
        {
            Vector2 intersectionPoint = Vector3.zero;
            RoadSegment intersectionSegment = null;

            int intersectionCount = SegmentIntersection(newSegment, out intersectionPoint, out intersectionSegment, rootSegment);

            if (intersectionCount <= 1)
            {
                networkModel.roadSegments.RemoveAll(p => p.IsEqual(newSegment));
                networkModel.roadSegments.Add(newSegment);
                segment1 = true;
            }

            if (intersectionCount == 1)
            {
                RoadSegment[] segmentsA = PatchSegment(intersectionSegment, new RoadPoint(intersectionPoint, intersectionSegment));
                RoadSegment[] segmentsB = PatchSegment(newSegment, new RoadPoint(intersectionPoint, newSegment));

                //Убираем короткие сегменты дороги
                bool patchA = segmentsA[0].SegmentLength() > networkModel.ShortCutOff;
                bool patchB = segmentsA[1].SegmentLength() > networkModel.ShortCutOff;
                bool patchC = segmentsB[0].SegmentLength() > networkModel.ShortCutOff;
                bool patchD = segmentsB[1].SegmentLength() > networkModel.ShortCutOff;

                List<RoadPoint> points = new List<RoadPoint>();
                if (patchA)
                {
                    points.Add(segmentsA[0].PointB);
                }
                else
                {
                    networkModel.roadSegments.RemoveAll(p => p.IsEqual(segmentsA[0]));
                }

                if (patchB)
                {
                    points.Add(segmentsA[1].PointB);
                }
                else
                {
                    networkModel.roadSegments.RemoveAll(p => p.IsEqual(segmentsA[1]));
                }

                if (patchC)
                {
                    points.Add(segmentsB[0].PointB);
                }
                else
                {
                    networkModel.roadSegments.RemoveAll(p => p.IsEqual(segmentsB[0]));
                }

                if (patchD)
                {
                    points.Add(segmentsB[1].PointB);
                }
                else
                {
                    networkModel.roadSegments.RemoveAll(p => p.IsEqual(segmentsB[1]));
                }

                Intersection inter = new Intersection(points);
                //Все оставшиеся точки, и формируют перекрестки и т.д и состовляют пересечения в нашей сети
                networkModel.RoadIntersections.Add(inter);
            }
        }

        //Сегмент в противоположную сторону
        if (!withinInversionSegment)
        {
            Vector2 intersection = Vector3.zero;
            RoadSegment other = null;

            int intersectionCount = SegmentIntersection(newSegmentInversion, out intersection, out other, rootSegment);

            if (intersectionCount <= 1)
            {
                //Удаляем все сегменты из сети, которые точно такие же как только что созданый 
                networkModel.roadSegments.RemoveAll(p => p.IsEqual(newSegmentInversion));

                //Добавляем новосозданый сегмент в нашу сеть дорог
                networkModel.roadSegments.Add(newSegmentInversion);
                segment2 = true;
            }

            if (intersectionCount == 1)
            {
                RoadSegment[] segmentsA = PatchSegment(other, new RoadPoint(intersection, other));
                RoadSegment[] segmentsB = PatchSegment(newSegmentInversion, new RoadPoint(intersection, newSegmentInversion));

                //Убираем короткие сегменты дороги
                bool patchA = segmentsA[0].SegmentLength() > networkModel.ShortCutOff;
                bool patchB = segmentsA[1].SegmentLength() > networkModel.ShortCutOff;
                bool patchC = segmentsB[0].SegmentLength() > networkModel.ShortCutOff;
                bool patchD = segmentsB[1].SegmentLength() > networkModel.ShortCutOff;

                List<RoadPoint> points = new List<RoadPoint>();
                if (patchA)
                {
                    points.Add(segmentsA[0].PointB);
                }
                else
                {
                    networkModel.roadSegments.RemoveAll(p => p.IsEqual(segmentsA[0]));
                }

                if (patchB)
                {
                    points.Add(segmentsA[1].PointB);
                }
                else
                {
                    networkModel.roadSegments.RemoveAll(p => p.IsEqual(segmentsA[1]));
                }

                if (patchC)
                {
                    points.Add(segmentsB[0].PointB);
                }
                else
                {
                    networkModel.roadSegments.RemoveAll(p => p.IsEqual(segmentsB[0]));
                }

                if (patchD)
                {
                    points.Add(segmentsB[1].PointB);
                }
                else
                {
                    networkModel.roadSegments.RemoveAll(p => p.IsEqual(segmentsB[1]));
                }
                Intersection inter = new Intersection(points);
                networkModel.RoadIntersections.Add(inter);
            }
        }

        if (segment1 || segment2)
        {
            RoadSegment[] rootPatchs = this.PatchSegment(rootSegment, new RoadPoint(new Vector2(pointBeginNewSegment.x, pointBeginNewSegment.z), rootSegment));

            if (segment1 && segment2)
            {
                Intersection inter = new Intersection(new List<RoadPoint>{rootPatchs[0].PointB,
                                                                           rootPatchs [1].PointB,
                                                                           newSegment.PointA,
                                                                           newSegmentInversion.PointA});
                networkModel.RoadIntersections.Add(inter);
            }
            else if (segment1)
            {
                Intersection inter = new Intersection(new List<RoadPoint>{rootPatchs[0].PointB,
                                                                           rootPatchs[1].PointB,
                                                                           newSegment.PointA});
                networkModel.RoadIntersections.Add(inter);
            }
            else if (segment2)
            {
                Intersection inter = new Intersection(new List<RoadPoint>{rootPatchs[0].PointB,
                                                                           rootPatchs[1].PointB,
                                                                           newSegmentInversion.PointA});
                networkModel.RoadIntersections.Add(inter);
            }
        }
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
        networkModel.roadSegments.RemoveAll(p => p.IsEqual(segment));

        RoadSegment left = new RoadSegment(segment.PointA, new RoadPoint(newPoint.point), segment.Level);
        RoadSegment right = new RoadSegment(segment.PointB, new RoadPoint(newPoint.point), segment.Level);

        networkModel.roadSegments.Add(left);
        networkModel.roadSegments.Add(right);

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
        foreach (RoadSegment seg in networkModel.roadSegments)
        {
            bool amax = DistPointSegment(seg.PointA, segment) < max;
            bool bmax = DistPointSegment(seg.PointB, segment) < max;

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

        Vector2 v = S.PointB.point - S.PointA.point;
        Vector2 w = P.point - S.PointA.point;

        float c1 = Vector2.Dot(w, v);
        if (c1 <= 0) // перед S.PointA
        {
            return Vector2.Distance(P.point, S.PointA.point);
        }

        float c2 = Vector2.Dot(v, v);
        if (c2 <= c1) // после S.PointB
        {
            return Vector2.Distance(P.point, S.PointB.point);
        }

        float b = c1 / c2;
        Vector2 Pb = S.PointA.point + (v * b);
        return Vector2.Distance(P.point, Pb);
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
        if (Vector2.Distance(a.PointA.point, b.PointA.point) < min)
        {
            return true;
        }
        if (Vector2.Distance(a.PointA.point, b.PointB.point) < min)
        {
            return true;
        }
        if (Vector2.Distance(a.PointB.point, b.PointA.point) < min)
        {
            return true;
        }
        if (Vector2.Distance(a.PointB.point, b.PointB.point) < min)
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
        for (int i = 0; i < networkModel.roadSegments.Count; i++)
        {
            currentSegment = networkModel.roadSegments[i];
            if (currentSegment.IsEqual(skipInFound))
            {
                continue;
            }
            else if (Vector2.Distance(currentSegment.PointA.point, segment.PointA.point) < ignoreDistance ||
                    Vector2.Distance(currentSegment.PointB.point, segment.PointB.point) < ignoreDistance)
            {
                continue;
            }
            else if (Vector2.Distance(currentSegment.PointA.point, segment.PointB.point) < ignoreDistance ||
                     Vector2.Distance(currentSegment.PointB.point, segment.PointA.point) < ignoreDistance)
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
        Vector2 u = segment1.PointB.point - segment1.PointA.point;
        Vector2 v = segment2.PointB.point - segment2.PointA.point;
        Vector2 w = segment1.PointA.point - segment2.PointA.point;

        float dotProduct = Perp(u, v);

        I0 = Vector2.zero;
        I1 = Vector2.zero;

        // проверяем, параллельны ли они (включает либо точку)
        if (Mathf.Abs(dotProduct) < 0.01f)            // S1 и S2 паралельны
        {
            if (Perp(u, w) != 0 || Perp(v, w) != 0)
            {
                return 0;                    // Они не коллинеарны (не лежат на одной прамой, или паралельных прямых)
            }

            // они коллинеарны или вырождены
            // проверяем, являются ли они вырожденными точками
            float dotProductU = Vector2.Dot(u, u);
            float dotProductV = Vector2.Dot(v, v);

            // оба сегмента - это точки
            if (dotProductU == 0 && dotProductV == 0)
            {
                if (segment1.PointA.point != segment2.PointA.point)
                {
                    return 0;
                }
                I0 = segment1.PointA.point; // они - одна и та же точка
                return 1;
            }
            // S1 - одна точка
            if (dotProductU == 0)
            {
                if (InSegment(segment1.PointA, segment2) == 0) // но не в S2
                {
                    return 0;
                }
                I0 = segment1.PointA.point;
                return 1;
            }
            // S2 одна точка
            if (dotProductV == 0)
            {
                if (InSegment(segment2.PointA, segment1) == 0)  // но не в S1
                {
                    return 0;
                }
                I0 = segment2.PointA.point;
                return 1;
            }

            // они коллинеарные сегменты - получить перекрытие (или нет)


            float t0, t1;                   // конечные точки S1 в eqn для S2
            Vector2 w2 = segment1.PointB.point - segment2.PointA.point;
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
                I0 = segment2.PointA.point + t0 * v;
                return 1;
            }

            // они перекрываются в допустимом сегменте
            I0 = segment2.PointA.point + t0 * v;
            I1 = segment2.PointA.point + t1 * v;
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

        I0 = segment1.PointA.point + sI * u;              // вычисляем S1 точку пересечения
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
        if (S.PointA.point.x != S.PointB.point.x)
        {    // S is not  vertical
            if (S.PointA.point.x <= P.point.x && P.point.x <= S.PointB.point.x)
            {
                return 1;
            }
            if (S.PointA.point.x >= P.point.x && P.point.x >= S.PointB.point.x)
            {
                return 1;
            }
        }
        else
        {    // S is vertical, so test y  coordinate
            if (S.PointA.point.y <= P.point.y && P.point.y <= S.PointB.point.y)
            {
                return 1;
            }
            if (S.PointA.point.y >= P.point.y && P.point.y >= S.PointB.point.y)
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
}