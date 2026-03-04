using System.Collections.Generic;
using UnityEngine;

public class Inventory_Storage : Inventory_Base
{
    public Inventory_Player playerInventory { get; private set; }
    public List<Inventory_Item> materialStash; //칸 제한 없는 재료 창고

    //순서대로 플레이어 인벤토리에서 필요한 재료 소비, 재료 창고에서 필요한 재료 소비, 저장소에서 필요한 재료 소비
    public void ConsumeMaterials(Inventory_Item itemToCraft)
    {
        foreach (var requiredItem in itemToCraft.itemData.craftRecipe)
        {
            int amountToConsume = requiredItem.itemStackSize;

            amountToConsume -= ConsumedMaterialsAmount(playerInventory.items, requiredItem);

            if (amountToConsume > 0)
            {
                amountToConsume -= ConsumedMaterialsAmount(materialStash, requiredItem);
            }
            if (amountToConsume > 0)
            {
                ConsumedMaterialsAmount(items, requiredItem);
            }
        }
    }
    //아이템 리스트에서 제작 아이템에 필요한 아이템을 소비하는 함수, 소비된 아이템 수량 반환
    private int ConsumedMaterialsAmount(List<Inventory_Item> itemList,Inventory_Item neededItem)
    {

        int amountNeeded = neededItem.itemStackSize;
        int amountConsumed = 0;

        foreach(var item in itemList)
        {
            if(item.itemData != neededItem.itemData)
            {
                continue;
            }
            int removeAmount = Mathf.Min(item.itemStackSize, amountNeeded - amountConsumed);
            item.itemStackSize = item.itemStackSize - removeAmount;
            amountConsumed += removeAmount;

            if(item.itemStackSize <= 0)
            {
                itemList.Remove(item);
            }

            if(amountConsumed >= amountNeeded)
            {
                break;
            }
        }

        return amountConsumed;
    }

    public bool HasEnoughMaterials(Inventory_Item itemToCraft)
    {
        foreach(var requiredMater in itemToCraft.itemData.craftRecipe)
        {
            if(GetAvailableAmountOf(requiredMater.itemData) < requiredMater.itemStackSize)
            {
                return false;
            }
        }
        return true;
    }
    // 모든 인벤토리, 저장소에서 이 아이템이 몇개 있는지 얻음
    public int GetAvailableAmountOf(ItemDataSO requiredItem)
    {
        int totalAmount = 0;
        foreach (var item in playerInventory.items)
        {
            if (item.itemData == requiredItem)
            {
                totalAmount += item.itemStackSize;
            }
        }
        foreach (var item in materialStash)
        {
            if (item.itemData == requiredItem)
            {
                totalAmount += item.itemStackSize;
            }
        }
        foreach(var item in items)
        {
            if (item.itemData == requiredItem)
            {
                totalAmount += item.itemStackSize;
            }
        }
        return totalAmount;
    }

    public void AddMaterialToStash(Inventory_Item itemToAdd)
    {
        var stackableItem = StackableInStash(itemToAdd);

        if(stackableItem != null)
        {
            stackableItem.AddStack();
        }
        else
        {
            materialStash.Add(itemToAdd);
        }
        TriggerUpdateUI();
    }

    public Inventory_Item StackableInStash(Inventory_Item item)
    {
        List<Inventory_Item> stackableItems = materialStash.FindAll(stashItem => stashItem.itemData == item.itemData);
        //모든 같은 종류 아이템 스택이 최대 스택 수량이 찾는지 확인
        foreach (var stackableItem in stackableItems)
        {
            if (stackableItem.CanAddStack())
            {
                return stackableItem;
            }
        }
        return null;
    }

    public void SetInventory(Inventory_Player inventoryPlayer) => this.playerInventory = inventoryPlayer;

    public void FromPlayerToStorage(Inventory_Item item,bool transferFullStack)
    {
        int transferAmount = transferFullStack? item.itemStackSize : 1;
         for (int i = 0; i < transferAmount; i++)
         {
             if (CanAddItem(item))
             {
                 var itemToAdd = new Inventory_Item(item.itemData);
                 playerInventory.RemoveOneItem(item);
                 AddItem(itemToAdd);
             }
         }
         TriggerUpdateUI();

    }
    public void FromStorageToPlayer(Inventory_Item item, bool transferFullStack)
    {
        int transferAmount = transferFullStack ? item.itemStackSize : 1;
        for (int i = 0; i < transferAmount; i++)
        {
            if (playerInventory.CanAddItem(item))
            {
                var itemToAdd = new Inventory_Item(item.itemData);

                RemoveOneItem(item);
                playerInventory.AddItem(itemToAdd);
            }
        }
        TriggerUpdateUI();
    }
}
