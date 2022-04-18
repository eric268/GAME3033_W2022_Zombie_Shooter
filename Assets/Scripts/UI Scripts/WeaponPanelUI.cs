using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeaponPanelUI : MonoBehaviour
{
    public TextMeshProUGUI weaponNameText;
    public TextMeshProUGUI currentBulletCountText;
    public TextMeshProUGUI totalAmmoText2;
    public WeaponComponent weaponComponent;

    private void OnEnable()
    {
        PlayerEvents.OnWeaponEquipped += OnWeaponEquipped;
    }
    private void OnDisable()
    {
        PlayerEvents.OnWeaponEquipped -= OnWeaponEquipped;
    }

    void OnWeaponEquipped(WeaponComponent _weaponComponent)
    {
        weaponComponent = _weaponComponent;
    }

    // Update is called once per frame
    void Update()
    {
        if (!weaponComponent) return;
        
        weaponNameText.text = weaponComponent.weaponStats.weaponName;
        currentBulletCountText.text = weaponComponent.weaponStats.bulletInClip.ToString();
        totalAmmoText2.text = weaponComponent.weaponStats.totalBullets.ToString();
    }
}
