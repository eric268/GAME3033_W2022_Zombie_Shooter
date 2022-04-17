using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public GameObject mZombiePrefab;
    public float mSpawnRate;
    public int mSpawnAmount;
    public int mSpawnCounter;

    void StartSpawningZombies()
    {
        mSpawnCounter = mSpawnAmount;
        InvokeRepeating(nameof(SpawnZombie), 0, mSpawnRate);
    }

    void SpawnZombie()
    {
        mSpawnCounter--;
        GameManager.mNumberActiveZombies++;
        GameObject newZombie = Instantiate(mZombiePrefab, this.transform);

        if (mSpawnCounter <= 0)
        {
            CancelInvoke();
        }
    }
}
