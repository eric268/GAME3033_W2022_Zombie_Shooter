using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AKComponent : WeaponComponent
{
    protected override void FireWeapon()
    { 
        Vector3 hitLocation;
        
        if(weaponStats.bulletInClip > 0 && !isReloading && !weaponHolder.playerController.isRunning)
        {
            base.FireWeapon();

            Ray screenRay = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2.0f, Screen.height / 2.0f, 0.0f));

            if (Physics.Raycast(screenRay, out RaycastHit hit, weaponStats.fireDistance, weaponStats.weaponHitLayer))
            {
                hitLocation = hit.point;
                Vector3 hitDirection = hit.point - mainCamera.transform.position;
                Debug.DrawRay(mainCamera.transform.position, hitDirection.normalized * weaponStats.fireDistance, Color.red, 1);
            }
        }
        else if (weaponStats.bulletInClip <= 0)
        {
            weaponHolder.StartReloading();
        }
    }
}
