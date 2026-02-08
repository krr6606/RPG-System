using System;
using UnityEngine;
[CreateAssetMenu(fileName = "item effect Data - Buff", menuName = "ScriptableObjects/Item Data/Item effect/Buff", order = 1)]

public class ItemEffect_Buff : ItemEffectDataSO
{
    [SerializeField] private BuffEffectData[] buffsToApply;
    [SerializeField] private float buffDuration = 5f;
    [SerializeField] private string source = Guid.NewGuid().ToString();

    private Player_Stats playerStats;
    public override bool CanBeUsed()
    {
        if(playerStats == null)
            playerStats = FindAnyObjectByType<Player_Stats>();
        if (playerStats == null)
            return false;
        if (playerStats.CanApplyBuffOf(source))
        {
            return true;
        }
        else
        {
            Debug.Log("같은 버프를 중첩 불가");
            return false;
        }
    }
    public override void ExecuteEffect()
    {
        playerStats.ApplyBuff(buffsToApply, buffDuration, source);
    }
}
