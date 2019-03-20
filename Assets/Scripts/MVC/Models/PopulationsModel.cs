using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Модель храни в себе все здания созданые на карте
/// </summary>
public class PopulationsModel
{
  public Vector3 GetRendomeBuilding()
  {
    int index = Random.Range(0, Buildings.Count);
    return Buildings[index].transform.position;
  }


  public PopulationsModel()
  {
    Buildings = new List<GameObject>();
  }

  public List<GameObject> Buildings { get; set; }
}