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
        if (slotType == MerchantSlotType.PlayerSlot) // 플레이어 슬롯에서 오른쪽 클릭은 판매, 왼쪽 클릭은 장착
        {
            if (rightClick)
            {
                bool sellfullStack =  Input.GetKey(KeyCode.LeftControl);
                if(merchantInventory == null)
                {
                    Debug.Log("상인 인벤토리가 설정되지 않았습니다.");
                    return;
                }
                merchantInventory.TrySellItem(itemInSlot, sellfullStack);

            }
            else if (leftClick)
            {
                base.OnPointerDown(eventData);
            }
        }
        else if (slotType == MerchantSlotType.MerchantSlot) // 상인 슬롯에서 오른쪽 클릭은 구매
        {
            if (leftClick) return;
            else if (rightClick)
            {
                bool buyFullStack = Input.GetKey(KeyCode.LeftControl);
                merchantInventory.TryBuyItem(itemInSlot, buyFullStack);
            }
        }
        ui.toolTip.ShowToolTip(false, null);
    }
    public void SetupMerchantUI(Inventory_Merchant merchantInventory)
    {
        this.merchantInventory = merchantInventory;
    }

}
