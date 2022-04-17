using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public GameObject mZombiePrefab;
    public LevelManager mLevelManager;
    public void Start()
    {
        mLevelManager = FindObjectOfType<LevelManager>();
    }

    public void SpawnZombie()
    {
        GameObject newZombie = Instantiate(mZombiePrefab, this.transform);

        //Zombie health increases with level
        newZombie.GetComponent<HealthComponent>().mStartingHealth += 10 * mLevelManager.mCurrentLevel;
        newZombie.GetComponent<HealthComponent>().mStartingHealth = newZombie.GetComponent<HealthComponent>().mCurrentHealth;
        int childCount = newZombie.transform.childCount;

        if (childCount > 1)
        {
            int ranZombie = Random.Range(1, childCount);
            newZombie.transform.GetChild(ranZombie).gameObject.SetActive(true);
        }
    }
}
