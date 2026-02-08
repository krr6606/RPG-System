using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Player_Stats : Entity_Stats
{
    private List<string> activeBuff = new List<string>();
    private Inventory_Player Inventory;

    protected override void Awake()
    {
        base.Awake();
        Inventory = GetComponent<Inventory_Player>();
    }
    public bool CanApplyBuffOf(string source)
    {
        return !activeBuff.Contains(source);
    }
    public void ApplyBuff(BuffEffectData[] buffs,float duration,string source)
    {
        StartCoroutine(BuffCoroutine(buffs, duration, source));
    }
    private IEnumerator BuffCoroutine(BuffEffectData[] buffs, float duration, string source)
    {
        activeBuff.Add(source);
        foreach (var buff in buffs)
            GetStatByType(buff.Type).AddModifier(buff.Value, source);
        yield return new WaitForSeconds(duration);
        foreach (var buff in buffs)
            GetStatByType(buff.Type).RemoveModifierBySource(source);

        Inventory.TriggerUpdateUI();
        activeBuff.Remove(source);
    }
}
