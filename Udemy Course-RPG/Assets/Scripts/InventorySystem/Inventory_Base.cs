using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory_Base : MonoBehaviour
{
    public event Action OnInventoryUpdated;

    public int MaxInventorySize = 20;
    public List<Inventory_Item> items = new List<Inventory_Item>();


    protected virtual void Awake()
    {

    }

    public void TryUseItem(Inventory_Item itemToUse)
    {
        Inventory_Item consumable = items.Find(item => item == itemToUse);
        if(consumable != null && consumable.itemEffectData != null)
        {
            consumable.itemEffectData.ExecuteEffect();

            if(consumable.itemData.maxStackSize > 1)
            {
                consumable.RemoveStack();
                if(consumable.itemStackSize <= 0)
                {
                    RemoveOneItem(consumable);
                }
            }
            else
            {
                RemoveOneItem(consumable);
            }

            OnInventoryUpdated?.Invoke();
        }
    }

    public bool CanAddItem(Inventory_Item itemToAdd)
    {
        bool hasStackable =  FindStackable(itemToAdd) != null;

        return hasStackable || items.Count < MaxInventorySize; // 인벤이 비어있지 않고 스택을 쌓을 수 없으면 flase
    }
    public Inventory_Item FindStackable(Inventory_Item itemToAdd)
    {
        List<Inventory_Item> stackableItems = items.FindAll(item => item.itemData == itemToAdd.itemData);
        foreach (var stackableItem in stackableItems)
        {
            if (stackableItem.CanAddStack())
            {
                return stackableItem;
            }
        }
            return null;
    }
    public void AddItem(Inventory_Item itemToAdd)
    {
        Inventory_Item canAddItemInInventory = FindStackable(itemToAdd);
        if (canAddItemInInventory != null)
        {
            canAddItemInInventory.AddStack();

        }
        else
        {
            items.Add(itemToAdd); 

        }


        OnInventoryUpdated?.Invoke();
    }
    public void RemoveOneItem(Inventory_Item itemToRemve)
    {
        Inventory_Item itemInInventory = items.Find(item => item == itemToRemve);
        if(itemInInventory.itemStackSize > 1)
        {
            itemInInventory.RemoveStack();
        }
        else
        {
            items.Remove(itemInInventory);
        }

            OnInventoryUpdated?.Invoke();
    }

    public Inventory_Item FindCanAddItem(ItemDataSO itemDataSO)
    {
        return items.Find(item => item.itemData == itemDataSO );
    }
    public void TriggerUpdateUI()
    {
        OnInventoryUpdated?.Invoke();
    }
}
