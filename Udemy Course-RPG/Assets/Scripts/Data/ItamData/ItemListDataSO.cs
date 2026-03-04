using UnityEngine;
[CreateAssetMenu(fileName = "List of items - ", menuName = "ScriptableObjects/Item Data/Item List", order = 1)]
public class ItemListDataSO : ScriptableObject
{
    public ItemDataSO[] itemList;

}
