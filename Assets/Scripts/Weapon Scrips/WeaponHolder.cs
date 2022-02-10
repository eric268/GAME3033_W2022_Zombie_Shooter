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

    PlayerController playerController;
    Animator animator;

    public Sprite corssHairImage;
    WeaponComponent equippedWeapon;

    [SerializeField]
    GameObject weaponSocketLocation;

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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnAnimatorIK(int layerIndex)
    {
        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
        animator.SetIKPosition(AvatarIKGoal.LeftHand, gripSocketLocation.transform.position);
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
}
