using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
    [Header("WeaponToSpawn"), SerializeField]
    public GameObject weaponToSpawn;

    public Sprite corssHairImage;

    [SerializeField]
    GameObject weaponSocketLocation;
    // Start is called before the first frame update
    void Start()
    {
        GameObject spawnedWeapon = Instantiate(weaponToSpawn, weaponSocketLocation.transform.position, weaponSocketLocation.transform.rotation, weaponSocketLocation.transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}