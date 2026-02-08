using UnityEngine;
[System.Serializable]
public class Inventory_EquipmentSlot 
{
    public ItemType slotType;
    public Inventory_Item storedItem;

    public bool IsEmpty()
    {
        return storedItem == null || storedItem.itemData == null;
    }

}
