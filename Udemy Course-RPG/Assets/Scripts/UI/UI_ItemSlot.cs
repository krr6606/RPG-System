using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
public class UI_ItemSlot : MonoBehaviour,IPointerDownHandler, IPointerClickHandler
{
    public Inventory_Item itemInSlot { get; private set; }
    protected Inventory_Player playerInventory;

    [Header("UI Item Slot Settings")]
    [SerializeField] private Image itemIconImage;
    [SerializeField] private Sprite defaultImageSprite;
    [SerializeField] private TextMeshProUGUI itemStackSize;
    protected void Awake()
    {
        playerInventory = FindFirstObjectByType<Inventory_Player>();
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if(itemInSlot == null) return;
        playerInventory.TryEquipItem(itemInSlot);
    }


    public void UpdateSlotUI(Inventory_Item item)
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

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Å¬¸¯µÊ");
    }
}
