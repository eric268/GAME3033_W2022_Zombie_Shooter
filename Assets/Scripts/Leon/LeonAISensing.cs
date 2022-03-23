using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AISensingType
{
    Shoot_At,
    Close_By,
    Num_Sensing_Types,
}

public class LeonAISensing : MonoBehaviour
{
    public SensingColliderContainer[] colliderArray;

    private void Start()
    {
        colliderArray = GetComponentsInChildren<SensingColliderContainer>();
    }

    private void Update()
    {
        
    }
}
