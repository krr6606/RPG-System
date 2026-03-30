using System.Text;
using TMPro;
using UnityEngine;

public class UI_ItemToolTip : UI_ToolTip
{
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemType;
    [SerializeField] private TextMeshProUGUI itemInfo;

    [SerializeField] private TextMeshProUGUI itemPrice;
    [SerializeField] private TextMeshProUGUI merchantInfo;
    // 상인 정보는 구매/판매 가격을 보여줄 때 활성화됩니다.
    public void ShowToolTip(bool show, RectTransform targetRect, Inventory_Item itemToShow, bool buyPrice = false,bool ShowMerchantInfo = false)
    {
        base.ShowToolTip(show, targetRect);

        merchantInfo.gameObject.SetActive(ShowMerchantInfo);

        int price = buyPrice ? itemToShow.buyPrice : itemToShow.sellPrice;

        int totalPrice = price * itemToShow.itemStackSize;

        string fullStackPrice = ($"가격:{price}x{itemToShow.itemStackSize} - {totalPrice}골드");
        string singlePrice = ($"가격:{price}골드");

        itemName.text = itemToShow.itemData.itemName;
        itemPrice.text = itemToShow.itemStackSize > 1 ? fullStackPrice : singlePrice;
        itemType.text = itemToShow.itemData.itemType.ToString();
        itemInfo.text = itemToShow.GetItemInfo();
    }
    
}
