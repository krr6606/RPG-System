using System;
using UnityEngine;
[CreateAssetMenu(fileName = "Equipment item Data - ", menuName = "ScriptableObjects/Item Data/Equipment item", order = 1)]

public class EquipmentDataSO : ItemDataSO
{
    [Header("Item Modifiers")]
    public ItemModifier[] itemModifiers;
}

[Serializable]
public class ItemModifier
{
    public StatType statType;
    public float statValue;
}