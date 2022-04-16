using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAttack : MonoBehaviour
{
    private CapsuleCollider mCollider;
    // Start is called before the first frame update
    void Start()
    {
        mCollider = GetComponent<CapsuleCollider>();
        mCollider.enabled = false;
    }

    public void EnableHitCollider()
    {
        mCollider.enabled = true;
    }

    public void DisableHitCollider()
    {
        mCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Betty") || other.gameObject.CompareTag("Leon"))
        {
            other.gameObject.GetComponent<HealthComponent>().TakeDamage(GetComponentInParent<ZombieController>().mAttackDamage);
        }
    }
}
