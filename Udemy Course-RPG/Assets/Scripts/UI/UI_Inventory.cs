using System;
using UnityEngine;
using System.Collections.Generic;
public class UI_Inventory : MonoBehaviour
{
    private UI_ItemSlot[] UI_ItemSlots;
    private Inventory_Base playerInventory;
    private void Awake()
    {
        UI_ItemSlots = GetComponentsInChildren<UI_ItemSlot>();
        playerInventory = FindFirstObjectByType<Inventory_Base>();
    }
    private void Start()
    {
        playerInventory.OnInventoryUpdated += UpdateInventorySlots;
        UpdateInventorySlots();
    }
    private void UpdateInventorySlots()
    {
        List<Inventory_Item> itemList = playerInventory.items;

        for (int i = 0; i < UI_ItemSlots.Length; i++)
        {
            if (i < itemList.Count)
            {
                UI_ItemSlots[i].UpdateSlot(itemList[i]);
            }
            else
            {
                UI_ItemSlots[i].UpdateSlot(null);
            }
        }
    }
}
