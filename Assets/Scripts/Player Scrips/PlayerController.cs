using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public class PlayerController : MonoBehaviour
{
    //Movement Variables
    public bool isFiring;
    public bool isReloading;
    public bool isJumping;
    public bool isRunning;
    public bool isAiming;
    public bool isDead;
    public bool canFire;

    public bool inInventory;
    public bool isPaused;
    public InventoryComponent inventory;
    public GameUIController uiController;
    public WeaponHolder weaponHolder;
    public HealthComponent healthComponent;
    public MovementComponent mMovementComponent;
    public GameObject mPauseMenu;

    public void Awake()
    {
        inventory = GetComponent<InventoryComponent>();
        uiController = FindObjectOfType<GameUIController>();
        weaponHolder = GetComponent<WeaponHolder>();
        healthComponent = GetComponent<HealthComponent>();
        mMovementComponent = GetComponent<MovementComponent>();
        canFire = true;
    }

    private void Start()
    {
        //mPauseMenu = GameObject.FindGameObjectWithTag("PauseMenu");
        if (mPauseMenu)
            mPauseMenu.SetActive(false);
        Time.timeScale = 1.0f;
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

    public void OnPause(InputValue value)
    {
        isPaused = !isPaused;
        if (isPaused)
        {
            Time.timeScale = 0.0f;
        }
        else
        {
            Time.timeScale = 1.0f;
        }
        GameManager.instance.EnableCursor(isPaused);
        mPauseMenu.SetActive(isPaused);
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
        SoundEffects.PlaySound("BettyDead");
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
        float rotAmount = (90.0f - angle) / 400.0f;
        for (int i = 0; i < 400; i++)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x + rotAmount, transform.eulerAngles.y, transform.eulerAngles.z);
            transform.position = new Vector3(transform.position.x, transform.position.y + (1.0f * Time.fixedDeltaTime) / 4.0f, transform.position.z - (1.0f * Time.fixedDeltaTime) / 3.0f);
            yield return null;
        }

        yield return new WaitForSeconds(3.0f);

        SceneManager.LoadScene("GameLost");
    }
}
