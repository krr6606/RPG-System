using UnityEngine;

public class Entity_AnimationTrigger : MonoBehaviour
{
    private Entity entity;
    private Entity_Combat entity_Combat;
    void Awake()
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
