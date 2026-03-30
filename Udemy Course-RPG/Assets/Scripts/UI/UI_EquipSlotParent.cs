using System.Collections.Generic;
using UnityEngine;

public class UI_EquipSlotParent : MonoBehaviour
{
    private UI_EquipSlot[] equipSlots;

    public void UpdateEqipmentSlots(List<Inventory_EquipmentSlot> equipList)
    {
        if(equipSlots == null)
            equipSlots = GetComponentsInChildren<UI_EquipSlot>();

        int count = Mathf.Min(equipSlots.Length, equipList.Count);

        for (int i = 0; i < count; i++)
        {
            Inventory_EquipmentSlot playerEquipSlot = equipList[i];
            if (playerEquipSlot.IsEmpty())
            {
                equipSlots[i].UpdateSlotUI(null);
            }
            else
            {
                equipSlots[i].UpdateSlotUI(playerEquipSlot.storedItem);
            }
        }

    }
}
