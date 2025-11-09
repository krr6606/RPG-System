using UnityEngine;

public class Entity_AnimationTriggers : MonoBehaviour
{
    private Entity entity;
    private Entity_Combat entity_Combat;
    protected virtual void Awake()
    {
        entity = GetComponentInParent<Entity>();
        entity_Combat = GetComponentInParent<Entity_Combat>();
    }
    private void CurrentStateTrigger()
    {
        entity.CurrentAinmTriggerCall();
    }
    private void AttackTrigger()
    {
        entity_Combat.performAttack();
    }
}
