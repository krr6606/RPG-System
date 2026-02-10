using System;
using UnityEngine;
using System.Collections.Generic;
public class UI_Inventory : MonoBehaviour
{
    private Inventory_Player playerInventory;
    private UI_EquipSlot[] UI_EquipSlots;

    [SerializeField] private UI_ItemSlotParent uiItemSlotParent;
    [SerializeField] private Transform uiEquipSlotsParent;
    private void Awake()
    {
        UI_EquipSlots = uiEquipSlotsParent.GetComponentsInChildren<UI_EquipSlot>();
        playerInventory = FindFirstObjectByType<Inventory_Player>();
    }
    private void Start()
    {
        playerInventory.OnInventoryUpdated += UpdateUI;
        UpdateUI();
    }

    private void UpdateUI()
    {
        uiItemSlotParent.UpdateSlots(playerInventory.items);
        UpdateEquipmentSlots();
    }
    private void UpdateEquipmentSlots()
    {
        List<Inventory_EquipmentSlot> PlayerEquipmentSlots = playerInventory.equipmentSlots;

        for(int i = 0; i < UI_EquipSlots.Length; i++)
        {
            var PlayerEquipSlot = PlayerEquipmentSlots[i];
            if(PlayerEquipSlot.IsEmpty())
            {
                UI_EquipSlots[i].UpdateSlotUI(null);
            }
            else
            {
                UI_EquipSlots[i].UpdateSlotUI(PlayerEquipSlot.storedItem);
            }
        }
    }

}
