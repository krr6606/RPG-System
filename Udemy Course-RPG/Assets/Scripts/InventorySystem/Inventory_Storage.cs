using System.Collections.Generic;
using UnityEngine;

public class Inventory_Storage : Inventory_Base
{
    private Inventory_Player playerInventory;
    public List<Inventory_Item> materialStash; //칸 제한 없는 재료 창고


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
