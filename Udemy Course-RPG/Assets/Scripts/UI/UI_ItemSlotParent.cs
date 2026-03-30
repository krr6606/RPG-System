using System.Collections.Generic;
using UnityEngine;
public class UI_ItemSlotParent : MonoBehaviour
{
    private UI_ItemSlot[] slots;

    //하이라이키창에서 자식노드들의 슬롯들의 업데이트 함수를 실행
    public void UpdateSlots(List<Inventory_Item> itemList)
    {
        if(slots == null)
        {
            slots = GetComponentsInChildren<UI_ItemSlot>();
        }

        for (int i = 0; i < slots.Length; i++)
        {
            if (i < itemList.Count)
            {
                slots[i].UpdateSlotUI(itemList[i]);
            }
            else
            {
                slots[i].UpdateSlotUI(null);
            }
        }
    }
}
