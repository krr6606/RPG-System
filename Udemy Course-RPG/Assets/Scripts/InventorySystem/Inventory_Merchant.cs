using UnityEngine;
using System.Collections.Generic;
public class Inventory_Merchant : Inventory_Base
{
    private Inventory_Player playerInventory;

    [SerializeField] private ItemListDataSO shopData;
    [SerializeField] private int minItemsAmount = 5;

     
    protected override void Start()
    {
        base.Start();
        FillShopList();
    }

    public void TryBuyItem(Inventory_Item itemToBuy, bool buyFullStack)
    {
        int amountToBuy = buyFullStack ? itemToBuy.itemStackSize : 1;
        if(playerInventory.gold < itemToBuy.buyPrice * amountToBuy)
        {
            Debug.Log("골드가 부족합니다.");
            return;
        }

        for (int i = 0; i < amountToBuy; ++i)
        {
           if(itemToBuy.itemData.itemType == ItemType.Matetial)
            {
                playerInventory.storage.AddMaterialToStash(itemToBuy);
                RemoveOneItem(itemToBuy);
            }
            else
            {
                if(playerInventory.CanAddItem(itemToBuy))
                {
                    playerInventory.AddItem(itemToBuy);
                    RemoveOneItem(itemToBuy);
                }
                else
                {
                    Debug.Log("인벤토리에 공간이 없습니다.");
                    return; 
                }
            }
        }
        playerInventory.gold -= itemToBuy.buyPrice * amountToBuy;
        TriggerUpdateUI();
    }

    public void TrySellItem(Inventory_Item itemToSell, bool sellFullStack)
    {
        int amountToSell = sellFullStack ? itemToSell.itemStackSize : 1;
        for (int i = 0; i < amountToSell; ++i)
        {
            int sellPrice = Mathf.FloorToInt(itemToSell.sellPrice);

            playerInventory.gold += sellPrice;
            playerInventory.RemoveOneItem(itemToSell);
        }
        TriggerUpdateUI();
    }


    public void FillShopList()
    {
        itemList.Clear();
        List<Inventory_Item> possibleItems = new List<Inventory_Item>(); // ItemListDataSO의 아이템들을 Inventory_Item으로 변환하여 저장할 리스트

        foreach(var itemData in shopData.itemList)
        {
            // 아이템의 스택 사이즈를 랜덤으로 결정
            int randomizedStackSize = Random.Range(itemData.minStackSizeAtShop, itemData.maxStackSizeAtShop + 1); 
            int finalStackSize = Mathf.Clamp(randomizedStackSize,1, itemData.maxStackSize); 

            Inventory_Item itemTOAdd = new Inventory_Item(itemData);
            itemTOAdd.itemStackSize = finalStackSize;

            possibleItems.Add(itemTOAdd);
        }
        // 가능한 아이템 리스트에서 랜덤으로 아이템을 선택하여 상점에 추가
        int randomItemAmount = Random.Range(minItemsAmount, MaxInventorySize + 1);
        int finalItemAmount = Mathf.Clamp(randomItemAmount, 1, possibleItems.Count);

        for (int i = 0; i < finalItemAmount; i++)
        {
            int randomIndex = Random.Range(0, possibleItems.Count);
            Inventory_Item itemToAdd = possibleItems[randomIndex];
            if (CanAddItem(itemToAdd))
            {
                possibleItems.Remove(itemToAdd); // 중복 아이템 제거
                itemList.Add(itemToAdd);
            }

        } 
        TriggerUpdateUI();
    }

    public void SetPlayerInventory(Inventory_Player playerInventory)
    {
        this.playerInventory = playerInventory;
    }
}
