using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
public class UI_ItemSlot : MonoBehaviour,IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Inventory_Item itemInSlot { get; private set; }
    protected Inventory_Player playerInventory;
    protected UI ui;
    protected RectTransform rectTransform;

    [Header("UI Item Slot Settings")]
    [SerializeField] private Image itemIconImage;
    [SerializeField] private Sprite defaultImageSprite;
    [SerializeField] private TextMeshProUGUI itemStackSize;
    protected void Awake()
    {
        playerInventory = FindFirstObjectByType<Inventory_Player>();
        rectTransform = GetComponent<RectTransform>();
        ui = GetComponentInParent<UI>();
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if(itemInSlot == null) return;

        if (itemInSlot.itemData.itemType == ItemType.Consumable)
        {
            if(itemInSlot.itemEffectData.CanBeUsed() == false)
            {
                return;
            }

            playerInventory.TryUseItem(itemInSlot);
        }


        playerInventory.TryEquipItem(itemInSlot);

        if(itemInSlot == null)
            ui.itemToolTip.ShowToolTip(false, null);
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

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(itemInSlot == null) return;
        ui.itemToolTip.ShowToolTip(true, rectTransform,itemInSlot);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.itemToolTip.ShowToolTip(false, null);
    }


}
