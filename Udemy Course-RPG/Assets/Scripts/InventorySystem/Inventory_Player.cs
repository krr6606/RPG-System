using System.Collections.Generic;
using UnityEngine;

public class Inventory_Player : Inventory_Base
{
    private Player player;
    public int gold;
    public List<Inventory_EquipmentSlot> equipmentList;
    public Inventory_Storage storage { get; private set; }
    protected override void Awake()
    {
        base.Awake();
        player = GetComponent<Player>();
        storage = FindFirstObjectByType<Inventory_Storage>();
    }

    public void TryEquipItem(Inventory_Item item)
    {
        Inventory_Item inventory_Item = FindCanAddItem(item.itemData);

        if (inventory_Item == null)
        {
            Debug.Log("인벤토리에서 해당 아이템을 찾을 수 없습니다.");
            return;
        }

        if (inventory_Item.itemData.itemType != ItemType.Weapon &&
            inventory_Item.itemData.itemType != ItemType.Armor &&
            inventory_Item.itemData.itemType != ItemType.Trinket)
        {
            Debug.Log("장비할 수 없는 아이템입니다.");
            return;
        }

        List<Inventory_EquipmentSlot> matchingSlot = equipmentList.FindAll(slot => slot.slotType == item.itemData.itemType);

        if (matchingSlot.Count == 0)
        {
            Debug.Log("해당 아이템 타입에 맞는 장비 슬롯이 없습니다.");
            return;
        }

        // 일치하는 장비 슬롯이 없는 경우 체크
        foreach (Inventory_EquipmentSlot slot in matchingSlot)
        {
            if (slot.IsEmpty())
            {
                EquipItem(inventory_Item, slot);
                return;
            }
        }

        // 모든 장비 슬롯이 찼을 경우 처리
        Inventory_EquipmentSlot slotToReplace = matchingSlot[0];
        UnequipItem(slotToReplace.storedItem, true); //해제 후 장착
        EquipItem(inventory_Item, slotToReplace);
    }
    private void EquipItem(Inventory_Item itemToEquip, Inventory_EquipmentSlot slot)
    {
        float savedHealth = player.health.GetHealthPercentage();

        slot.storedItem = itemToEquip;
        slot.storedItem.AddModifiers(player.entityStat);
        slot.storedItem.AddItemEffect(player);

        player.health.SetHealthToPercent(savedHealth);
        RemoveOneItem(itemToEquip);
    }
    public void UnequipItem(Inventory_Item itemToUnquip,bool replacingItem = false)
    {
        if (CanAddItem(itemToUnquip) == false && replacingItem == false)
        {
            Debug.Log("인벤토리에 공간이 없습니다.");
            return;
        }

        float savedHealthPercent = player.health.GetHealthPercentage();

        var slotToUnequip = equipmentList.Find(slot => slot.storedItem == itemToUnquip);
        if(slotToUnequip != null)
        {
            slotToUnequip.storedItem = null;
        }

        itemToUnquip.RemoveModifiers(player.entityStat);
        itemToUnquip.RemoveItemEffect();
        player.health.SetHealthToPercent(savedHealthPercent);
        AddItem(itemToUnquip);
    }
}
