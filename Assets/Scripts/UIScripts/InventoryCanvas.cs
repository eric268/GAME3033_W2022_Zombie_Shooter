using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class InventoryCanvas : GameHUDWidget
{
    private ItemDisplayPanel ItemDisplayPanel;
    private List<CategorySelectButton> CategoryButtons;
    private PlayerController[] PC;
    private PlayerController PlayerController;

    private void Awake()
    {
        PC = FindObjectsOfType<PlayerController>();
        for (int i = 0; i< PC.Length; i++)
        {
            if (PC[i].CompareTag("Betty"))
            {
                PlayerController = PC[i];
                break;
            }
        }
        //PlayerController = FindObjectOfType<PlayerController>();
        CategoryButtons = GetComponentsInChildren<CategorySelectButton>().ToList();
        ItemDisplayPanel = GetComponentInChildren<ItemDisplayPanel>();
        foreach (CategorySelectButton button in CategoryButtons)
        {
            button.Initialize(this);
        }
    }

    private void OnEnable()
    {
        if (!PlayerController || !PlayerController.inventory) return;
        if (PlayerController.inventory.GetItemCount() <= 0) return;

        ItemDisplayPanel.PopulatePanel(PlayerController.inventory.GetItemsOfCategory(ItemCategory.None));
    }

    public void SelectCategory(ItemCategory category)
    {
        if (!PlayerController || !PlayerController.inventory) return;
        if (PlayerController.inventory.GetItemCount() <= 0) return;

        ItemDisplayPanel.PopulatePanel(PlayerController.inventory.GetItemsOfCategory(category));
    }
}
