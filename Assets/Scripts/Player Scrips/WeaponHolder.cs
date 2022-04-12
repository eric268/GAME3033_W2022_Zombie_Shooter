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
    GameObject spawnedWeapon;
    public Sprite corssHairImage;
    public WeaponComponent equippedWeapon;
    public WeaponPanelUI weaponPanelUI;
    [SerializeField]
    GameObject weaponSocketLocation;

    bool firingPressed = false;
    bool wasFiring;
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
        //EquipWeapon();

       // spawnedWeapon = Instantiate(weaponToSpawn, weaponSocketLocation.transform.position, weaponSocketLocation.transform.rotation, weaponSocketLocation.transform);
        //equippedWeapon = spawnedWeapon.GetComponent<WeaponComponent>();
        //gripSocketLocation = equippedWeapon.gripLocation;
        //equippedWeapon.Initalize(this);
        //PlayerEvents.InvokeOnWeaponEquipped(equippedWeapon);
    }

    // Update is called once per frame
    void Update()
    {
        //print(playerController.isFiring);
    }
    private void OnAnimatorIK(int layerIndex)
    {
        if (equippedWeapon)
        {
            if (!playerController.isReloading)
            {
                animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
                animator.SetIKPosition(AvatarIKGoal.LeftHand, gripSocketLocation.transform.position);
            }
        }
    }

    public void OnFire(InputValue value)
    {
        if (!equippedWeapon)
            return;
        playerController.isFiring = value.isPressed;
        firingPressed = playerController.isFiring;
        if (!playerController.isReloading && firingPressed)
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

        if (equippedWeapon.weaponStats.bulletInClip <= 0)
        {
            StartReloading();
            return;
        }
        animator.SetBool(isFiringHash, playerController.isFiring);
        playerController.isFiring = true;
        equippedWeapon.StartFiringWeapon();
    }

    void StopFiring()
    {

        animator.SetBool(isFiringHash, false);
        playerController.isFiring = false;
        equippedWeapon.StopFiringWeapon();
    }

    public void OnReload(InputValue value)
    {
        if (!equippedWeapon)
            return;

        if (!playerController.isReloading)
        {
            playerController.isReloading = value.isPressed;
            StartReloading();
        }
    }

    public void StartReloading()
    {
        playerController.isReloading = true;
        if (playerController.isFiring)
        {
            StopFiring();
            wasFiring = true;
        }
        if (equippedWeapon.weaponStats.totalBullets <= 0)
               return;
        equippedWeapon.StartReloading();

        animator.SetBool(isReloadingHash, true);

        InvokeRepeating(nameof(StopReloading), 0, 0.1f);
    }

    public void StopReloading()
    {
        if (animator.GetBool(isReloadingHash)) return;

        playerController.isReloading = false;
        animator.SetBool(isReloadingHash, false);
        equippedWeapon.StopReloading();
        CancelInvoke(nameof(StopReloading));

        if (firingPressed)
            StartFiring();
    }

    public void EquipWeapon(WeaponScriptable weaponScriptable)
    {
        if (!weaponScriptable)
            return;

        spawnedWeapon = Instantiate(weaponScriptable.itemPrefab, weaponSocketLocation.transform.position, weaponSocketLocation.transform.rotation, weaponSocketLocation.transform);
        if (!spawnedWeapon)
            return;
        
        equippedWeapon = spawnedWeapon.GetComponent<WeaponComponent>();

        if (!equippedWeapon)
            return;

        equippedWeapon.Initalize(this, weaponScriptable);
        PlayerEvents.InvokeOnWeaponEquipped(equippedWeapon);
        gripSocketLocation = equippedWeapon.gripLocation;
        weaponPanelUI.weaponComponent = equippedWeapon;
    }

    public void UnEquipWeapon()
    {

    }
}
