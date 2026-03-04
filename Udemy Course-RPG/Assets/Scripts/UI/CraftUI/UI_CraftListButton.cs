using UnityEngine;

public class UI_CraftListButton : MonoBehaviour
{
    [SerializeField] private ItemListDataSO itemListData;
    private UI_CraftSlot[] craftSlots;

    public void SetCraftSlot(UI_CraftSlot[] craftSlots) => this.craftSlots = craftSlots;

    public void UpdateCraftSlots()
    {
        if (craftSlots == null)
        {
            Debug.Log("∏ÆΩ∫∆Æ ΩΩ∑‘ «“¥Á¿Ã æ»µ "); return;
        }
        foreach (var craftSlot in craftSlots)
        {
            craftSlot.gameObject.SetActive(false);
        }
        for (int i = 0; i < itemListData.itemList.Length; i++)
        {
            craftSlots[i].gameObject.SetActive(true);
            craftSlots[i].SetupButton(itemListData.itemList[i]);
        }
    }
}
