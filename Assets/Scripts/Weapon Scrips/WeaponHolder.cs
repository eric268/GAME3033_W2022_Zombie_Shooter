using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponHolder : MonoBehaviour
{
    [Header("WeaponToSpawn"), SerializeField]
    public GameObject weaponToSpawn;

    public readonly int isFiringHash = Animator.StringToHash("isFiring");
    public readonly int isReloadingHash = Animator.StringToHash("isReloading");

    public PlayerController playerController;
    Animator animator;

    public Sprite corssHairImage;
    WeaponComponent equippedWeapon;

    [SerializeField]
    GameObject weaponSocketLocation;

    bool firingPressed = false;

    [SerializeField]
    Transform gripSocketLocation;
    // Start is called before the first frame update

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        animator = GetComponent<Animator>();
    }
    void Start()
    {

        GameObject spawnedWeapon = Instantiate(weaponToSpawn, weaponSocketLocation.transform.position, weaponSocketLocation.transform.rotation, weaponSocketLocation.transform);
        equippedWeapon = spawnedWeapon.GetComponent<WeaponComponent>();
        gripSocketLocation = equippedWeapon.gripLocation;
        equippedWeapon.Initalize(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnAnimatorIK(int layerIndex)
    {
        if (!playerController.isReloading)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
            animator.SetIKPosition(AvatarIKGoal.LeftHand, gripSocketLocation.transform.position);
        }
    }
    public void OnReload(InputValue value)
    {
        playerController.isReloading = value.isPressed;
        animator.SetBool(isReloadingHash, playerController.isReloading);
        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
    }
    public void OnFire(InputValue value)
    {
        playerController.isFiring = value.isPressed;
        firingPressed = playerController.isFiring;
        if (firingPressed)
        {
            StartFiring();
        }
        else
        {
            StopFiring();
        }
        //equippedWeapon.StartFiringWeapon();
    }

    void StartFiring()
    {
        if (equippedWeapon.weaponStats.bulletInClip > 0)
        {
            animator.SetBool(isFiringHash, playerController.isFiring);
            playerController.isFiring = true;
            equippedWeapon.StartFiringWeapon();
        }        

    }

    void StopFiring()
    {
        animator.SetBool(isFiringHash, false);
        playerController.isFiring = false;
        equippedWeapon.StopFiringWeapon();
    }
}
