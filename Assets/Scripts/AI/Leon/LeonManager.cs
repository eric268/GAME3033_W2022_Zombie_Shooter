using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeonManager : MonoBehaviour
{
    Animator animator;
    private readonly int movementXHash = Animator.StringToHash("MovementX");
    private readonly int isRunningHash = Animator.StringToHash("isRunning");
    private readonly int verticalAimHash = Animator.StringToHash("verticalAim");

    // Start is called before the first frame update

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        //All just temporary values will be properly setup later
        if (animator)
        {
            animator.SetFloat(movementXHash, 1.0f);
            animator.SetBool(isRunningHash, true);
            animator.SetFloat(verticalAimHash, 0.5f);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
