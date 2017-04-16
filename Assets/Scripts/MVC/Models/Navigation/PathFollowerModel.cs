using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PathFollowerModel
{
    public Path path;
    public float pathOffset = 0.0f;
    public float currentParam;
}