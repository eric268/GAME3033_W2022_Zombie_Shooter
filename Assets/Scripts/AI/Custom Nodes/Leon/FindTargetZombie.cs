using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AIBehaviourTree;

public class FindTargetZombie : Node
{
    LeonAI leonAI;
    LeonAISensing leonAISensing;
    GameObject tempTarget;
    float distance;
    public FindTargetZombie(LeonAI _leonAI)
    {
        leonAI = _leonAI;
        leonAISensing = leonAI.leonAISensing;
    }

    GameObject FindClosestZombieWithLOS()
    {
        distance = Mathf.Infinity;
        Vector3 leonPos = leonAI.transform.position;
        tempTarget = null;
        foreach (Collider coll in leonAISensing.colliderArray[(int)AISensingType.Close_By].zombieSensingCollider)
        {
            //if (coll.gameObject != null)
            {
                Vector3 zombiePos = coll.gameObject.transform.position;
                RaycastHit hit;
                if (Physics.Linecast(leonPos, zombiePos, out hit))
                {
                    if (hit.collider.CompareTag("Zombie"))
                    {
                        //Debug.Log("ZOMBIE FOUND");
                        float tempDistance = Vector3.Distance(leonPos, zombiePos);

                        if (tempDistance < distance)
                        {
                            distance = tempDistance;
                            tempTarget = coll.gameObject;

                            //leonAI.transform.LookAt(coll.transform);

                            //Eventually switch this to setting a firing state for Leon
                            Debug.DrawLine(leonPos, zombiePos, Color.red, 0.3f);
                        }
                    }
                }
            }
        }
        return tempTarget;
    }

    public override NodeState Evaluate()
    {
        leonAI.currentTargetZombie = FindClosestZombieWithLOS();
        return NodeState.SUCCESS;
    }
}
