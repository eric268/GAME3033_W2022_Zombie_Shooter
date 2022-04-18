using System;
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
    LevelInfoUI mLevelInfoUI;
    ZombieSpawner[] mZombieSpawner;
    Action<int> FUpdateLevelUI;

    private void Awake()
    {
        mZombieSpawner = FindObjectsOfType<ZombieSpawner>();
        mLevelInfoUI = FindObjectOfType<LevelInfoUI>();
    }
    // Start is called before the first frame update
    void Start()
    {
        FUpdateLevelUI = mLevelInfoUI.NewLevelStarted;
        StartNewLevel();

    }

    void StartNewLevel()
    {
        CancelInvoke();
        mCurrentLevel++;
        mZombiesToSpawnPerLevel = 5 + 5 * mCurrentLevel;
        mSpawnRate = 1.0f / mCurrentLevel;
        FUpdateLevelUI(mCurrentLevel);
        mLevelInfoUI.UpdateZombiesReaminingText(mActiveZombiesInLevel + mZombiesToSpawnPerLevel);
        StartLevel();
    }

    void StartLevel()
    {
        Invoke(nameof(BeingSpawningZombies), mTimeBetweenLevels);
    }

    void BeingSpawningZombies()
    {
        InvokeRepeating(nameof(SpawnZombie), mTimeBetweenLevels, mSpawnRate);
    }

    void SpawnZombie()
    {
        if (mActiveZombiesInLevel > mMaxPossibleActiveZombie)
            return;

        mActiveZombiesInLevel++;
        mZombiesToSpawnPerLevel--;
        
        if (mZombieSpawner.Length > 0)
        { 
            int ranSpawner = UnityEngine.Random.Range(0, mZombieSpawner.Length);
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
        mLevelInfoUI.UpdateZombiesReaminingText(mActiveZombiesInLevel + mZombiesToSpawnPerLevel);
        print(mActiveZombiesInLevel);
        if (mActiveZombiesInLevel == 0 && mZombiesToSpawnPerLevel == 0)
        {
            StartNewLevel();
        }
    }    
}
