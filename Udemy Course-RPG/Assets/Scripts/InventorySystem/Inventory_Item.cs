using System.Text;
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
    public string GetItemInfo()
    {
        if (itemData.itemType == ItemType.Matetial)
        {
            return "제작에 사용되는 재료.";
        }
        if (itemData.itemType == ItemType.Consumable)
        {
            return itemData.itemEffect.effectDescription;
        }

        StringBuilder stringBuilder = new StringBuilder();

        stringBuilder.AppendLine("");

        foreach (var modifier in modifiers)
        {
            string modType = GetStatNameByType(modifier.statType);
            string modValue = IsPercentageStat(modifier.statType) ? modifier.statValue + "%" : modifier.statValue.ToString();
            stringBuilder.AppendLine("+ " + modType + " " + modValue);
        }

        if (itemEffectData != null)
        {
            stringBuilder.AppendLine("");
            stringBuilder.AppendLine("특수 각인: ");
            stringBuilder.AppendLine(itemEffectData.effectDescription);
        }

        return stringBuilder.ToString();
    }
    private string GetStatNameByType(StatType statType)
    {
        switch (statType)
        {
            case StatType.MaxHealth:
                return "최대 체력";
            case StatType.healthRegen:
                return "채력 재생력";
            case StatType.Strangth:
                return "힘";
            case StatType.Agility:
                return "민첩";
            case StatType.Intelligence:
                return "지능";
            case StatType.Vitality:
                return "활력";
            case StatType.AtackSpeed:
                return "공격 속도";
            case StatType.Damage:
                return "피해량";
            case StatType.CritChance:
                return "치명타 확률";
            case StatType.CritPower:
                return "치명타 피해량";
            case StatType.ArmorReduction:
                return "방어구 관통";
            case StatType.FireDamage:
                return "화염 피해";
            case StatType.IceDamage:
                return "냉기 피해";
            case StatType.LightningDamage:
                return "번개 피해";
            case StatType.Armor:
                return "방어력";
            case StatType.Evasion:
                return "회피율";
            case StatType.IceResistance:
                return "냉기 저항력";
            case StatType.FireResistance:
                return "화염 저항력";
            case StatType.LightningResistance:
                return "번개 저항력";
            default:
                return "???";

        }
    }

    private bool IsPercentageStat(StatType statType)
    {
        switch (statType)
        {
            case StatType.CritChance:
            case StatType.ArmorReduction:
            case StatType.Evasion:
            case StatType.IceResistance:
            case StatType.FireResistance:
            case StatType.LightningResistance:
                return true;
            default:
                return false;
        }
    }
}
