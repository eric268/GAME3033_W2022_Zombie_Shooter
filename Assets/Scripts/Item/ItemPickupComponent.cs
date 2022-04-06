using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickupComponent : MonoBehaviour
{

    [SerializeField]
    ItemScript pickupItem;

    [Tooltip("Manual Override for drop amount, if left at -1 it will use the amount from the scriptable object")]
    [SerializeField]
    int amount = -1;

    [SerializeField] MeshRenderer propMeshRenderer;
    [SerializeField] MeshFilter propMeshFiler;

    ItemScript itemInstance;

    // Start is called before the first frame update
    void Start()
    {
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

        Destroy(gameObject);
    }

}
