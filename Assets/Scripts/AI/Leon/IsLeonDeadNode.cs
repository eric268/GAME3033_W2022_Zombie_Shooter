using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AIBehaviourTree;
using UnityEngine.AI;

public class IsLeonDeadNode : Node
{
    LeonAI mLeonAI;
    NavMeshAgent agent;

    public IsLeonDeadNode(LeonAI leonAI, NavMeshAgent _agent)
    {
        mLeonAI = leonAI;
        agent = _agent;
    }
    public override NodeState Evaluate()
    {
        if (mLeonAI.GetComponentInParent<HealthComponent>().CurrentHealth <= 0.0f)
        {
            mLeonAI.FChangeState(LeonState.Dying);
            agent.isStopped = true;
            return NodeState.SUCCESS;
        }

        return NodeState.FAILURE;
    }
}
