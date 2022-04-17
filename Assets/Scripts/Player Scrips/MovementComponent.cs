using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementComponent : MonoBehaviour
{
    //Movement variables
    [SerializeField]
    float walkSpeed = 5;
    [SerializeField]
    float runSpeed = 10;
    [SerializeField]
    float jumpForce = 5;

    //components
    PlayerController playerController;
    Rigidbody rigidBody;
    Animator animator;
    public GameObject followTransform;

    //Movement references
    Vector2 inputVector = Vector2.zero;
    Vector3 moveDirection = Vector3.zero;
    Vector2 lookInput = Vector2.zero;
    public float aimSensativity = 1;

    public bool mIsSlowed = false;
    public float mSlowTimer = 0.2f;
    public float mSlowAmount = 0.625f;

    //Hash values for animator
    public readonly int movementXHash = Animator.StringToHash("MovementX");
    public readonly int movementYHash = Animator.StringToHash("MovementY");
    public readonly int isJumpingHash = Animator.StringToHash("isJumping");
    public readonly int isRunningHash = Animator.StringToHash("isRunning");
    public readonly int aimOffsetHash = Animator.StringToHash("VerticalAim");

    float aimOffset;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
        rigidBody = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!GameManager.instance.cursorActive)
        {
            AppEvents.InvokeMouseCursorEnabled(false);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (playerController.isDead)
            return;

        followTransform.transform.rotation *= Quaternion.AngleAxis(lookInput.x * aimSensativity, Vector3.up);
        followTransform.transform.rotation *= Quaternion.AngleAxis(lookInput.y * aimSensativity, Vector3.left);

        var angles = followTransform.transform.localEulerAngles;
        angles.z = 0;
        var angle = followTransform.transform.localEulerAngles.x;

        float min = -20;
        float max = 40.0f;
        float range = max - min;
        float offsetToZero = 0 - min;
        float aimAngle = followTransform.transform.localEulerAngles.x;
        aimAngle = (aimAngle > 180) ? aimAngle - 360 : aimAngle;
        float val = (aimAngle + offsetToZero) / (range);

        animator.SetFloat(aimOffsetHash, val);

        if (angle > 180 && angle < 340)
        {
            angles.x = 340;
        }
        else if (angle < 180 && angle > 40)
        {
            angles.x = 40;
        }

        followTransform.transform.localEulerAngles = angles;
        transform.rotation = Quaternion.Euler(0, followTransform.transform.rotation.eulerAngles.y, 0);
        followTransform.transform.localEulerAngles = new Vector3(angles.x, 0, 0);

        //Sets movement to zero if no input is received 
        if (!(inputVector.magnitude > 0)) moveDirection = Vector3.zero;

        moveDirection = transform.forward * inputVector.y + transform.right * inputVector.x;
        float currentSpeed = playerController.isRunning ? runSpeed : walkSpeed;

        Vector3 movementDirection = Vector3.zero;
        if (mIsSlowed)
        {
            if (movementDirection.magnitude > 0)
            {
                print("moving while slowed");
            }
            movementDirection = moveDirection * currentSpeed * mSlowAmount * Time.fixedDeltaTime;
        }
        else
        {
            movementDirection = moveDirection * currentSpeed * Time.fixedDeltaTime;
        }

        rigidBody.AddForce(movementDirection, ForceMode.VelocityChange);

        //Assist in slowing player 
        rigidBody.velocity *= 0.97f;
    }

    public void BeingSlowedEffect()
    {
        CancelInvoke();
        mIsSlowed = true;
        Invoke(nameof(EndSlowEffect), mSlowTimer);
    }

    void EndSlowEffect()
    {
        mIsSlowed = false;
    }    

    public void OnMovement(InputValue value)
    {
        if (playerController.isDead)
            return;
        inputVector = value.Get<Vector2>();
        animator.SetFloat(movementXHash, inputVector.x);
        animator.SetFloat(movementYHash, inputVector.y);
    }
    public void OnRun(InputValue value)
    {
        if (playerController.isDead)
            return;
        playerController.isRunning = value.isPressed;
        animator.SetBool(isRunningHash, playerController.isRunning);
    }
    public void OnJump(InputValue value)
    {
        if (playerController.isDead)
            return;
        if (!playerController.isJumping)
        {
            rigidBody.AddForce((transform.up + moveDirection) * jumpForce, ForceMode.Impulse);
        }
    }

    public void OnAim(InputValue value)
    {
        if (playerController.isDead)
            return;
        playerController.isAiming = value.isPressed;
    }
    public void OnLook(InputValue value)
    {
        if (playerController.isDead)
            return;
        lookInput = value.Get<Vector2>();
        //If we aim up down adjust animations
    }


    private void OnCollisionEnter(Collision collision)
    {
        //Checks if player is grounded
        if (collision.gameObject.CompareTag("Ground"))
        {
            playerController.isJumping = false;
            animator.SetBool(isJumpingHash, playerController.isJumping);
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        //Checks if player is not grounded
        if (collision.gameObject.CompareTag("Ground"))
        {
            playerController.isJumping = true;
            animator.SetBool(isJumpingHash, playerController.isJumping);
        }
    }
}