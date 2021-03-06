using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickupComponent : MonoBehaviour
{

    [SerializeField]
    public ItemScript pickupItem;

    [Tooltip("Manual Override for drop amount, if left at -1 it will use the amount from the scriptable object")]
    [SerializeField]
    int amount = -1;

    [SerializeField] MeshRenderer propMeshRenderer;
    [SerializeField] MeshFilter propMeshFiler;
    WeaponPanelUI weaponPanelUI;
    ItemScript itemInstance;
    public bool mIsAvailable = true;
    public int mRespawnTimer = 10;
    public int mItemValue = 0;

    // Start is called before the first frame update
    void Start()
    {
        weaponPanelUI = FindObjectOfType<WeaponPanelUI>();
        InstatiateItem();
    }

    private void InstatiateItem()
    {
        itemInstance = Instantiate(pickupItem);

        if (amount > 0)
        {
            itemInstance.SetAmount(amount);
        }

        ApplyMesh();
    }

    void ApplyMesh()
    {
        if (propMeshFiler) 
            propMeshFiler.mesh = pickupItem.itemPrefab.GetComponentInChildren<MeshFilter>().sharedMesh;

        if (propMeshRenderer) 
            propMeshRenderer.materials = pickupItem.itemPrefab.GetComponentInChildren<MeshRenderer>().sharedMaterials;
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (!other.CompareTag("Betty") && !other.CompareTag("Leon")) return;

        SoundEffects.PlaySound("PickUp");

        if (other.CompareTag("Betty"))
        {
            InventoryComponent playerInventory = other.GetComponent<InventoryComponent>();
            WeaponHolder weaponHolder = other.gameObject.GetComponent<WeaponHolder>();
            if (weaponHolder.equippedWeapon == null || (weaponHolder.equippedWeapon != null && itemInstance.name == weaponHolder.equippedWeapon.weaponStats.weaponName))
            {
                if (playerInventory)
                {
                    playerInventory.AddItem(itemInstance, amount);
                }
                    itemInstance.UseItem(other.gameObject.GetComponent<PlayerController>());
                if (weaponPanelUI)
                {
                    weaponPanelUI.weaponComponent = weaponHolder.equippedWeapon;
                }
            }
            else if (playerInventory)
            {
                playerInventory.AddItem(itemInstance, amount);
            }
        }
        else
        {
            WeaponHolder wP = other.GetComponent<WeaponHolder>();
            WeaponScriptable wS = (WeaponScriptable)itemInstance;
            if (wS)
            {
                wP.EquipWeapon(wS);
            }
            else
            {
                itemInstance.UseItem(other.gameObject.GetComponent<PlayerController>());
            }
        }

        DeactivateComponent();
        Invoke(nameof(RespawnItem), mRespawnTimer);
    }

    private void DeactivateComponent()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        GetComponent<BoxCollider>().enabled = false;
        mIsAvailable = false;
    }

    private void RespawnItem()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        GetComponent<BoxCollider>().enabled = true;
        mIsAvailable = true;
    }

}
