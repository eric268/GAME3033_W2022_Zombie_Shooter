using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensingColliderContainer : MonoBehaviour
{
    public HashSet<Collider> zombieSensingCollider;
    public List<GameObject> tempTestColliderList;
    public SphereCollider collider;
    public LayerMask targetLayerMask;
    // Start is called before the first frame update

    private void Awake()
    {
        collider = GetComponent<SphereCollider>();
        zombieSensingCollider = new HashSet<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (targetLayerMask == (targetLayerMask | 1 << other.gameObject.layer))
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

    public void RemoveDeadZombieCollider(Collider other)
    {
        if (zombieSensingCollider.Contains(other))
            print("Collider found");
        else
        {
            print("Collider not found");
        }
        zombieSensingCollider.Remove(other);
        tempTestColliderList.Remove(other.gameObject);
        zombieSensingCollider.TrimExcess();

        foreach(Collider coll in zombieSensingCollider)
        {
            print(coll.gameObject.name);
        }
    }
}
