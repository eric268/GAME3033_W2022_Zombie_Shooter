using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AIBehaviourTree;
using UnityEngine.AI;

public class IsDeadNode : Node
{
    ZombieAI zombieAI;
    NavMeshAgent agent;

    public IsDeadNode(ZombieAI _zombieAI, NavMeshAgent _agent)
    {
        zombieAI = _zombieAI;
        agent = _agent;    
    }
    public override NodeState Evaluate()
    {
        if (zombieAI.GetComponent<HealthComponent>().CurrentHealth <= 0.0f)
        {
            zombieAI.ChangeZombieState(ZombieState.Dead);
            agent.isStopped = true;
            return NodeState.SUCCESS;
        }

         return NodeState.FAILURE;
    }
}
