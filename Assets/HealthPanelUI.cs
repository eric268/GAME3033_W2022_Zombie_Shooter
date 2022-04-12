using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class HealthPanelUI : MonoBehaviour
{
    public TextMeshProUGUI mPlayerHealthText;
    public PlayerHealthComponent mBettyHealthComponent;

    private void Update()
    {
        mPlayerHealthText.text = mBettyHealthComponent.CurrentHealth.ToString();
    }
}
