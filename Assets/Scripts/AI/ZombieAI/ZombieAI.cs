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
    public float currentHealth;
    private Node topNode;
    ZombieController zombieController;
    public Action<ZombieState> ChangeZombieState;
    // Start is called before the first frame update

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        bettyReference = GameObject.FindGameObjectWithTag("Betty");
        leonReference = GameObject.FindGameObjectWithTag("Leon");
        zombieController = GetComponent<ZombieController>();
        ChangeZombieState = zombieController.ChangeZombieState;
        currentTarget = bettyReference;
    }
    void Start()
    {
        attackRange = 2.0f;
        currentHealth = 100.0f;
        BuildBehaviourTree();
    }

    // Update is called once per frame
    void Update()
    {
        topNode.Evaluate();   
    }

    void BuildBehaviourTree()
    {
        FindClosestTarget findClosestTargetNode = new FindClosestTarget(this, bettyReference, leonReference, attackRange);
        ZombieMoveTo moveToTargetNode = new ZombieMoveTo(this, agent, attackRange);
        AttackTarget attackTargetNode = new AttackTarget(this, agent);
        IsDeadNode isZombieDeadNode = new IsDeadNode(this, agent);

        Sequence findAndMoveToTargetSequence = new Sequence(new List<Node> { findClosestTargetNode, moveToTargetNode });
        Selector zombieBehaviourSequence = new Selector(new List<Node> {isZombieDeadNode, findAndMoveToTargetSequence, attackTargetNode });

        topNode = zombieBehaviourSequence;
    }
}
