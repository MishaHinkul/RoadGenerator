using UnityEngine;
using System.Collections;

public struct HVector3
{
  public Vector3 vector;
  

  public static bool operator == (HVector3 lhs, HVector3 rhs)
  {
    return lhs.vector == rhs.vector;
  }

  public static bool operator != (HVector3 lhs, HVector3 rhs)
  {
    return lhs.vector != rhs.vector;
  }

  public static bool operator == (HVector3 lhs, Vector3 rhs)
  {
    return lhs.vector == rhs;
  }

  public static bool operator != (HVector3 lhs, Vector3 rhs)
  {
    return lhs.vector != rhs;
  }

  public static bool operator == (Vector3 lhs, HVector3 rhs)
  {
    return lhs == rhs;
  }

  public static bool operator != (Vector3 lhs, HVector3 rhs)
  {
    return lhs != rhs;
  }

  public override bool Equals(object other)
  {
    if (other is Vector3)
    {
      Vector3 vector3 = (Vector3)other;
      return vector == vector3;
    }
    if (other is HVector3)
    {
      HVector3 hVector = (HVector3)other;
      return hVector == vector;
    }

    return false; 
  }

  public override int GetHashCode()
  {
    return base.GetHashCode();
  }


  public HVector3(HVector3 hVector)
  {
    this.vector = hVector.vector;
  }

  public HVector3(Vector3 vector)
  {
    this.vector = vector;
  }
}