using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AIBehaviourTree;
public class IsWithinRange : Node
{
    LeonAI leonAI;
    float distanceRange;
    public IsWithinRange(LeonAI _leonAI, float _distanceRange)
    {
        leonAI = _leonAI;
        distanceRange = _distanceRange;
    }
    public override NodeState Evaluate()
    {
        float distance = Vector3.Distance(leonAI.transform.position, leonAI.currentWaypointTarget.position);
        return distance <= distanceRange ? NodeState.SUCCESS : NodeState.FAILURE;   
    }
}
