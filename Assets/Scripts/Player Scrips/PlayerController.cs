using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    //Movement Variables
    public bool isFiring;
    public bool isReloading;
    public bool isJumping;
    public bool isRunning;
    public bool isAiming;
    public bool isDead;

    public bool inInventory;
    public InventoryComponent inventory;
    public GameUIController uiController;
    public WeaponHolder weaponHolder;
    public HealthComponent healthComponent;
    public MovementComponent mMovementComponent;

    public void Awake()
    {
        inventory = GetComponent<InventoryComponent>();
        uiController = FindObjectOfType<GameUIController>();
        weaponHolder = GetComponent<WeaponHolder>();
        healthComponent = GetComponent<HealthComponent>();
        mMovementComponent = GetComponent<MovementComponent>();
    }
    public void OnInventory(InputValue value)
    {
        if (isDead)
            return;
        if (inInventory)
        {
            inInventory = false;
        }
        else
        {
            inInventory = true;
        }

        OpenInventory(inInventory);
    }

    private void OpenInventory(bool open)
    {

        if (open)
        {
            uiController.EnableInventoryMenu();
            GameManager.instance.EnableCursor(true);
        }
        else
        {
            uiController.EnableGameMenu();
            GameManager.instance.EnableCursor(false);
        }
    }

    public void SetIsDead()
    {
        isDead = true;
        OpenInventory(false);
        weaponHolder.StopFiring();
    }

    public void StartCameraZoomOut()
    {
        if (mMovementComponent.followTransform)
        {
            StartCoroutine("CameraZoomOut");
        }
    }

    IEnumerator CameraZoomOut()
    {
        Transform transform = mMovementComponent.followTransform.transform;
        float aimAngle = transform.eulerAngles.x;
        float angle = aimAngle = (aimAngle > 180) ? aimAngle - 360 : aimAngle;
        float rotAmount = (90.0f - angle) / 300.0f;
        for (int i = 0; i < 300; i++)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x + rotAmount, transform.eulerAngles.y, transform.eulerAngles.z);
            transform.position = new Vector3(transform.position.x, transform.position.y + (1.0f * Time.fixedDeltaTime) / 4.0f, transform.position.z - (1.0f * Time.fixedDeltaTime) / 3.0f);
            yield return null;
        }
    }
}
