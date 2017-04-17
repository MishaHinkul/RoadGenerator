using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Будет служить базой для будущих моделей поведения.
/// Т.е. сдесь мы реализуем только логику перемещения объекта, а все остальные расчеты делает Agent
/// </summary>
public class AgentBehaviurView : BaseView
{
    [Header("AgentBehaviurView: ")]
    public AgentBehaviurModel agentBehaviourModel;
 

    protected AgentView agent;


    public virtual void LoadView()
    {
        agent = gameObject.GetComponent<AgentView>();
    }

    public virtual void RemoveView()
    {

    }

    public virtual void Update()
    {
        if (agent.model.blendWeight)
        {
            agent.SetSteering(GetSteering(), agentBehaviourModel.weight);
        }
        else if (agent.model.blendPriority)
        {
            agent.SetSteering(GetSteering(), agentBehaviourModel.priority);
        }
        else if (agent.model.blendPipeline)
        {
            agent.SetSteering(GetSteering(), true);
        }
        else
        {
            agent.SetSteering(GetSteering());
        }
    }

    /// <summary>
    /// Сдесь реализуется вся основная логика модели поведения, которая и определяет врящение и положение объекта
    /// </summary>
    /// <returns></returns>
    public virtual Steering GetSteering()
    {
        return new Steering();
    }

    /// <summary>
    /// Определяем направление вращения
    /// </summary>
    /// <param name="rotation"></param>
    /// <returns></returns>
    public float MapToRange(float rotation)
    {
        rotation %= 360.0f;
        if (Mathf.Abs(rotation) > 180.0f)
        {
            if (rotation < 0.0f)
            {
                rotation += 360.0f;
            }
            else
            {
                rotation -= 360.0f;
            }
        }
        return rotation;
    }

    /// <summary>
    /// Преобразуем направление в вектор
    /// </summary>
    /// <param name="orientation"></param>
    /// <returns></returns>
    public Vector3 OriToVec(float orientation)
    {
        Vector3 vector = Vector3.zero;
        vector.x = Mathf.Sin(orientation * Mathf.Deg2Rad) * 1.0f;
        vector.z = Mathf.Cos(orientation * Mathf.Deg2Rad) * 1.0f;
        return vector.normalized;
    }
}