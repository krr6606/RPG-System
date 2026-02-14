using UnityEngine;
using UnityEngine.EventSystems;

public class UI_StorageSlot : UI_ItemSlot
{
    private Inventory_Storage Storage;
    public enum StorageSlotType { StorageSlot, PlayerInventorySlot }
    public StorageSlotType SlotType;
    public void SetStorage(Inventory_Storage storage) => this.Storage = storage;
    public override void OnPointerDown(PointerEventData eventData)
    {
        if(itemInSlot ==null) return;

        bool tansferFullStack = Input.GetKey(KeyCode.LeftControl);

        if (SlotType == StorageSlotType.StorageSlot)
        {
            Storage.FromStorageToPlayer(itemInSlot, tansferFullStack);
        }
        else if(SlotType == StorageSlotType.PlayerInventorySlot)
        {
            Storage.FromPlayerToStorage(itemInSlot, tansferFullStack);
        }
        ui.itemToolTip.ShowToolTip(false, null);
    }
}
