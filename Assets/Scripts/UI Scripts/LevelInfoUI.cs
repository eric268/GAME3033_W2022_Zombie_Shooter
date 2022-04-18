using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelInfoUI : MonoBehaviour
{
    public TextMeshProUGUI mCurrentLevelNumberText;
    public TextMeshProUGUI mZombiesRemainingText;

    private void Start()
    {
    }

    public void NewLevelStarted(int level)
    {
        mCurrentLevelNumberText.text = "Round " + level;
    }

    public void UpdateZombiesReaminingText(int zombiesRemaining)
    {
        mZombiesRemainingText.text = "Zombies Remaining  " + zombiesRemaining;
    }
}
