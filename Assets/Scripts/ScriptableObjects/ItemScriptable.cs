using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemCategory
{
    None, 
    Weapon,
    Consumable,
    Equipment,
    Ammo
}

public abstract class ItemScriptable : ScriptableObject
{
    public string naem = "Item";
    public ItemCategory itemCategory = ItemCategory.None;
    public GameObject itemPrefab;
    public bool stackable;
    public int maxSize =1;

    public delegate void AmountChange();
    public event AmountChange OnAmountChange;

    public delegate void ItemDestoryed();
    public event ItemDestoryed OnItemDestroyed;

    public delegate void ItemDropped();
    public event ItemDropped OnItemDropped;

    public int amountValue = 1;
    public PlayerController controller { get; private set; }

    public virtual void Initialize(PlayerController playerController)
    {
        controller = playerController;
    }
    public abstract void UseItem(PlayerController playerController);

    public virtual void DeleteItem(PlayerController playerController)
    {
        OnItemDestroyed?.Invoke();
    }

    public virtual void DropItem(PlayerController playerController)
    {
        OnItemDropped?.Invoke();
    }

    public void ChangeItem(int amount)
    {
        amountValue += amount;
        OnAmountChange?.Invoke();
    }

    public void SetAmount(int amount)
    {
        amountValue = amount;
        OnAmountChange?.Invoke();
    }
}
