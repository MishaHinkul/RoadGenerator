using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeEntryModel
{
  public TreeEntryModel()
  {
    Entrances = new List<Vector3>();
  }

  public List<Vector3> Entrances { get; private set; }
}