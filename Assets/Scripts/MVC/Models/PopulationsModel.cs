using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Модель храни в себе все здания созданые на карте
/// </summary>
public class PopulationsModel
{
	public List<GameObject> buildings { get; set; }

    public PopulationsModel()
    {
        buildings = new List<GameObject>();
    }
}
