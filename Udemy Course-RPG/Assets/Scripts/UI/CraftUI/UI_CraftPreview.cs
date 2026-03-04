using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_CraftPreview : MonoBehaviour
{
    private Inventory_Item itemToCraft;
    private Inventory_Storage storage;
    private UI_CraftPreviewMertialSlot[] craftPreviewSlots;

    [Header("Item Preview Settings")]
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemInfo;
    [SerializeField] private TextMeshProUGUI buttonText;

    public void SetupCraftPreview(Inventory_Storage storage)
    {
        this.storage = storage;
        craftPreviewSlots = GetComponentsInChildren<UI_CraftPreviewMertialSlot>();
        foreach(var slot in craftPreviewSlots)
        {
            slot.gameObject.SetActive(false);
        }
    }

    public void ConfirmCraft()
    {
        if(itemToCraft == null)
        {
            buttonText.text = "선택된 아이템이 없습니다.";
            return;
        }
        if(storage.HasEnoughMaterials(itemToCraft) && storage.playerInventory.CanAddItem(itemToCraft))
        {
            storage.ConsumeMaterials(itemToCraft);
            storage.playerInventory.AddItem(itemToCraft);

        }

        UpdateCraftPreviewSlots();
    }


    public void UpdateCraftPreview(ItemDataSO itemData)
    {
        itemToCraft = new Inventory_Item(itemData);

        itemIcon.sprite = itemToCraft.itemData.icon;
        itemName.text = itemToCraft.itemData.itemName;
        itemInfo.text = itemToCraft.GetItemInfo();

        UpdateCraftPreviewSlots();
    }

    private void UpdateCraftPreviewSlots()
    {
        foreach (var slot in craftPreviewSlots)
        {
            slot.gameObject.SetActive(false);
        }
        //제작에 쓰여 인벤에서 제거되는 아이템 보여주기
        for (int i = 0; i < itemToCraft.itemData.craftRecipe.Length; i++)
        {
            Inventory_Item requiredItem = itemToCraft.itemData.craftRecipe[i];
            int avaliableAmount = storage.GetAvailableAmountOf(requiredItem.itemData);
            int requiredAmount = requiredItem.itemStackSize;

            craftPreviewSlots[i].gameObject.SetActive(true);
            craftPreviewSlots[i].SetupPreviewSlot(requiredItem.itemData, avaliableAmount, requiredAmount);
        }
    }
}
