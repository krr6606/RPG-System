using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory_Base : MonoBehaviour
{
    public event Action OnInventoryUpdated;

    public int MaxInventorySize = 20;
    public List<Inventory_Item> items = new List<Inventory_Item>();

    public bool CanAddItem()
    {
        if(items.Count < MaxInventorySize)
        {
            return true;
        } 
        return false;
    }
    public void AddItem(Inventory_Item itemToAdd)
    {
        Inventory_Item canAddItemInInventory = FindCanAddItem(itemToAdd.itemData);
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
    public Inventory_Item FindCanAddItem(ItemDataSO itemDataSO)
    {
        return items.Find(item => item.itemData == itemDataSO && item.CanAddStack());
    }
}
