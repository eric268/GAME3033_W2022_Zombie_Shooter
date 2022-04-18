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
    public GameObject mBettyReference;

    // Start is called before the first frame update
    void Start()
    {
        mLeonController = GetComponentInParent<LeonController>();
        agent = GetComponentInParent<NavMeshAgent>();
        leonAISensing = FindObjectOfType<LeonAISensing>();
        wayPointArray = FindObjectsOfType<WayPoint>();
        FChangeState = mLeonController.OnStateChange;
        currentWaypointTarget = wayPointArray[UnityEngine.Random.Range(0, wayPointArray.Length)];
        mConsumableArray = GameObject.FindGameObjectsWithTag("Consumable");
        mWeaponPickupArray = GameObject.FindGameObjectsWithTag("Weapon");
        mHealthComponent = GetComponentInParent<HealthComponent>();
        mWeaponHolder = GetComponentInParent<WeaponHolder>();
        mBettyReference = GameObject.FindGameObjectWithTag("Betty");
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
    }

    void RunBehaviourTree()
    {
        topNode.Evaluate();
    }

    void ConstructBehaviourTree()
    {
        SelectWaypoint selectNewWaypointNode = new SelectWaypoint(this, wayPointArray, currentWaypointTarget, 100.0f, EQSNodeType.Filter_And_Score);
        MoveTo moveToWaypointNode = new MoveTo(this, agent, 2.0f);
        FindTarget findTargetNode = new FindTarget(this, mBettyReference);
        IsLeonDeadNode isLeonDeadNode = new IsLeonDeadNode(this, agent);
        FindWeaponNode findWeaponNode = new FindWeaponNode(this, agent, mWeaponPickupArray);
        FindConsumableNode findConsumableNode = new FindConsumableNode(this, agent, mConsumableArray);
        FindBetterWeapon findBetterWeaponNode = new FindBetterWeapon(this, agent, mWeaponPickupArray, 15);

        Selector findItemIfNeeded = new Selector(new List<Node> { findWeaponNode, findConsumableNode, findBetterWeaponNode });
        Selector movementSelector = new Selector(new List<Node> { findItemIfNeeded, moveToWaypointNode, selectNewWaypointNode });
        Sequence combateSequence = new Sequence(new List<Node> { findTargetNode, movementSelector });
        
        topNode = new Selector(new List<Node> { isLeonDeadNode, combateSequence });

        //Create sequencer for topNode
        //Contains selector above and a selector of finding and setting the current target
    }

    private void OnDestroy()
    {
        FChangeState -= mLeonController.OnStateChange;
    }
}
