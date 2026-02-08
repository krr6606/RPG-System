using UnityEngine;
[CreateAssetMenu(fileName = "item effect Data - ", menuName = "ScriptableObjects/Item Data/Item effect/heal", order = 1)]
public class ItemEffect_Heal : ItemEffectDataSO
{
    [SerializeField] private float healPercentage = 0.1f;

    public override void ExecuteEffect()
    {
       Player player = FindFirstObjectByType<Player>();

        float healAmount =  player.entityStat.GetMaxHP() * healPercentage;
        player.health.IncreaseHP(healAmount);
    }
}
