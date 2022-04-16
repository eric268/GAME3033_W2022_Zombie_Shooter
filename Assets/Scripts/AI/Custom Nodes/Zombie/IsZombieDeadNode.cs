using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AIBehaviourTree;
using UnityEngine.AI;

public class IsZombieDeadNode : Node
{
    ZombieAI zombieAI;
    NavMeshAgent agent;

    public IsZombieDeadNode(ZombieAI _zombieAI, NavMeshAgent _agent)
    {
        zombieAI = _zombieAI;
        agent = _agent;    
    }
    public override NodeState Evaluate()
    {
        if (zombieAI.GetComponent<HealthComponent>().CurrentHealth <= 0.0f)
        {
            zombieAI.FChangeState(ZombieState.Dead);
            agent.isStopped = true;
            return NodeState.SUCCESS;
        }

         return NodeState.FAILURE;
    }
}
