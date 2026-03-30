using System;
using UnityEngine;
using System.Collections.Generic;
public class UI_Inventory : MonoBehaviour
{
    private Inventory_Player playerInventory;


    [SerializeField] private UI_ItemSlotParent uiItemSlotParent;
    [SerializeField] private UI_EquipSlotParent uiEquipSlotsParent;
    private void Awake()
    {
        playerInventory = FindFirstObjectByType<Inventory_Player>();
    }
    private void Start()
    {
        playerInventory.OnInventoryUpdated += UpdateUI;
        UpdateUI();
    }

    private void UpdateUI()
    {
        uiItemSlotParent.UpdateSlots(playerInventory.itemList);
        uiEquipSlotsParent.UpdateEqipmentSlots(playerInventory.equipmentList);
    }


}
