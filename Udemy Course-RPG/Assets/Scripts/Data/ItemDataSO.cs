using UnityEngine;
[CreateAssetMenu(fileName = "Material item Data - ", menuName = "ScriptableObjects/Item Data/Material item", order = 1)]

public class ItemDataSO : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public ItemType itemType;
    public int maxStackSize = 1;
}
