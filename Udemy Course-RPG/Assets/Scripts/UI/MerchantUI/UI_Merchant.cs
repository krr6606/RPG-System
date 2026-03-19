using UnityEngine;

public class UI_Merchant : MonoBehaviour
{
    private Inventory_Merchant merchantInventory;
    private Inventory_Player playerInventory;

    [SerializeField] private UI_ItemSlotParent merchantSlots;
    [SerializeField] private UI_ItemSlotParent playerSlots;

    public void SetupMerchantUI(Inventory_Merchant merchant, Inventory_Player player)
    {
        this.merchantInventory = merchant;
        this.playerInventory = player;
        merchant.OnInventoryUpdated += UpdateSlotUI;
        UpdateSlotUI();

        UI_MerchantSlot[] merchantSlotArray = merchantSlots.GetComponentsInChildren<UI_MerchantSlot>();
        UI_MerchantSlot[] playerSlotArray = playerSlots.GetComponentsInChildren<UI_MerchantSlot>();
        foreach (UI_MerchantSlot slot in merchantSlotArray)
        {
            slot.SetupMerchantUI(merchantInventory);


        }
        foreach (UI_MerchantSlot slot in playerSlotArray)
        {
            slot.SetupMerchantUI(merchantInventory);
        }

    }
    private void UpdateSlotUI()
    {
        playerSlots.UpdateSlots(playerInventory.itemList);
        merchantSlots.UpdateSlots(merchantInventory.itemList);
    }
}
