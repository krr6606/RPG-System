using UnityEngine;

public class SkillObject_PlayerSword : SkillObject_Base
{
    protected Skill_SwordThrow Skill_SwordThrow;
    protected Rigidbody2D rb;

    protected Transform playerTransform;
    protected bool shouldComeback;
    protected float comebackSpeed = 10f;
    protected float MaxAllowedDistance = 30f;

    private void Update()
    {
        transform.right = rb.linearVelocity;
        HandleComeback();
    }
    public virtual void SetupSword(Skill_SwordThrow skill_SwordThrowMgr, Vector2 dir)
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = dir;
        this.Skill_SwordThrow = skill_SwordThrowMgr;
        playerTransform = skill_SwordThrowMgr.player.transform;
        entityStat = Skill_SwordThrow.player.entityStat;
        damageScaleData = Skill_SwordThrow.damageScaleData;

    }
    public void SwordComebackOn() => shouldComeback = true;
    protected void HandleComeback()
    {
        float distanceFromPlayer = Vector2.Distance(transform.position, playerTransform.position);
        if(distanceFromPlayer > MaxAllowedDistance)
        {
            SwordComebackOn();
        }
        if (!shouldComeback)
            return;
        rb.simulated = false;
        if (distanceFromPlayer > 25f)
        {
            Vector3 direction = transform.position - playerTransform.position;
            transform.position = playerTransform.position + direction.normalized * 25f;
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, comebackSpeed * Time.deltaTime);
        }
        if(distanceFromPlayer < 0.5f)
        {
            Destroy(gameObject);
        }
    }
      protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        stopSword(collision);
        DamageEnemiesInRadius(transform, 1);
    }
    protected void stopSword(Collider2D collision)
    {
        rb.simulated = false;
        transform.parent = collision.transform;
    }
}
