using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Модель поведения следования за целью
/// </summary>
public class SeekView : AgentBehaviurView
{
  public override Steering GetSteering()
  {
    Steering steering = new Steering();
    steering.linear = agentBehaviourModel.target.transform.position - transform.position;
    steering.linear.Normalize();
    steering.linear = steering.linear * agent.model.maxAccel;

    return steering;
  }
}