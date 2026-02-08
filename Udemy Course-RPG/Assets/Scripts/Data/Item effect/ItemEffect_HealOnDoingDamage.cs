using UnityEngine;
[CreateAssetMenu(fileName = "item effect Data - 피해를 줄 시 체력 회복", menuName = "ScriptableObjects/Item Data/Item effect/HealOnDoingDamage", order = 1)]

public class ItemEffect_HealOnDoingDamage : ItemEffectDataSO
{
    [SerializeField] private float percentHealedOnAttack = .2f;

    public override void Subscribe(Player player)
    {
        base.Subscribe(player);
        player.playerCombat.OnDoingPhysicalDamage += HealOnDoingDamage;
    }

    private void HealOnDoingDamage(float damage)
    {
        player.health.IncreaseHP(damage * percentHealedOnAttack);
    }
    public override void Unsubscribe()
    {
        base.Unsubscribe();
        player.playerCombat.OnDoingPhysicalDamage -= HealOnDoingDamage;
        player = null;
    }
}
