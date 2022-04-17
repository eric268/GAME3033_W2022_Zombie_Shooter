using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AIBehaviourTree;
using System.Linq;

public class SelectWaypoint : EQSNode<WayPoint>
{
    LeonAI leonAI;
    WayPoint currentWaypoint;
    WayPoint[] waypointArray;

    float minRange;
    float maxRange;
    float mIdealAngle;
    float scoreForLessZombiesMultiplier;
    float scoreForRightAngleMultiplier;

    public SelectWaypoint(LeonAI _leonAI, WayPoint[] _waypointArray, WayPoint _currentWaypoint, float idealAngle, EQSNodeType _EQSType)
    {
        leonAI = _leonAI;
        waypointArray = _waypointArray;
        currentWaypoint = _currentWaypoint;
        mEQSNodeType = _EQSType;
        minRange = 0;
        maxRange = 100;
        scoreForLessZombiesMultiplier = 1;
        scoreForRightAngleMultiplier = 2;
        mIdealAngle = idealAngle;
    }

    public override NodeState Evaluate()
    {
        leonAI.currentWaypointTarget = EvaluateQuery();
        return NodeState.SUCCESS;
    }

    public override WayPoint EvaluateQuery()
    {
        WayPoint bestWaypoint = null;
        float bestScore = 0;

        //Create dictionary of way points with their EQS score as value
        List<EQSContainer<WayPoint>> wayPointList = waypointArray.Select(x => new EQSContainer<WayPoint>(x, 0.0f)).ToList();

        //Filter necessary way points out if they do not meet requirements
        for (int i = 0; i < waypointArray.Length; i++)
        {
            //Filtering based on min and max range
            float distance = Vector3.Distance(leonAI.transform.position, waypointArray[i].transform.position);
            if (distance < minRange || distance > maxRange)
            {
                WayPoint wP = waypointArray[i];
                wayPointList.Remove(wayPointList.Find(i => i.Key == wP));
            }
        }

        //Score each way point based on criteria 
        foreach (EQSContainer<WayPoint> wayPoint in wayPointList)
        {
            float mActiveZombies = (GameManager.mNumberActiveZombies == 0.0f) ? 1.0f : GameManager.mNumberActiveZombies;
            //Adds score for way points having less zombies 
            wayPoint.Score = ((1.0f - wayPoint.Key.mNumZombiesCloseBy / mActiveZombies) * scoreForLessZombiesMultiplier);

            //Adds score for way points being to the right or left of Leon's current direction
            Vector3 targetDir = wayPoint.Key.transform.position - leonAI.transform.position;
            float angle = Vector3.Angle(targetDir, leonAI.transform.forward);
            float score = Mathf.Abs((angle - mIdealAngle)) / mIdealAngle * scoreForRightAngleMultiplier;
            wayPoint.Score += score;
        }

        //Find and return way point with the best score
        foreach (EQSContainer<WayPoint> wayPoint in wayPointList)
        {
            if (wayPoint.Score > bestScore)
            {
                bestScore = wayPoint.Score;
                bestWaypoint = wayPoint.Key;
            }
        }
        return bestWaypoint;
    }
}

