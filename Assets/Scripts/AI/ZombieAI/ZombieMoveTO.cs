using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AIBehaviourTree;
using UnityEngine.AI;

public class ZombieMoveTo : Node
{
    ZombieAI zombieAI;
    public WayPoint targetPosition;
    public NavMeshAgent agent;
    float distanceBuffer;
    public ZombieMoveTo(ZombieAI _zombieAI, NavMeshAgent _agent, float _distanceBuffer)
    {
        zombieAI = _zombieAI;
        agent = _agent;
        distanceBuffer = _distanceBuffer;
    }

    public override NodeState Evaluate()
    {
        //This should never be called
        if (zombieAI.currentTarget == null)
        {
            Debug.LogError("No target Set Error in zombie move to function");
            return NodeState.FAILURE;
        }

        if (Vector3.Distance(zombieAI.currentTarget.transform.position, zombieAI.transform.position) <= distanceBuffer)
        {
            return NodeState.FAILURE;
        }

        agent.SetDestination(zombieAI.currentTarget.transform.position);
        agent.isStopped = false;
        zombieAI.ChangeZombieState(ZombieState.Running);
        return NodeState.RUNNING;
    }
}
