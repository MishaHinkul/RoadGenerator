using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AgentModel
{
    public bool blendWeight = false;
    public bool blendPriority = false;
    public float priorityThreshold = 0.2f;
    public bool blendPipeline = false;
    public float maxSpeed;
    public float maxAccel;
    public float maxRotation;
    public float maxAngularAccel;
    public float orientation;
    public float rotation;
    public Vector3 velocity;
    public Steering Steering
    {
        get
        {
            return steering;
        }
        set
        {
            if (value == null)
            {
                Debug.Log("NULL");
            }
            steering = value;
        }
    }
    private Steering steering;
    public Dictionary<int, List<Steering>> groups;

    public AgentModel()
    {
        velocity = Vector3.zero;
        Steering = new Steering();
        groups = new Dictionary<int, List<Steering>>();

    }
}