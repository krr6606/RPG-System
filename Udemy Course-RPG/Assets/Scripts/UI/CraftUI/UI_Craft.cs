using UnityEngine;

public class UI_Craft : MonoBehaviour
{
    [SerializeField] private UI_ItemSlotParent inventoryParent;
    private Inventory_Player playerInventory;
    private UI_CraftPreview craftPreview;
    private UI_CraftSlot[] craftSlots;
    private UI_CraftListButton[] craftListButtons;


    public void SetupCraftUI(Inventory_Storage storage)
    {
        playerInventory = storage.playerInventory;
        playerInventory.OnInventoryUpdated += UpdateUI;
        UpdateUI();
        craftPreview = GetComponentInChildren<UI_CraftPreview>();
        craftPreview.SetupCraftPreview(storage);
        SetupCraftListButtons();
    }
    private void SetupCraftListButtons()
    {
        craftSlots = GetComponentsInChildren<UI_CraftSlot>();
        foreach(var slot in craftSlots)
        {
            slot.gameObject.SetActive(false);
        }
        craftListButtons = GetComponentsInChildren<UI_CraftListButton>();
        foreach(var button in craftListButtons)
        {
            button.SetCraftSlot(craftSlots);
        }
    }
    private void UpdateUI() => inventoryParent.UpdateSlots(playerInventory.itemList);
}
