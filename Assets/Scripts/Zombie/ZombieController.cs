using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum ZombieState
{
    Dead, 
    Running,
    Attacking
}
public class ZombieController : MonoBehaviour
{
    
    LevelManager mLevelManager;
    Action FZombieDied;
    Animator animator;
    ZombieAttack mZombieAttack;
    NavMeshAgent mNavMeshAgent;
    SensingColliderContainer[] mSensingColliderContainers;
    Action<Collider> FRemoveDeadZombieCollider;
    public ZombieState mZombieState;
    private readonly int isDeadHash = Animator.StringToHash("isDead");
    private readonly int isRunningHash = Animator.StringToHash("isRunning");
    private readonly int isAttackingHash = Animator.StringToHash("isAttacking");

    public bool mAttackStarted = false;
    public int mAvoidanceValue;
    public int mAttackDamage;

    // Start is called before the first frame update
    void Start()
    {
        mNavMeshAgent = GetComponent<NavMeshAgent>();
        mZombieAttack = GetComponentInChildren<ZombieAttack>();
        animator = GetComponent<Animator>();
        mLevelManager = FindObjectOfType<LevelManager>();
        mAvoidanceValue = mNavMeshAgent.avoidancePriority;
        FZombieDied = mLevelManager.ZombieKilled;
        mSensingColliderContainers = FindObjectsOfType<SensingColliderContainer>();

        for (int i = 0; i < mSensingColliderContainers.Length; i++)
        {
            FRemoveDeadZombieCollider += mSensingColliderContainers[i].RemoveDeadZombieCollider;
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ChangeZombieState(ZombieState state)
    {
        animator.SetBool(isDeadHash, false);
        animator.SetBool(isRunningHash, false);
        animator.SetBool(isAttackingHash, false);
        switch (state)
        {
            case ZombieState.Dead:
                if (GetComponent<CapsuleCollider>())
                    FRemoveDeadZombieCollider(GetComponent<CapsuleCollider>());
                animator.SetBool(isDeadHash, true);
                mZombieState = ZombieState.Dead;
                FZombieDied();
                break;
            case ZombieState.Running:
                mZombieState = ZombieState.Running;
                animator.SetBool(isRunningHash, true);
                mNavMeshAgent.avoidancePriority = mAvoidanceValue;
                break;
            case ZombieState.Attacking:
                mZombieState = ZombieState.Attacking;
                animator.SetBool(isAttackingHash, true);
                mNavMeshAgent.avoidancePriority = mAvoidanceValue + 10;
                break;
        }

    }

    public void EnableHitCollider()
    {
        mZombieAttack.EnableHitCollider();
    }

    public void DisableHitCollider()
    {
        mZombieAttack.DisableHitCollider();
    }

    public void DisableCapsuleCollider()
    {
        GetComponent<CapsuleCollider>().enabled = false;
        GetComponent<Rigidbody>().useGravity = false;
        gameObject.isStatic = true;
    }
    public void StartZombieRemovalProcess()
    {
        StartCoroutine("RemoveDeadZombie");
    }
    IEnumerator RemoveDeadZombie()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        FZombieDied -= mLevelManager.ZombieKilled;

        for (int i = 0; i < mSensingColliderContainers.Length; i++)
        {
            FRemoveDeadZombieCollider -= mSensingColliderContainers[i].RemoveDeadZombieCollider;
        }
    }
}
