using UnityEngine;

public enum WeaponType
{
    None, Pistol, MachineGun
}
public enum WeaponFiringPattern
{
    SemiAuto, FullAuto, ThreeShotBurst,FiveShotBurst,PumpAction
}
[System.Serializable]
public struct WeaponStats
{
    public WeaponType weaponType;
    public WeaponFiringPattern firingPattern;
    public string weaponName;
    public float damage;
    public int bulletInClip;
    public int clipSize;
    public float fireStartDelay;
    public float fireRate;
    public float fireDistance;
    public bool repeating;
    public LayerMask weaponHitLayer;
    public int totalBullets;
}


public class WeaponComponent : MonoBehaviour
{
    public bool isFiring;
    public bool isReloading;
    public Transform gripLocation;
    public WeaponStats weaponStats;
    protected WeaponHolder weaponHolder;
    protected Camera mainCamera;
    
    [SerializeField]
    protected ParticleSystem firingEffect;
    private void Awake()
    {
        mainCamera = Camera.main;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    public void Initalize(WeaponHolder _weaponHolder)
    {
        weaponHolder = _weaponHolder;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public virtual void StartFiringWeapon()
    {
        isFiring = true;

        if (weaponStats.repeating)
        {
            CancelInvoke(nameof(FireWeapon));
            InvokeRepeating(nameof(FireWeapon), weaponStats.fireStartDelay, weaponStats.fireRate);
        }
        else
        {
            FireWeapon();
        }
    }

    public virtual void StopFiringWeapon()
    {
        isFiring = false;
        CancelInvoke(nameof(FireWeapon));

        if (firingEffect && firingEffect.isPlaying)
            firingEffect.Stop();
    }   
    protected virtual void FireWeapon()
    {
        print("Firing weapon");
        weaponStats.bulletInClip--;
        print(weaponStats.bulletInClip);
    }

    public virtual void StartReloading()
    {
        isReloading = true;
        ReloadWeapon();
    }
    public virtual void StopReloading()
    {
        isReloading = false;
    }
    protected virtual void ReloadWeapon()
    {
        if (firingEffect && firingEffect.isPlaying)
            firingEffect.Stop();
        //Check to see if there is a firing effect
        int bulletsToReload = weaponStats.totalBullets - weaponStats.clipSize;

        if (bulletsToReload < 0)
        {
            weaponStats.bulletInClip = weaponStats.totalBullets;
            weaponStats.totalBullets = 0;
        }
        else
        {
            weaponStats.bulletInClip = weaponStats.clipSize;
            weaponStats.totalBullets -= weaponStats.clipSize;
        }

    }
}
