using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollowerView : SeekView
{
    [Header("PathFollowerView:")]
    public PathFollowerModel pathFllowerModel;

    private System.Action callbackFinished = null;

    private bool isMove = false;

    public override void LoadView()
    {
        base.LoadView();
        agentBehaviourModel.target = new GameObject("Car Target");
        pathFllowerModel.currentParam = 0f;
    }

    public override Steering GetSteering()
    {
        if (isMove)
        {
            //Достигли конца пути
            if (Vector3.Distance(agentBehaviourModel.target.transform.position, 
                                 pathFllowerModel.path.nodes[pathFllowerModel.path.nodes.Count - 1].transform.position) < pathFllowerModel.pathOffset)
            {
                isMove = false; 
                if (callbackFinished != null)
                {
                    callbackFinished();
                }

            }
            else
            {
                pathFllowerModel.currentParam = pathFllowerModel.path.GetParam(transform.position, pathFllowerModel.currentParam);
                float targetParam = pathFllowerModel.currentParam + pathFllowerModel.pathOffset;
                agentBehaviourModel.target.transform.position = pathFllowerModel.path.GetPosition(targetParam);
                return base.GetSteering();
            }
        }
        return new Steering();
       
    }

    public void StartMove(Path path, System.Action callbackToFinished)
    {

        if (path == null)
        {
            return;
        }
        pathFllowerModel.path = path;
        this.callbackFinished = callbackToFinished;
        isMove = true;
    }

    private void OnDrawGizmos()
    {
        if (pathFllowerModel.path == null || pathFllowerModel.path.nodes == null)
        {
            return;
        }
        Vector3 direction;
        Color tmp = Gizmos.color;
        Gizmos.color = Color.magenta;
        for (int i = 0; i < pathFllowerModel.path.nodes.Count - 1; i++)
        {
            Vector3 src = pathFllowerModel.path.nodes[i].transform.position;
            Vector3 dst = pathFllowerModel.path.nodes[i + 1].transform.position;
            direction = dst - src;
            Gizmos.DrawRay(src, direction);
        }
        Gizmos.color = tmp;
    }

    private void OnDestroy()
    {
        if (agentBehaviourModel.target != null)
        {
            GameObject.Destroy(agentBehaviourModel.target);
        }
    }
}