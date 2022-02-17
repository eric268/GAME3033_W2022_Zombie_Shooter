using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AIBehaviourTree;
using UnityEngine.AI;

public class MoveTo : Node
{
    LeonAI leonAI;
    public WayPoint targetPosition;
    public NavMeshAgent agent;
    float distanceBuffer;
    public MoveTo(LeonAI _leonAI, NavMeshAgent _agent, float _distanceBuffer)
    {
        leonAI = _leonAI;
        agent = _agent;
        distanceBuffer = _distanceBuffer;
    }

    public override NodeState Evaluate()
    {
        if (Vector3.Distance(leonAI.currentWaypointTarget.position, leonAI.transform.position) <= distanceBuffer)
        {
            return NodeState.FAILURE;
        }

        agent.SetDestination(leonAI.currentWaypointTarget.position);
        agent.isStopped = false;
        return NodeState.RUNNING;
    }
}
