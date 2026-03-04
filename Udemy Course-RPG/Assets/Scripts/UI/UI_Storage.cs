using UnityEngine;

public class UI_Storage : MonoBehaviour
{
    //내부 실제 값
    private Inventory_Player playerInventory;
    private Inventory_Storage Storage;

    //외부UI
    [SerializeField] private UI_ItemSlotParent inventoryParent;
    [SerializeField] private UI_ItemSlotParent storageParent;
    [SerializeField] private UI_ItemSlotParent materialStashParent;

    public void SetupStorageUI( Inventory_Storage storage)
    {
        playerInventory = storage.playerInventory;
        this.Storage = storage;
        storage.OnInventoryUpdated += UpdateUI;
        UpdateUI();

        UI_StorageSlot[] storageSlots = GetComponentsInChildren<UI_StorageSlot>();
        foreach(UI_StorageSlot slot in storageSlots)
        {
            slot.SetStorage(storage);
        }
    }

    private void UpdateUI()
    {
        inventoryParent.UpdateSlots(playerInventory.items);
        storageParent.UpdateSlots(Storage.items);
        materialStashParent.UpdateSlots(Storage.materialStash);
    }

}
