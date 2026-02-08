using UnityEngine;
[CreateAssetMenu(fileName = "item effect Data - refund all skills", menuName = "ScriptableObjects/Item Data/Item effect/refund all skills", order = 1)]
public class ItemEffect_RefundAllSkills : ItemEffectDataSO
{
    public override void ExecuteEffect()
    {
        UI uI = FindFirstObjectByType<UI>();
        uI.skillTreeUI.RefundAllskills();
    }
}
