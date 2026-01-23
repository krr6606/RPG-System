using System.Collections.Generic;
using UnityEngine;

public class Inventory_Player : Inventory_Base
{
    private Entity_Stats playerStats;
    public List<Inventory_EquipmentSlot> equipmentSlots;
    protected override void Awake()
    {
        base.Awake();
        playerStats = GetComponent<Entity_Stats>();
    }

    public void TryEquipItem(Inventory_Item item)
    {
        var inventory_Item = FindCanAddItem(item.itemData);
        var matchingSlot = equipmentSlots.FindAll(slot => slot.slotType == item.itemData.itemType);
        if (inventory_Item.itemData.itemType == ItemType.Matetial)
        {
            Debug.Log("장비할 수 없는 아이템입니다.");
            return;
        }
        //일치하는 장비 슬롯이 없는 경우 체크
        foreach (var slot in matchingSlot)
        {
            if (slot.IsEmpty())
            {
                EquipItem(inventory_Item, slot);
                return;
            }
        }

        //모든 장비 슬롯이 찼을 경우 처리
        var slotToReplace = matchingSlot[0];
        var itemToReplace = slotToReplace.storedItem;
        UnequipItem(slotToReplace.storedItem);
        EquipItem(inventory_Item, slotToReplace);
    }
    private void EquipItem(Inventory_Item itemToEquip,Inventory_EquipmentSlot slot)
    {
        slot.storedItem = itemToEquip;
        itemToEquip.AddModifiers(playerStats);
        
        RemoveItem(itemToEquip);
    }
    public void UnequipItem(Inventory_Item itemToUnquip)
    {
        if (CanAddItem() == false)
        {
            Debug.Log("인벤토리에 공간이 없습니다.");
            return;
        }
        foreach (var slot in equipmentSlots)
        {
            if (slot.storedItem == itemToUnquip)
            {
                slot.storedItem = null;
                itemToUnquip.RemoveModifiers(playerStats);
                AddItem(itemToUnquip);
                return;
            }
        }
    }
}
