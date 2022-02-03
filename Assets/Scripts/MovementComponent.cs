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

    //Hash values for animator
    public readonly int movementXHash = Animator.StringToHash("MovementX");
    public readonly int movementYHash = Animator.StringToHash("MovementY");
    public readonly int isJumpingHash = Animator.StringToHash("isJumping");
    public readonly int isRunningHash = Animator.StringToHash("isRunning");
    public readonly int isFiringHash = Animator.StringToHash("isFiring");
    public readonly int isReloadingHash = Animator.StringToHash("isReloading");

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
        rigidBody = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        followTransform.transform.rotation *= Quaternion.AngleAxis(lookInput.x * aimSensativity, Vector3.up);
        followTransform.transform.rotation *= Quaternion.AngleAxis(lookInput.y * aimSensativity, Vector3.left);

        var angles = followTransform.transform.localEulerAngles;
        angles.z = 0;
        var angle = followTransform.transform.localEulerAngles.x;

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

        Vector3 movementDirection = moveDirection * currentSpeed * Time.fixedDeltaTime;

        rigidBody.AddForce(movementDirection, ForceMode.VelocityChange);
        
        //Assist in slowing player 
        rigidBody.velocity *= 0.97f;
    }

    public void OnMovement(InputValue value)
    {
        inputVector = value.Get<Vector2>();
        animator.SetFloat(movementXHash, inputVector.x);
        animator.SetFloat(movementYHash, inputVector.y);
    }
    public void OnRun(InputValue value)
    {
        playerController.isRunning = value.isPressed;
        animator.SetBool(isRunningHash, playerController.isRunning);
    }
    public void OnJump(InputValue value)
    {
        if (!playerController.isJumping)
        {
            rigidBody.AddForce((transform.up + moveDirection) * jumpForce, ForceMode.Impulse);
        }
    }

    public void OnAim(InputValue value)
    {
        playerController.isAiming = value.isPressed;
    }
    public void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();
        //If we aim up down adjust animations
    }
    public void OnReload(InputValue value)
    {
        playerController.isReloading = value.isPressed;
        animator.SetBool(isReloadingHash, playerController.isReloading);
    }
    public void OnFire(InputValue value)
    {
        playerController.isFiring = value.isPressed;
        animator.SetBool(isFiringHash, playerController.isFiring);
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
