using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Пользовательский тип данных для хранения перемещения и поворота агента
/// </summary>
[System.Serializable]
public class Steering
{
    public float angular;
    public Vector3 linear;

    public Steering()
    {
        angular = 0.0f;
        linear = new Vector3();
    }
}
