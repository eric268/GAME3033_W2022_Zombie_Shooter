using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using AIBehaviourTree;

public class LeonAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public SphereCollider collider;
    public Node topNode;
    public LeonAISensing leonAISensing;

    public GameObject currentTargetZombie;
    public WayPoint[] wayPointArray;
    public WayPoint currentWaypointTarget;
    public Vector3 targetPosition;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        leonAISensing = GetComponentInChildren<LeonAISensing>();
        wayPointArray = FindObjectsOfType<WayPoint>();
        currentWaypointTarget = wayPointArray[Random.Range(0, wayPointArray.Length)];
        ConstructBehaviourTree();

    }

    // Update is called once per frame
    void Update()
    {
        topNode.Evaluate();
    }

    void ConstructBehaviourTree()
    {
        SelectWaypoint selectNewWaypointNode = new SelectWaypoint(this, wayPointArray, currentWaypointTarget);
        MoveTo moveToWaypointNode = new MoveTo(this, agent, 5.0f);
        FindTargetZombie findTargetZombie = new FindTargetZombie(this);

        Sequence moveAndShootSequence = new Sequence(new List<Node> { moveToWaypointNode, findTargetZombie });

        topNode = new Selector(new List<Node> { moveAndShootSequence, selectNewWaypointNode });
    }
}
