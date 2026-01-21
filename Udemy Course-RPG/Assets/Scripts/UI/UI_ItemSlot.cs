using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UI_ItemSlot : MonoBehaviour
{
    public Inventory_Item itemInSlot { get; private set; }

    [Header("UI Item Slot Settings")]
    [SerializeField] private Image itemIconImage;
    [SerializeField] private Sprite defaultImageSprite;
    [SerializeField] private TextMeshProUGUI itemStackSize;

    public void UpdateSlot(Inventory_Item item)
    {
        itemInSlot = item;

        if (itemInSlot == null)
        {
            itemStackSize.text = "";
            itemIconImage.sprite = defaultImageSprite;
            return;
        }

        Color iconColor = Color.white;
        iconColor.a = 0.9f;
        itemIconImage.color = iconColor;
        itemIconImage.sprite = itemInSlot.itemData.icon;
        itemStackSize.text = item.itemStackSize < 2 ? "" : itemInSlot.itemStackSize.ToString();
    }
}
