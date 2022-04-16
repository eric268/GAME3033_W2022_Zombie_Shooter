using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AKComponent : WeaponComponent
{
    public LayerMask mZombieLayerMask;
    public LayerMask mLeonLayerMask;
    protected override void FireWeapon()
    { 
        Vector3 hitLocation;
        if (firingEffect)
            firingEffect.Play();

        if(weaponStats.bulletInClip > 0 && !isReloading)
        {
            base.FireWeapon();
            bool leonHit = false;

            //This allows for bullet penetration and not damaging same zombie twice
            HashSet<Collider> mZombiesHit = new HashSet<Collider>();
            Ray screenRay = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2.0f, Screen.height / 2.0f, 0.0f));

            RaycastHit[] hitArray = Physics.RaycastAll(screenRay, weaponStats.fireDistance, weaponStats.weaponHitLayer);

            if (hitArray.Length > 0)
            {
                foreach (RaycastHit hit in hitArray)
                {
                    if (!hit.collider.isTrigger)
                    {
                        if (mZombieLayerMask == (mZombieLayerMask | 1 << hit.collider.gameObject.layer) && !mZombiesHit.Contains(hit.collider))
                        {
                            mZombiesHit.Add(hit.collider);
                            hit.collider.gameObject.GetComponent<HealthComponent>().TakeDamage((int)weaponStats.damage / mZombiesHit.Count); //Can add scalar for different weapons to have more or less bullet penetration
                            print("Zombie hit");
                        }
                        else if (mLeonLayerMask == (mLeonLayerMask | 1 << hit.collider.gameObject.layer) && !leonHit)
                        {
                            leonHit = true;
                            hit.collider.gameObject.GetComponent<LeonController>().OnStateChange(LeonState.Slowed);
                            print("Leon hit");
                        }
                    }
                }
            }
        }
        else if (weaponStats.bulletInClip <= 0)
        {
            weaponHolder.StartReloading();
        }
    }
}
