using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeEntryModel
{
    public List<Vector3> Entrances { get; private set; }

    public TreeEntryModel()
    {
        Entrances = new List<Vector3>();
    }
}
