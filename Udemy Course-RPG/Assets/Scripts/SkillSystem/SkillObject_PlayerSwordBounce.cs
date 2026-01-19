using System.Collections.Generic;
using UnityEngine;

public class SkillObject_PlayerSwordBounce : SkillObject_PlayerSword
{
    private float bounceSpeed = 15;
    private int bounceCount;

    private Collider2D[] enemyTargets;
    private Transform nextTarget;
    private List<Transform> selectedBefore = new List<Transform>();


    public override void SetupSword(Skill_SwordThrow skill_SwordThrowMgr, Vector2 dir)
    {
        animator.SetTrigger("spin");
        base.SetupSword(skill_SwordThrowMgr, dir);
        this.bounceCount = skill_SwordThrowMgr.bounceCount;
        this.bounceSpeed = skill_SwordThrowMgr.bounceSpeed;
    }
    protected override void Update()
    {
        HandleBounce();
        HandleComeback();
    }
    private void HandleBounce()
    {
        if (nextTarget == null )
            return;
        if (!shouldComeback)
        {
        transform.position = Vector2.MoveTowards(transform.position, nextTarget.position, bounceSpeed * Time.deltaTime);
        }

        if (Vector2.Distance(transform.position, nextTarget.position) < 0.7f)
        {
            DamageEnemiesInRadius(transform, 1);
            BounceNextTarget();
        }
        if (shouldComeback == false && (bounceCount <= 0 || nextTarget == null))
        {
            nextTarget = null;
            SwordComebackOn();
        }
    }
    private void BounceNextTarget()
    {
        nextTarget = GetNextTarget();
        bounceCount--;
    }
    private Transform GetNextTarget()
    {
        List<Transform> aliveTarget = GetAliveEnemies();
        if(aliveTarget.Count < 2)
            return null;

        List<Transform> validTargets = GetValidTargets();
        int randomIndex = Random.Range(0, validTargets.Count);
        Transform target = validTargets[randomIndex];

        selectedBefore.Add(target);

        return target;
    }
    private List<Transform> GetValidTargets()
    {
        List<Transform> validTargets = GetAliveEnemies();


        validTargets.RemoveAll(t => selectedBefore.Contains(t));
        if(validTargets.Count > 0)
            return validTargets;
        else
        {
            selectedBefore.Clear();
            return GetAliveEnemies();
        }
    }
    private List<Transform> GetAliveEnemies()
    {
        List<Transform> aliveEnemies = new List<Transform>();
        foreach (var enemy in enemyTargets)
        {
            if (enemy != null)
            {
                aliveEnemies.Add(enemy.transform);
            }
        }
        return aliveEnemies;
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if(enemyTargets == null || enemyTargets.Length == 0)
        {
            enemyTargets = GetEnemiesAround(transform, 10);
            rb.simulated = false;
        }
        DamageEnemiesInRadius(transform, 1);

        if(enemyTargets.Length < 2 || bounceCount <= 0)
            SwordComebackOn();
        else
        {
            nextTarget = GetNextTarget();
        }
    }
}
