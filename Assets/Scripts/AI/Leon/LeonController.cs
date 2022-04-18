using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum LeonState
{
    None = -1,
    Idle,
    Slowed,
    Running,
    Dying,
    Num_Leon_States
}


public class LeonController : MonoBehaviour
{
    Animator animator;
    NavMeshAgent mNavMeshAgent;
    private readonly int movementXHash = Animator.StringToHash("MovementX");
    private readonly int isRunningHash = Animator.StringToHash("isRunning");
    private readonly int verticalAimHash = Animator.StringToHash("VerticalAim");
    public LeonState mLeonState;
    public float mSlowedSpeed = 2.5f;
    public float mRunSpeed = 4.0f;
    public float mSlowTimer = 0.2f;
    public bool mIsDead;
    WeaponHolder mWeaponHolder;
    PlayerController mPlayerController;
    public GameObject mWeaponSocket;
    public Transform mFiringLocation;
    LeonAI mLeonAI;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        mNavMeshAgent = GetComponent<NavMeshAgent>();
        mWeaponHolder = GetComponent<WeaponHolder>();
        mPlayerController = GetComponent<PlayerController>();
        mLeonAI = GetComponent<LeonAI>();
    }

    void Start()
    {
        mLeonState = LeonState.Running;
        if (animator)
        {
            OnStateChange(mLeonState);
            animator.SetFloat(verticalAimHash, 0.5f);
        }

        //InvokeRepeating(nameof(CheckIfCanFire), 0.0f, 0.3f);
    }

    private void Update()
    {
        if (mLeonAI.currentTarget && mWeaponHolder.equippedWeapon)
        {
            InvokeRepeating(nameof(CheckIfCanFire), 0.0f, mWeaponHolder.equippedWeapon.weaponStats.fireRate);
        }
        else
        {
            CancelInvoke();
            if (mLeonState == LeonState.Slowed)
                Invoke(nameof(OnSlowEnded), mSlowTimer);
        }
    }

    // Start is called before the first frame update

    public void OnStateChange(LeonState state)
    {
        switch (state)
        {
            case LeonState.Idle:
                mLeonState = LeonState.Idle;
                animator.SetFloat(movementXHash, 0.0f);
                animator.SetBool(isRunningHash, false);
                break;
            case LeonState.Slowed:
                CancelInvoke();
                Invoke(nameof(OnSlowEnded), mSlowTimer);
                mLeonState = LeonState.Slowed;
                animator.SetFloat(movementXHash, 1.0f);
                animator.SetBool(isRunningHash, false);
                mNavMeshAgent.speed = mSlowedSpeed;
                break;
            case LeonState.Running:
                mLeonState = LeonState.Running;
                animator.SetFloat(movementXHash, 1.0f);
                animator.SetBool(isRunningHash, true);
                mNavMeshAgent.speed = mRunSpeed;
                break;
            case LeonState.Dying:
                mLeonState = LeonState.Dying;
                animator.SetFloat(movementXHash, 0.0f);
                animator.SetBool(isRunningHash, false);
                mIsDead = true;
                animator.Play("Death");
                break;
        }
    }

    void CheckIfCanFire()
    {
        if (mWeaponHolder.equippedWeapon != null)
        {
            mWeaponHolder.LeonStartFiring();
        }
    }

    public void OnSlowEnded()
    {
        OnStateChange(LeonState.Running);
    }

    public void WeaponPickedUp()
    {
        if (mWeaponHolder.equippedWeapon != null)
        {
            Invoke(nameof(OnSlowEnded), mSlowTimer);
            InvokeRepeating(nameof(CheckIfCanFire), 0.0f, mWeaponHolder.equippedWeapon.weaponStats.fireRate);
        }
    }
}
