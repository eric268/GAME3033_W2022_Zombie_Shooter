using AIBehaviourTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FindWeaponNode : Node
{
    GameObject[] mConsumableArray;
    NavMeshAgent mAgent;
    LeonAI mLeonAI;


    //Check if no weapon or no ammo

    public FindWeaponNode(LeonAI leonAI, NavMeshAgent navMeshAgent, GameObject[] consumableArray)
    {
        mLeonAI = leonAI;
        mAgent = navMeshAgent;
        mConsumableArray = consumableArray;
    }

    public override NodeState Evaluate()
    {
        return (FindClosestConsumable()) ? NodeState.SUCCESS : NodeState.FAILURE;
    }

    private GameObject FindClosestConsumable()
    {
        float closestConsumable = Mathf.Infinity;
        GameObject mTarget = null;

        if (mLeonAI.mWeaponHolder.equippedWeapon != null && mLeonAI.mWeaponHolder.equippedWeapon.weaponStats.totalBullets >= 10)
            return null;

        foreach (GameObject consumable in mConsumableArray)
        {
            if (consumable.GetComponent<ItemPickupComponent>().mIsAvailable)
            {
                float distance = Vector3.Distance(mLeonAI.transform.position,consumable.transform.position);
                if (distance < closestConsumable)
                {
                    closestConsumable = distance;
                    mTarget = consumable;
                }
            }
        }

        if (mTarget != null)
        {
            mAgent.SetDestination(mTarget.transform.position);
            mAgent.isStopped = false;
            Debug.DrawLine(mTarget.transform.position, mTarget.transform.position + 5 * Vector3.up);
        }

        return mTarget;
    }
}
