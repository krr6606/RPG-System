using UnityEngine;

public class UI_EquipSlot : UI_ItemSlot
{
    public ItemType slotType;

    private void OnValidate()
    {
        gameObject.name = "Equip Slot - " + slotType.ToString();
    }
    public override void OnPointerDown(UnityEngine.EventSystems.PointerEventData eventData)
    {
        if (itemInSlot == null) return;
        playerInventory.UnequipItem(itemInSlot);
    }
}
    