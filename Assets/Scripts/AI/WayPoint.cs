using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint : MonoBehaviour
{
    public Vector3 position;
    public int mNumZombiesCloseBy;
    public LayerMask mZombieLayerMask;

    private void Start()
    {
        position = transform.position;
        mNumZombiesCloseBy = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (mZombieLayerMask == (mZombieLayerMask | 1 << other.gameObject.layer))
        {
            mNumZombiesCloseBy++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (mZombieLayerMask == (mZombieLayerMask | 1 << other.gameObject.layer))
        {
            mNumZombiesCloseBy--;
        }
    }        

}
