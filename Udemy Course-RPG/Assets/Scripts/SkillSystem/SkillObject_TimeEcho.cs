using UnityEngine;

public class SkillObject_TimeEcho : SkillObject_Base
{
    [SerializeField] private float wiapMoveSpeed = 15;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private GameObject onDeathVFX;

    private bool shouldMoveToPlayer = false;
    private Transform playerTransform;
    private Skill_TimeEcho skillTimeEcho;
    private TrailRenderer wispTrail;
    private Entity_Health entity_Health;
    private SkillObject_Health echoHealth;
    private Player_SkillManager playerSkillManager;
    private Entity_StatusHendler Entity_StatusHendler;
    public int maxAttack {  get; private set; }
    public void SetupEcho(Skill_TimeEcho skill)
    {
        skillTimeEcho = skill;
        this.maxAttack = skill.GetMaxAttack();
        entityStat = skill.player.entityStat;
        damageScaleData = skill.damageScaleData;
        playerTransform = skill.player.transform;
        entity_Health = skill.player.health;
        playerSkillManager = skill.player.skillManager;
        Entity_StatusHendler = skill.player.statusHendler;
        Invoke(nameof(HandleDeath),skillTimeEcho.GetEchoDuration());
        FlipToTarget();

        echoHealth = GetComponent<SkillObject_Health>();
        wispTrail = GetComponentInChildren<TrailRenderer>();
        wispTrail.gameObject.SetActive(false);

        animator.SetBool("canAttack", maxAttack > 0);
    }
    private void Update()
    {
        if(shouldMoveToPlayer)
        {
            HandleWispMovement();
            return;
        }
        animator.SetFloat("yVelocity", rb.linearVelocity.y);
        StopHorizontalMovement();
    }
    private void HandleWispMovement()
    {
        transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, wiapMoveSpeed * Time.deltaTime);
        if(Vector3.Distance(transform.position, playerTransform.position) < 0.5f)
        {
            HandleWispTouchdown();
            Destroy(gameObject);
        }
    }
    private void HandleWispTouchdown()
    {
        float healAmount = echoHealth.lastDamageTaken * skillTimeEcho.GetDamageHealPercentage();
        entity_Health.IncreaseHP(healAmount);

        float cooldownReduction = skillTimeEcho.GetCooldownReducedInSeconds();
        playerSkillManager.ReduceAllSkillCooldowns(cooldownReduction);

        if(skillTimeEcho.CanRemoveNegativeEffects())
        {
            Entity_StatusHendler.RemoveAllNegativeStatusEffects();
        }
    }

    public void HandleDeath()
    {
        Instantiate(onDeathVFX, transform.position,Quaternion.identity);
        if(skillTimeEcho.ShouldBeWisp())
        {
            ConvertToWisp();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void ConvertToWisp()
    {
        animator.gameObject.SetActive(false);
        wispTrail.gameObject.SetActive(true);
        rb.simulated = false;
        shouldMoveToPlayer = true;
    }

    private void FlipToTarget()
    {
        Transform target = TargetTracking();

        if(target == null)
        {
            return;
        }
        if(target.position.x  < transform.position.x)
        {
            transform.Rotate(0,180,0);
        }

    }
    private void StopHorizontalMovement()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position,Vector2.down,1.5f,groundLayer);
        if(hit.collider != null)
        {
            rb.linearVelocity = new Vector2(0,rb.linearVelocity.y);
        }
    }
    public void performAttack()
    {
        DamageEnemiesInRadius(targetCheck, 1);

        if(targetGotHit == false)
        {
            return;
        }
        bool shouldDuplicate = Random.value < skillTimeEcho.GetDuplicateChance();
        float echoOffsetX = transform.position.x < lastTarget.position.x ? 1.0f : -1.0f;

        if(shouldDuplicate)
        {
            Vector3 echoPos = new Vector3(lastTarget.position.x + echoOffsetX, lastTarget.position.y, lastTarget.position.z);
            skillTimeEcho.CreateTimeEcho(echoPos);
        }
    }
}
