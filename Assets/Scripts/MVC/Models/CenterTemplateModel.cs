using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterTemplateModel
{
    public Vector2 Center { get; set; }
    public float Angle { get; set; }

    public CenterTemplateModel(Vector2 center, float angle)
    {
        Center = center;
        Angle = angle;
    }
}