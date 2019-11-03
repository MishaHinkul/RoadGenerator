using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowIntersectionModel
{
  public ShowIntersectionModel()
  {
  }

  public ShowIntersectionModel(Intersection intersection)
  {
    Intersection = intersection;
  }
  public ShowIntersectionModel(Intersection intersection, System.Func<bool> callback) : this(intersection)
  {
    Callback = callback;
  }

  public System.Func<bool> Callback { get; set; }
  public Intersection Intersection { get; set; }
}
