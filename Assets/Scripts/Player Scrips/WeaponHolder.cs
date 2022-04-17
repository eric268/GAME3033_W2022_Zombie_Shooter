using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum PlayerType
{
    None = -1,
    Betty,
    Leon,
    Num_Player_Types
}

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
    public PlayerType mPlayerType;
    bool firingPressed = false;
    bool wasFiring;
    [SerializeField]
    Transform gripSocketLocation;
    // Start is called before the first frame update

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        animator = GetComponent<Animator>();
        weaponPanelUI = FindObjectOfType<WeaponPanelUI>();
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
        if (!equippedWeapon || playerController.isDead)
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

    public void LeonStartFiring()
    {
        if (!playerController.isReloading)
        {
            StartFiring();
        }
        else
        {
            StopFiring();
        }
    }

    void StartFiring()
    {
        if (playerController.isDead)
            return;
        if (equippedWeapon.weaponStats.bulletInClip <= 0)
        {
            StartReloading();
            return;
        }
        animator.SetBool(isFiringHash, playerController.isFiring);
        playerController.isFiring = true;
        equippedWeapon.StartFiringWeapon();
    }

    public void StopFiring()
    {
        animator.SetBool(isFiringHash, false);
        playerController.isFiring = false;
        if (equippedWeapon)
        equippedWeapon.StopFiringWeapon();
    }

    public void OnReload(InputValue value)
    {
        if (!equippedWeapon || playerController.isDead)
            return;

        if (!playerController.isReloading)
        {
            playerController.isReloading = value.isPressed;
            StartReloading();
        }
    }

    public void StartReloading()
    {
        if (playerController.isDead)
            return;

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
        if (!weaponScriptable || playerController.isDead)
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

        if (weaponPanelUI)
        {
            weaponPanelUI.weaponComponent = equippedWeapon;
        }
    }

    public void UnEquipWeapon()
    {
        if (weaponSocketLocation.transform.childCount > 0)
        {
            Destroy(weaponSocketLocation.transform.GetChild(0).gameObject);
        }
    }
}
