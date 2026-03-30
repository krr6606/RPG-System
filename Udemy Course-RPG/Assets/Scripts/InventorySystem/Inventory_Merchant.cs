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
        if (playerInventory.gold < itemToBuy.buyPrice * amountToBuy)
        {
            Debug.Log("골드가 부족합니다.");
            return;
        }

        int amountBought = 0;

        for (int i = 0; i < amountToBuy; ++i)
        {
            if (itemToBuy.itemData.itemType == ItemType.Matetial)
            {
                playerInventory.storage.AddMaterialToStash(itemToBuy);
                RemoveOneItem(itemToBuy);
                amountBought++;
            }
            else
            {
                if (playerInventory.CanAddItem(itemToBuy))
                {
                    playerInventory.AddItem(itemToBuy);
                    RemoveOneItem(itemToBuy);
                    amountBought++;
                }
                else
                {
                    Debug.Log("인벤토리에 공간이 없습니다.");
                    break;
                }
            }
        }

        if (amountBought > 0)
        {
            playerInventory.gold -= itemToBuy.buyPrice * amountBought;
            playerInventory.TriggerUpdateUI();
            TriggerUpdateUI();
        }
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
        List<Inventory_Item> possibleItems = new List<Inventory_Item>();

        foreach (ItemDataSO itemData in shopData.itemList)
        {
            int randomizedStackSize = Random.Range(itemData.minStackSizeAtShop, itemData.maxStackSizeAtShop + 1);
            int finalStackSize = Mathf.Clamp(randomizedStackSize, 1, itemData.maxStackSize);

            Inventory_Item itemToAdd = new Inventory_Item(itemData);
            itemToAdd.itemStackSize = finalStackSize;

            possibleItems.Add(itemToAdd);
        }

        int randomItemAmount = Random.Range(minItemsAmount, MaxInventorySize + 1);
        int finalItemAmount = Mathf.Clamp(randomItemAmount, 1, possibleItems.Count);

        for (int i = 0; i < finalItemAmount; i++)
        {
            int randomIndex = Random.Range(0, possibleItems.Count);
            Inventory_Item itemToAdd = possibleItems[randomIndex];
            if (CanAddItem(itemToAdd))
            {
                possibleItems.Remove(itemToAdd);
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
