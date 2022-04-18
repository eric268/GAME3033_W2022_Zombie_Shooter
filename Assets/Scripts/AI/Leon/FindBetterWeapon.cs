using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AIBehaviourTree;
using UnityEngine.AI;

public class FindBetterWeapon : Node
{
    LeonAI leonAI;
    NavMeshAgent navMeshAgent;
    LeonAISensing leonAISensing;
    GameObject[] weaponPickUpArray;
    float maxDistanceToGetGun;
    int mCurrentGunLevel;

    public FindBetterWeapon(LeonAI _leonAI, NavMeshAgent agent, GameObject[] _weaponPickUpArray, float _maxDistance)
    {
        leonAI = _leonAI;
        navMeshAgent = agent;
        leonAISensing = leonAI.leonAISensing;
        weaponPickUpArray = _weaponPickUpArray;
        maxDistanceToGetGun = _maxDistance;
    }

    bool FindBetterGunCloseBY()
    {
        Vector3 leonPos = leonAI.transform.position;
        GameObject betterGun = null;

        if (!leonAI.mWeaponHolder.equippedWeapon)
        {
            mCurrentGunLevel = -1;
        }
        else
        {
            mCurrentGunLevel = leonAI.mWeaponHolder.equippedWeapon.weaponStats.weaponValueToAI;
        }

        //Only want to look if no zombies very close
        if (leonAISensing.colliderArray[(int)AISensingType.Close_By].zombieSensingCollider.Count == 0)
        {
            foreach(GameObject obj in weaponPickUpArray)
            {
                ItemPickupComponent item = obj.GetComponent<ItemPickupComponent>();
                if (item.mIsAvailable && item.mItemValue >= leonAI.mWeaponHolder.equippedWeapon.weaponStats.weaponValueToAI)
                {
                    if (Vector3.Distance(leonAI.transform.position, obj.transform.position) <= maxDistanceToGetGun)
                    {
                        navMeshAgent.SetDestination(obj.transform.position);
                        navMeshAgent.isStopped = false;
                        return true;
                    }
                }
            }
        }

        return false;
    }

    public override NodeState Evaluate()
    {
        return (FindBetterGunCloseBY()) ? NodeState.SUCCESS : NodeState.FAILURE;
    }
}
