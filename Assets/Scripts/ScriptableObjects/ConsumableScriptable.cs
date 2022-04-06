using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Consumable", menuName ="Item/Consumable", order = 1)]
public class ConsumableScriptable : ItemScriptable
{
    public int effect = 0;

    public override void UseItem(PlayerController playerController)
    {
        //Check to see if player is at max health

        //Heal player with potion

        SetAmount(amountValue - 1);
        if (amountValue <= 0)
        {
            DeleteItem(playerController);
        }
    }
}
