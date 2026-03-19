using UnityEngine;
[CreateAssetMenu(fileName = "Material item Data - ", menuName = "ScriptableObjects/Item Data/Material item", order = 1)]

public class ItemDataSO : ScriptableObject
{
    [Header("Material details")]
    [Range(0, 10000)]
    public int itemPrice = 100;
    public int minStackSizeAtShop = 1;
    public int maxStackSizeAtShop = 1;

    [Header("Creat details")]
    public Inventory_Item[] craftRecipe;

    [Header("Item details")]
    public string itemName;
    public Sprite icon;
    public ItemType itemType;
    public int maxStackSize = 1;

    [Header("Item Effects")]
    public ItemEffectDataSO itemEffect;

}
