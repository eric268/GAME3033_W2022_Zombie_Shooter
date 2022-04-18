using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AIBehaviourTree;

public class FindTarget : Node
{
    LeonAI leonAI;
    LeonAISensing leonAISensing;
    GameObject tempTarget;
    GameObject bettyReference;
    float distance;
    public FindTarget(LeonAI _leonAI, GameObject _bettyReference)
    {
        leonAI = _leonAI;
        leonAISensing = leonAI.leonAISensing;
        bettyReference = _bettyReference;
    }

    GameObject FindClosestZombieWithLOS()
    {
        distance = Mathf.Infinity;
        Vector3 leonPos = leonAI.transform.position;
        tempTarget = null;

        if (leonAISensing.colliderArray[(int)AISensingType.Close_By].zombieSensingCollider.Count > 0)
        {
            SearchColliderForTarget(leonPos, AISensingType.Close_By);
        }
        else
        {
            SearchColliderForTarget(leonPos, AISensingType.Shoot_At);
            
        }
        return tempTarget;
    }

    void SearchColliderForTarget(Vector3 _leonPos, AISensingType sensingType)
    {
        foreach (Collider coll in leonAISensing.colliderArray[(int)sensingType].zombieSensingCollider)
        {
            if (coll != null && coll.gameObject != null)
            {
                Vector3 zombiePos = coll.gameObject.transform.position;
                RaycastHit hit;
                if (Physics.Linecast(_leonPos, zombiePos, out hit))
                {
                    if (hit.collider.CompareTag("Zombie") )
                    {
                        //Debug.Log("ZOMBIE FOUND");
                        float tempDistance = Vector3.Distance(_leonPos, zombiePos);

                        float distanceOfZombieToBetty = Vector3.Distance(bettyReference.transform.position, coll.gameObject.transform.position);
                        if (tempDistance < distance && tempDistance < distanceOfZombieToBetty)
                        {
                            distance = tempDistance;
                            tempTarget = coll.gameObject;
                        }
                    }
                    else if(hit.collider.CompareTag("Betty"))
                    {
                        //Debug.Log("ZOMBIE FOUND");
                        float tempDistance = Vector3.Distance(_leonPos, zombiePos);
                        if (tempDistance < distance)
                        {
                            distance = tempDistance;
                            tempTarget = coll.gameObject;
                        }
                    }
                }
            }
        }
    }

    public override NodeState Evaluate()
    {
        if (!leonAI.mWeaponHolder.equippedWeapon)
        {
            leonAI.currentTarget = null;
            return NodeState.SUCCESS;
        }

        leonAI.currentTarget = FindClosestZombieWithLOS();
        return NodeState.SUCCESS;
    }
}
