using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AIBehaviourTree;
using UnityEngine.AI;

public class FindClosestTarget : Node
{
    ZombieAI aIReference;
    GameObject bettyReference;
    GameObject leonReference;
    GameObject currentTarget;

    float attackRange;

    public FindClosestTarget(ZombieAI _aIReference, GameObject betty, GameObject leon, float range)
    {
        aIReference = _aIReference;
        bettyReference = betty;
        leonReference = leon;
        attackRange = range;
    }


    public override NodeState Evaluate()
    {
        if (!bettyReference && !leonReference)
        {
            aIReference.currentTarget = null;
            return NodeState.FAILURE;
        }

        float distanceToBetty = GetPathLength(GetPath(bettyReference));
        float distanceToLeon = GetPathLength(GetPath(leonReference));

        aIReference.currentTarget = (distanceToBetty < distanceToLeon) ? bettyReference : leonReference;
        return NodeState.SUCCESS;
    }

    NavMeshPath GetPath(GameObject targetPostion)
    {
        NavMeshPath path = new NavMeshPath();
        NavMesh.CalculatePath(aIReference.transform.position, targetPostion.transform.position, NavMesh.AllAreas, path);
        return path;
    }

    float GetPathLength(NavMeshPath path)
    {
        float length = 0.0f;
        if (path.status != NavMeshPathStatus.PathInvalid)
        {
            for (int i = 1; i < path.corners.Length; i++)
            {
                length += Vector3.Distance(path.corners[i - 1], path.corners[i]);
            }
        }
        return length;
    }
}
