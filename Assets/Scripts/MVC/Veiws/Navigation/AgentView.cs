using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Основной компонент, отвечающий за модель интелектуального поведения.
/// С данным компонентом должны работать все кто реализует модель поведения
/// </summary>
public class AgentView : BaseView
{
  public AgentModel model;

  public void LoadView()
  {
    model.velocity = Vector3.zero;
    model.Steering = new Steering();
    model.groups = new Dictionary<int, List<Steering>>();
  }

  public void RemoveView()
  {

  }

  public virtual void Update()
  {
    //Обработка перемещения относительно текущих значений
    Vector3 displacement = model.velocity * Time.deltaTime;
    model.orientation += model.rotation * Time.deltaTime;
    //Ограничиваем значение orientation (0 - 360)
    if (model.orientation < 0.0f)
    {
      model.orientation += 360.0f;
    }
    else if (model.orientation > 360.0f)
    {
      model.orientation -= 360.0f;
    }
    transform.Translate(displacement, Space.World);
    transform.rotation = new Quaternion();
    transform.Rotate(Vector3.up, model.orientation);
  }

  //Подготавливаем данные для следующего кадра на основе текущего
  public virtual void LateUpdate()
  {
    if (model.blendPriority)
    {
      model.Steering = GetPrioritySteering();
      model.groups.Clear();
    }
    model.velocity += model.Steering.linear * Time.deltaTime;
    model.rotation += model.Steering.angular * Time.deltaTime;
    if (model.velocity.magnitude > model.maxSpeed)
    {
      model.velocity.Normalize();
      model.velocity = model.velocity * model.maxSpeed;
    }
    if (model.rotation > model.maxRotation)
    {
      model.rotation = model.maxRotation;
    }
    if (model.Steering.angular == 0.0f)
    {
      model.rotation = 0.0f;
    }
    if (model.Steering.linear.sqrMagnitude == 0.0f)
    {
      model.velocity = Vector3.zero;
    }
    model.Steering = new Steering();
  }

  public void SetSteering(Steering steering)
  {
    this.model.Steering = steering;
  }

  public void SetSteering(Steering steering, float weight)
  {
    if (steering == null)
    {
      return;
    }
    this.model.Steering.linear += (weight * steering.linear);
    this.model.Steering.angular += (weight * steering.angular);
  }

  public void SetSteering(Steering steering, int priority)
  {
    if (!model.groups.ContainsKey(priority))
    {
      model.groups.Add(priority, new List<Steering>());
    }
    model.groups[priority].Add(steering);
  }

  public void SetSteering(Steering steering, bool pipeline)
  {
    if (!pipeline)
    {
      this.model.Steering = steering;
      return;
    }
  }
  private Steering GetPrioritySteering()
  {
    Steering steering = new Steering();
    float sqrThreshold = model.priorityThreshold * model.priorityThreshold;
    List<int> gIdList = new List<int>(model.groups.Keys);
    gIdList.Sort();

    foreach (int gid in gIdList)
    {
      steering = new Steering();
      foreach (Steering singleSteering in model.groups[gid])
      {
        steering.linear += singleSteering.linear;
        steering.angular += singleSteering.angular;
      }
      if (steering.linear.magnitude > model.priorityThreshold ||
          Mathf.Abs(steering.angular) > model.priorityThreshold)
      {
        return steering;
      }
    }

    return steering;
  }
}