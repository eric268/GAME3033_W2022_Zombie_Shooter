using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AKComponent : WeaponComponent
{
    public LayerMask mZombieLayerMask;
    public LayerMask mLeonLayerMask;
    public LayerMask mBettyLayerMask;
    protected override void FireWeapon()
    { 
        Vector3 hitLocation;
        if (firingEffect)
            firingEffect.Play();

        if(weaponStats.bulletInClip > 0 && !isReloading)
        {
            base.FireWeapon();
            bool leonHit = false;
            bool bettyHit = false;
            RaycastHit[] hitArray;

            //This allows for bullet penetration and not damaging same zombie twice
            HashSet<Collider> mZombiesHit = new HashSet<Collider>();


            if (gameObject.transform.parent.CompareTag("Betty"))
            {
                Ray screenRay = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2.0f, Screen.height / 2.0f, 0.0f));
                hitArray = Physics.RaycastAll(screenRay, weaponStats.fireDistance, weaponStats.weaponHitLayer);
            }
            else
            {
                LeonController leonController = GetComponentInParent<LeonController>();
                Ray screenRay = new Ray(leonController.mFiringLocation.position, leonController.transform.forward);
                hitArray = Physics.RaycastAll(screenRay, weaponStats.fireDistance, weaponStats.weaponHitLayer);
                Debug.DrawLine(leonController.mFiringLocation.position, leonController.mFiringLocation.position + leonController.transform.forward * weaponStats.fireDistance, Color.red, Time.deltaTime);
            }

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
                        }
                        else if (mLeonLayerMask == (mLeonLayerMask | 1 << hit.collider.gameObject.layer) && !leonHit && !transform.parent.CompareTag("Leon"))
                        {
                            leonHit = true;
                            hit.collider.gameObject.GetComponent<LeonController>().OnStateChange(LeonState.Slowed);
                        }
                        else if (mBettyLayerMask == (mBettyLayerMask | 1 << hit.collider.gameObject.layer) && !bettyHit && !transform.parent.CompareTag("Betty"))
                        {
                            print("Betty slowed");
                            hit.collider.GetComponent<MovementComponent>().BeingSlowedEffect();
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
