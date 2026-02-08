using UnityEngine;

[System.Serializable]
public class Inventory_Item 
{

    private string uniqueItemID;
    public ItemDataSO itemData;
    public int itemStackSize = 1;

    public ItemModifier[] modifiers { get; private set; }
    public ItemEffectDataSO itemEffectData;

    public Inventory_Item(ItemDataSO itemData)
    {
        this.itemData = itemData;
        modifiers = EquipmentData()?.itemModifiers;
        itemEffectData = itemData.itemEffect;
        uniqueItemID = itemData.itemName + "_" + System.Guid.NewGuid().ToString();
    }
    public void AddModifiers(Entity_Stats playerStat)
    {
        if (modifiers == null) return;
        foreach (ItemModifier modifier in modifiers)
        {
            Stat ststToModify = playerStat.GetStatByType(modifier.statType);
            ststToModify.AddModifier(modifier.statValue, uniqueItemID);
        }
    }
    public void RemoveModifiers(Entity_Stats playerStat)
    {
        if (modifiers == null) return;
        foreach (ItemModifier modifier in modifiers)
        {
            Stat ststToModify = playerStat.GetStatByType(modifier.statType);
            ststToModify.RemoveModifierBySource(uniqueItemID);
        }
    }

    private EquipmentDataSO EquipmentData()
    {
        if(itemData is EquipmentDataSO equipmentData)
        {
            return equipmentData;
        }
        return null;
    }

    public void AddItemEffect(Player player) => itemEffectData?.Subscribe(player);
    public void RemoveItemEffect()=> itemEffectData?.Unsubscribe();

    public bool CanAddStack() => itemStackSize < itemData.maxStackSize;

    public void AddStack()=> itemStackSize++;
    public void RemoveStack()=> itemStackSize--; 
}
