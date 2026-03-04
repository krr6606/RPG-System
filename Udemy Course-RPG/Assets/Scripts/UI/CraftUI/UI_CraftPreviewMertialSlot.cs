using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UI_CraftPreviewMertialSlot : MonoBehaviour
{
    [SerializeField] private Image materialIcon;
    [SerializeField] private TextMeshProUGUI materialName;

    public void SetupPreviewSlot(ItemDataSO itemData, int avaliableAmount, int requiredAmount)
    {
        materialIcon.sprite = itemData.icon;
        materialName.text = $"{itemData.itemName} - ({avaliableAmount}/{requiredAmount})";

    }
}
