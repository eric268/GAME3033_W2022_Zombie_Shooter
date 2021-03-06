using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AIBehaviourTree;
using UnityEngine.AI;

public class AttackTarget : Node
{
    ZombieAI zombieAI;
    NavMeshAgent agent;
    public AttackTarget(ZombieAI _zombieAI, NavMeshAgent _agent) 
    {
        zombieAI = _zombieAI;
        agent = _agent;
    }
    public override NodeState Evaluate()
    {
        if (!zombieAI.currentTarget)
        {
            return NodeState.FAILURE;
        }
        zombieAI.FChangeState(ZombieState.Attacking);
        agent.isStopped = true;
        zombieAI.transform.LookAt(zombieAI.currentTarget.transform);
        return NodeState.RUNNING;
    }
}
