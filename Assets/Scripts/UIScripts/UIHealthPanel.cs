using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UIHealthPanel : MonoBehaviour
{
    public TextMeshProUGUI mPlayerHealthText;
    public PlayerHealthComponent mBettyHealthComponent;

    private void Update()
    {
        if (mBettyHealthComponent.CurrentHealth <= 0.0f)
        {
            mPlayerHealthText.text = 0.ToString();
        }
        else
        {
            mPlayerHealthText.text = mBettyHealthComponent.CurrentHealth.ToString();
        }
    }
}
