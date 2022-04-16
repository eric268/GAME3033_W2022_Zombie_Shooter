using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AKComponent : WeaponComponent
{
    protected override void FireWeapon()
    { 
        Vector3 hitLocation;
        if (firingEffect)
            firingEffect.Play();

        if(weaponStats.bulletInClip > 0 && !isReloading)
        {
            base.FireWeapon();

            Ray screenRay = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2.0f, Screen.height / 2.0f, 0.0f));

            if (Physics.Raycast(screenRay, out RaycastHit hit, weaponStats.fireDistance, weaponStats.weaponHitLayer))
            {
                hitLocation = hit.point;
                Vector3 hitDirection = hit.point - mainCamera.transform.position;
                Debug.DrawRay(mainCamera.transform.position, hitDirection.normalized * weaponStats.fireDistance, Color.red, 10);

                if (hit.collider.gameObject.CompareTag("Leon"))
                {
                    print("Leon hit");
                    hit.collider.gameObject.GetComponent<NavMeshAgent>().speed = 1.0f;
                }
                else if(hit.collider.gameObject.CompareTag("Zombie"))
                {
                    hit.collider.gameObject.GetComponent<HealthComponent>().TakeDamage((int)weaponStats.damage);
                }
            }
        }
        else if (weaponStats.bulletInClip <= 0)
        {
            weaponHolder.StartReloading();
        }
    }
}
