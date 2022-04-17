using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HealthConsumable", menuName = "Item/HealthConsumable", order = 1)]
public class HealthConsumableScritpable : ConsumableScriptable
{
    public override void UseItem(PlayerController playerController)
    {
        if (playerController.healthComponent.CurrentHealth >= playerController.healthComponent.StartingHealth) return;

        playerController.healthComponent.HealPlayer(effect);

        base.UseItem(playerController);
    }
}
