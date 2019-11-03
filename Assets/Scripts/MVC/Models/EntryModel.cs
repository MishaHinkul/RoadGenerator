using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntryModel
{
  public Vector3 GetRendomeEntry()
  {
    int indexEntry = Random.Range(0, Entrances.Count);
    return Entrances[indexEntry];
  }

  public EntryModel()
  {
    Entrances = new List<Vector3>();
  }

  public List<Vector3> Entrances { get; private set; }
}