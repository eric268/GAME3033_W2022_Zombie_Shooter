using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AISensingType
{
    Close_By,
    Shoot_At,
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
