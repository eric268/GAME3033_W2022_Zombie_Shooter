using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AIBehaviourTree;
public class SelectWaypoint : Node
{
    LeonAI leonAI;
    WayPoint currentWaypoint;
    WayPoint[] waypointArray;

    public SelectWaypoint(LeonAI _leonAI, WayPoint[] _waypointArray, WayPoint _currentWaypoint)
    {
        leonAI = _leonAI;
        waypointArray = _waypointArray;
        currentWaypoint = _currentWaypoint;
    }

    public override NodeState Evaluate()
    {
        while (true)
        {
            int waypointCount = waypointArray.Length;
            int index = Random.Range(0, waypointCount);

            if (waypointArray[index] != currentWaypoint)
            {
                leonAI.currentWaypointTarget = waypointArray[index];
                break;
            }
        }
        return NodeState.SUCCESS;
    }
}
