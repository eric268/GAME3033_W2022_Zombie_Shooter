using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using AIBehaviourTree;
using System;

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
    public LeonController mLeonController;
    public Action<LeonState> FChangeState;
    public GameObject[] mConsumableArray;
    public GameObject[] mWeaponPickupArray;
    public ItemPickupComponent[] testComp;

    // Start is called before the first frame update
    void Start()
    {
        mLeonController = GetComponent<LeonController>();
        agent = GetComponent<NavMeshAgent>();
        leonAISensing = GetComponentInChildren<LeonAISensing>();
        wayPointArray = FindObjectsOfType<WayPoint>();
        FChangeState = mLeonController.OnStateChange;
        currentWaypointTarget = wayPointArray[UnityEngine.Random.Range(0, wayPointArray.Length)];
        mConsumableArray = GameObject.FindGameObjectsWithTag("Consumable");
        mWeaponPickupArray = GameObject.FindGameObjectsWithTag("Weapon");

        ConstructBehaviourTree();


        InvokeRepeating(nameof(RunBehaviourTree), 0.0f, 0.25f);

    }

    // Update is called once per frame
    void Update()
    {
        if (mLeonController.mIsDead)
        {
            CancelInvoke();
        }
    }

    void RunBehaviourTree()
    {
        topNode.Evaluate();
    }

    void ConstructBehaviourTree()
    {
        SelectWaypoint selectNewWaypointNode = new SelectWaypoint(this, wayPointArray, currentWaypointTarget,100.0f, EQSNodeType.Filter_And_Score);
        MoveTo moveToWaypointNode = new MoveTo(this, agent, 25.0f);
        FindTargetZombie findTargetZombie = new FindTargetZombie(this);
        IsLeonDeadNode isLeonDeadNode = new IsLeonDeadNode(this, agent);

        Sequence moveAndShootSequence = new Sequence(new List<Node> { moveToWaypointNode, findTargetZombie });

        //topNode = new Selector(new List<Node> { isLeonDeadNode, moveAndShootSequence, selectNewWaypointNode });


        //Testing for weapon attachment
        FindConsumableNode findWeaponNode = new FindConsumableNode(this, agent, mWeaponPickupArray);
        FindConsumableNode findConsumableNode = new FindConsumableNode(this, agent, mConsumableArray);
        topNode = new Selector( new List<Node> { findWeaponNode } );
    }

    private void OnDestroy()
    {
        FChangeState -= mLeonController.OnStateChange;
    }
}
