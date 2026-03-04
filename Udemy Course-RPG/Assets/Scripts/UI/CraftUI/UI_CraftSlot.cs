using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_CraftSlot : MonoBehaviour
{
    private ItemDataSO itemToCraft;
    [SerializeField] private UI_CraftPreview craftPreview;

    [SerializeField] private Image craftItemIcon;
    [SerializeField] private TextMeshProUGUI itemNameText;

    public void SetupButton(ItemDataSO craftDataSO)
    {
        this.itemToCraft = craftDataSO;
        craftItemIcon.sprite = itemToCraft.icon;
        itemNameText.text = itemToCraft.itemName;
    }

    public void UpdateCraftPreview()
    {
        craftPreview.UpdateCraftPreview(itemToCraft);
    }
}
