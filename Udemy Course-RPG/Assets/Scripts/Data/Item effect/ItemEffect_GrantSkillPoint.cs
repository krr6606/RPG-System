using UnityEngine;
[CreateAssetMenu(fileName = "item effect Data - Grant Skill Point", menuName = "ScriptableObjects/Item Data/Item effect/Grant Skill Point", order = 1)]

public class ItemEffect_GrantSkillPoint : ItemEffectDataSO
{
    [SerializeField] private int pointsToAdd;

    public override void ExecuteEffect()
    {
        UI ui = FindAnyObjectByType<UI>();
        ui.skillTreeUI.AddSkillPoint(pointsToAdd);
    }

}
