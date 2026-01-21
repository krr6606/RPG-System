using UnityEngine;

[System.Serializable]
public class Inventory_Item 
{
    public ItemDataSO itemData;
    public int itemStackSize = 1;

    public Inventory_Item(ItemDataSO itemData)
    {
        this.itemData = itemData;
    }
    public bool CanAddStack() => itemStackSize < itemData.maxStackSize;

    public void AddStack()=> itemStackSize++;
    public void RemoveStack()=> itemStackSize--; 
}
