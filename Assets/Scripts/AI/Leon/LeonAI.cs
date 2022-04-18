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
    public GameObject currentTarget;
    public WayPoint[] wayPointArray;
    public WayPoint currentWaypointTarget;
    public Vector3 targetPosition;
    public LeonController mLeonController;
    public Action<LeonState> FChangeState;
    public GameObject[] mConsumableArray;
    public GameObject[] mWeaponPickupArray;
    public ItemPickupComponent[] testComp;
    public int mLowHealthLevel;

    public HealthComponent mHealthComponent;
    public WeaponHolder mWeaponHolder;

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
        mHealthComponent = GetComponent<HealthComponent>();
        mWeaponHolder = GetComponent<WeaponHolder>();

        ConstructBehaviourTree();
        InvokeRepeating(nameof(RunBehaviourTree), 0.0f, 0.25f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (mLeonController.mIsDead)
        {
            CancelInvoke();
        }

        if (currentTarget)
        {
            transform.LookAt(currentTarget.transform);
        }
    }

    void RunBehaviourTree()
    {
        topNode.Evaluate();
    }

    void ConstructBehaviourTree()
    {
        SelectWaypoint selectNewWaypointNode = new SelectWaypoint(this, wayPointArray, currentWaypointTarget, 100.0f, EQSNodeType.Filter_And_Score);
        MoveTo moveToWaypointNode = new MoveTo(this, agent, 25.0f);
        FindTargetZombie findTargetZombie = new FindTargetZombie(this);
        IsLeonDeadNode isLeonDeadNode = new IsLeonDeadNode(this, agent);

        FindWeaponNode findWeaponNode = new FindWeaponNode(this, agent, mWeaponPickupArray);
        FindConsumableNode findConsumableNode = new FindConsumableNode(this, agent, mConsumableArray);

        Sequence moveAndShootSequence = new Sequence(new List<Node> { moveToWaypointNode, findTargetZombie });
        Selector findConsumableIfNeeded = new Selector(new List<Node> { findWeaponNode, findConsumableNode });

        topNode = new Selector(new List<Node> { isLeonDeadNode, findConsumableIfNeeded,  moveAndShootSequence, selectNewWaypointNode });

        //Create sequencer for topNode
        //Contains selector above and a selector of finding and setting the current target
    }

    private void OnDestroy()
    {
        FChangeState -= mLeonController.OnStateChange;
    }
}
