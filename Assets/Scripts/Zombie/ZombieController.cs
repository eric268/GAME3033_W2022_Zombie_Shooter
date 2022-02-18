using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ZombieState
{
    Dead, 
    Running,
    Attacking
}
public class ZombieController : MonoBehaviour
{
    Animator animator;
    private readonly int isDeadHash = Animator.StringToHash("isDead");
    private readonly int isRunningHash = Animator.StringToHash("isRunning");
    private readonly int isAttackingHash = Animator.StringToHash("isAttacking");

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
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
}
