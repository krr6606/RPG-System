using UnityEngine;
using UnityEngine.EventSystems;
public class UI_MerchantSlot : UI_ItemSlot
{
    private Inventory_Merchant merchantInventory;
    public enum MerchantSlotType
    {
        MerchantSlot,
        PlayerSlot
    }
    public MerchantSlotType slotType;
    public override void OnPointerDown(PointerEventData eventData)
    {
        if (itemInSlot == null) return;

        bool rightClick = eventData.button == PointerEventData.InputButton.Right;
        bool leftClick = eventData.button == PointerEventData.InputButton.Left;

        if (slotType == MerchantSlotType.PlayerSlot)
        {
            if (rightClick) // 우클릭 → 판매
            {
                bool sellFullStack = Input.GetKey(KeyCode.LeftControl);
                if (merchantInventory == null)
                {
                    Debug.Log("상인 인벤토리가 설정되지 않았습니다.");
                    return;
                }
                merchantInventory.TrySellItem(itemInSlot, sellFullStack);
            }
            else if (leftClick) // 좌클릭 → 장착 (장착 가능한 아이템만)
            {
                if (itemInSlot.itemData.itemType == ItemType.Weapon ||
                    itemInSlot.itemData.itemType == ItemType.Armor ||
                    itemInSlot.itemData.itemType == ItemType.Trinket)
                {
                    base.OnPointerDown(eventData);
                }
            }
        }
        else if (slotType == MerchantSlotType.MerchantSlot)
        {
            if (leftClick) return;
            else if (rightClick) // 우클릭 → 구매
            {
                bool buyFullStack = Input.GetKey(KeyCode.LeftControl);
                merchantInventory.TryBuyItem(itemInSlot, buyFullStack);
            }
        }

        ui.toolTip.ShowToolTip(false, null);
    }
    override public void OnPointerEnter(PointerEventData eventData)
    {
        if (itemInSlot == null) return;
        if (slotType == MerchantSlotType.PlayerSlot)
        {
            ui.itemToolTip.ShowToolTip(true,rectTransform, itemInSlot,false,true);
        }
        else if (slotType == MerchantSlotType.MerchantSlot)
        {
            ui.itemToolTip.ShowToolTip(true, rectTransform,itemInSlot,true,true);
        }
    }
    public void SetupMerchantUI(Inventory_Merchant merchantInventory)
    {
        this.merchantInventory = merchantInventory;
    }

}
