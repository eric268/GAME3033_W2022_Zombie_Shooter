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
public class ZombieController : MonoBehaviour, IDamageable
{
    Animator animator;
    ZombieAttack mZombieAttack;
    private readonly int isDeadHash = Animator.StringToHash("isDead");
    private readonly int isRunningHash = Animator.StringToHash("isRunning");
    private readonly int isAttackingHash = Animator.StringToHash("isAttacking");

    //public CapsuleCollider leftHandCollider;
    //public CapsuleCollider rightHandCollider;

    public bool mAttackStarted = false;

    public int mAttackDamage;

    public int mZombieStartingHealth;
    public int mZombieCurrentHealth;

    // Start is called before the first frame update
    void Start()
    {
        mZombieAttack = GetComponentInChildren<ZombieAttack>();
        animator = GetComponent<Animator>();
        mZombieCurrentHealth = mZombieStartingHealth;
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
                animator.SetBool(isDeadHash, true);
                break;
            case ZombieState.Running:
                animator.SetBool(isRunningHash, true);
                break;
            case ZombieState.Attacking:
                animator.SetBool(isAttackingHash, true);
                break;
        }

    }

    public void TakeDamage(int damage)
    {
        mZombieCurrentHealth -= damage;

        if (mZombieCurrentHealth <= 0)
        {
            Destroy(gameObject);
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
        //GetComponent<CapsuleCollider>().enabled = false;
        //GetComponent<NavMeshAgent>().enabled = false;
        //StartCoroutine("DecreaseZombieHeight");
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
}
