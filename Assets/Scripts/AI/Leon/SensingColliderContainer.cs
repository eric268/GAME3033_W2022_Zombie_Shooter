using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensingColliderContainer : MonoBehaviour
{
    public HashSet<Collider> zombieSensingCollider;
    public List<GameObject> tempTestColliderList;
    public SphereCollider collider;
    public LayerMask zombieLayerMask;
    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<SphereCollider>();
        zombieSensingCollider = new HashSet<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (zombieLayerMask == (zombieLayerMask | 1 << other.gameObject.layer))
        {
            zombieSensingCollider.Add(other);
            tempTestColliderList.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        zombieSensingCollider.Remove(other);
        tempTestColliderList.Remove(other.gameObject);
    }
}
