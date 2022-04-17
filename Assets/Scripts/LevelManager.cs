using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static int mActiveZombiesInLevel = 0;
    public int mCurrentLevel = 0;
    public int mZombiesToSpawnPerLevel;
    public int mMaxPossibleActiveZombie = 25;
    public float mTimeBetweenLevels = 10.0f;
    public float mSpawnRate;

    ZombieSpawner[] mZombieSpawner;

    // Start is called before the first frame update
    void Start()
    {
        mZombieSpawner = FindObjectsOfType<ZombieSpawner>();
        Invoke(nameof(NewLevelStarted), mTimeBetweenLevels);
    }

    void NewLevelStarted()
    {
        CancelInvoke();
        mCurrentLevel++;
        mZombiesToSpawnPerLevel = 5 * mCurrentLevel;
        mSpawnRate = 1.0f / mCurrentLevel;
        BeingSpawningZombies();
    }

    void BeingSpawningZombies()
    {
        InvokeRepeating(nameof(SpawnZombie), mTimeBetweenLevels, mSpawnRate);
    }

    void SpawnZombie()
    {
        mActiveZombiesInLevel++;
        mZombiesToSpawnPerLevel--;
        
        if (mZombieSpawner.Length > 0)
        { 
            int ranSpawner = Random.Range(0, mZombieSpawner.Length);
            mZombieSpawner[ranSpawner].SpawnZombie();
        }
        //Stop spawning when max reached
        if (mZombiesToSpawnPerLevel <= 0)
        {
            CancelInvoke();
        }
    }

    public void ZombieKilled()
    {
        mActiveZombiesInLevel--;
        print(mActiveZombiesInLevel);
        if (mActiveZombiesInLevel == 0 && mZombiesToSpawnPerLevel == 0)
        {
            NewLevelStarted();
        }
    }    
}
