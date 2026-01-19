using UnityEngine;

public class SkillObject_PlayerSwordSpin : SkillObject_PlayerSword
{
    private int MaxDistance;
    private float attackPerSecond;
    private float attackTimer;

    protected override void Update()
    {
        HandleAttack();
        HandleStopping();
        HandleComeback();
    }
    public override void SetupSword(Skill_SwordThrow skill_SwordThrowMgr, Vector2 dir)
    {
        base.SetupSword(skill_SwordThrowMgr, dir);
        animator.SetTrigger("spin");
        this.MaxDistance = skill_SwordThrowMgr.MaxDistance;
        this.attackPerSecond = skill_SwordThrowMgr.attackPerSecond;
        Invoke(nameof(SwordComebackOn),skill_SwordThrowMgr.MaxDistance);
    }
    private void HandleStopping()
    {
        float distanceFromPlayer = Vector2.Distance(transform.position, playerTransform.position);

        if(distanceFromPlayer > MaxDistance && rb.simulated)
        {
            rb.simulated = false;
        }
    }
    private void HandleAttack()
    {
        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0f)
        {
            DamageEnemiesInRadius(transform, 1);
            attackTimer = 1f / attackPerSecond;
        }
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        rb.simulated = false;
    }
}
