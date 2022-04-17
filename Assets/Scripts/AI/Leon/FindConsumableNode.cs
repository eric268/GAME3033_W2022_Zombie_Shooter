using AIBehaviourTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FindConsumableNode : Node
{
    GameObject[] mConsumableArray;
    NavMeshAgent mAgent;
    LeonAI mLeonAI;

    //Check if health is below certain threshold

   public FindConsumableNode(LeonAI leonAI, NavMeshAgent navMeshAgent, GameObject[] consumableArray)
   {
        mLeonAI = leonAI;
        mAgent = navMeshAgent;
        mConsumableArray = consumableArray;
   }

    public override NodeState Evaluate()
    {
        return (FindClosestConsumable()) ? NodeState.RUNNING : NodeState.FAILURE;
    }

    private GameObject FindClosestConsumable()
    {
        float distanceToConsumable = -Mathf.Infinity;
        GameObject mTarget = null;

        foreach (GameObject consumable in mConsumableArray)
        {
            if (consumable.GetComponent<ItemPickupComponent>().mIsAvailable)
            {
                float maxDistance = Vector3.Distance(consumable.transform.position, mLeonAI.transform.position);
                if (distanceToConsumable < maxDistance)
                {
                    distanceToConsumable = maxDistance;
                    mTarget = consumable;
                }
            }
        }

        if (mTarget != null)
        {
            mAgent.SetDestination(mTarget.transform.position);
            mAgent.isStopped = false;
        }

        return mTarget;
    }
}
