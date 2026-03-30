using UnityEngine;

public class UI_Merchant : MonoBehaviour
{
    private Inventory_Merchant merchantInventory;
    private Inventory_Player playerInventory;

    [SerializeField] private UI_ItemSlotParent merchantSlots;
    [SerializeField] private UI_ItemSlotParent playerSlots;
    [SerializeField] private UI_EquipSlotParent playerEquipSlots;

    public void SetupMerchantUI(Inventory_Merchant merchant, Inventory_Player player)
    {
        this.merchantInventory = merchant;
        this.playerInventory = player;

        this.playerInventory.OnInventoryUpdated += UpdateSlotUI;
        this.merchantInventory.OnInventoryUpdated += UpdateSlotUI;

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

    public void CleanupMerchantUI()
    {
        if (playerInventory != null)
            playerInventory.OnInventoryUpdated -= UpdateSlotUI;
        if (merchantInventory != null)
            merchantInventory.OnInventoryUpdated -= UpdateSlotUI;
    }

    private void UpdateSlotUI()
    {
        if (playerInventory == null || merchantInventory == null)
        {
            Debug.LogWarning("플레이어 인벤터리 혹은 상인 인벤토리가 할당되지 않았습니다.");
            return;
        }
        playerSlots.UpdateSlots(playerInventory.itemList);
        merchantSlots.UpdateSlots(merchantInventory.itemList);
        playerEquipSlots.UpdateEqipmentSlots(playerInventory.equipmentList);
    }
}
