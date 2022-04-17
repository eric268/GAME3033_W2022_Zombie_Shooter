using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using AIBehaviourTree;
using System;

public class ZombieAI : MonoBehaviour
{
    NavMeshAgent agent;
    GameObject bettyReference;
    GameObject leonReference;
    public GameObject currentTarget;
    float attackRange;
    private Node topNode;
    ZombieController zombieController;
    public Action<ZombieState> FChangeState;
    // Start is called before the first frame update

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        bettyReference = GameObject.FindGameObjectWithTag("Betty");
        leonReference = GameObject.FindGameObjectWithTag("Leon");
        zombieController = GetComponent<ZombieController>();
        FChangeState = zombieController.ChangeZombieState;
        currentTarget = bettyReference;
    }
    void Start()
    {
        attackRange = 1.0f;
        BuildBehaviourTree();

        //InvokeRepeating(nameof(RunBehaviourTree), 0.0f, 0.25f);
    }

    private void Update()
    {
        //RunBehaviourTree();
    }

    void RunBehaviourTree()
    {
        topNode.Evaluate();
    }

    void BuildBehaviourTree()
    {
        FindClosestTarget findClosestTargetNode = new FindClosestTarget(this, bettyReference, leonReference, attackRange);
        ZombieMoveTo moveToTargetNode = new ZombieMoveTo(this, agent, attackRange);
        AttackTarget attackTargetNode = new AttackTarget(this, agent);
        IsZombieDeadNode isZombieDeadNode = new IsZombieDeadNode(this, agent);

        Sequence findAndMoveToTargetSequence = new Sequence(new List<Node> { findClosestTargetNode, moveToTargetNode });
        Selector zombieBehaviourSequence = new Selector(new List<Node> {isZombieDeadNode, findAndMoveToTargetSequence, attackTargetNode });

        topNode = zombieBehaviourSequence;
    }

    private void OnDestroy()
    {
        FChangeState = zombieController.ChangeZombieState;
    }
}
