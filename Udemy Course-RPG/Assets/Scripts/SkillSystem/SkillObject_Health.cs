using UnityEngine;

public class SkillObject_Health : Entity_Health
{
    protected override void Die()
    {
        SkillObject_TimeEcho skillObject_TimeEcho = GetComponent<SkillObject_TimeEcho>();
        skillObject_TimeEcho.HandleDeath();
    }
}
