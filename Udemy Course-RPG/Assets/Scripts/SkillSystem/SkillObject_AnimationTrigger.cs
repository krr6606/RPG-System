using UnityEngine;

public class SkillObject_AnimationTrigger : MonoBehaviour
{
    private SkillObject_TimeEcho SkillObject_TimeEcho;

    private void Start()
    {
        SkillObject_TimeEcho = GetComponentInParent<SkillObject_TimeEcho>();
    }

    private void AttackTrigger()
    {
        SkillObject_TimeEcho.performAttack();
    }
    private void TryTerminate(int currentAttackIndex)
    {
        if(currentAttackIndex >= SkillObject_TimeEcho.maxAttack)
        {
            SkillObject_TimeEcho.HandleDeath();
        }
    }
}
