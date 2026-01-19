using System.Collections.Generic;
using UnityEngine;

public class Skill_DomainExpansion : Skill_Base
{
    [SerializeField] private GameObject domainPrefab;


    [Header("Slowing Down Settings")]
    [Range(0f, 1f)]
    [SerializeField] private float slowDownPercent = .8f;
    [SerializeField] private float slowDownDuration = 5f;

    [Header("Shard Cast Settings")]
    [SerializeField] private int shardsPerCast = 10;
    [Range(0f, 1f)]
    [SerializeField] private float shardCastDomainSlowPercent = 0.5f;
    [SerializeField] private float shardCastDuration = 2f;

    [Header("Time Echo Cast Settings")]
    [SerializeField] private int echoesPerCast = 8;
    [Range(0f, 1f)]
    [SerializeField] private float echoCastDomainSlowPercent = 0.1f;
    [SerializeField] private float echoCastDuration = 6f;
    [SerializeField] private float healthRestorePerEcho = 0.05f;
    private float spellCastTimer;
    private float spellsPerSecond;
    [Header("Domain Expansion Settings")]
    public float maxDomainSize = 10f;
    public float domainExpansionSpeed = 2.6f;


    private List<Enemy> trappedEnemies = new List<Enemy>();
    private Transform currentTarget;
    public void CreateDomain()
    {
        spellsPerSecond = GetSpellPerCast()/GetDomainDuration();
        GameObject domain = Instantiate(domainPrefab, player.transform.position, Quaternion.identity);
        domain.GetComponent<SkillObject_DomainExpansion>().SetupDomain(this);
    }

    private Transform FindTargetInDomain()
    {
        trappedEnemies.RemoveAll(target => target == null || target.health.isDead);

        if (trappedEnemies.Count == 0)
            return null;
        int randomIndex = Random.Range(0, trappedEnemies.Count);
        
        return trappedEnemies[randomIndex].transform;
 
    }
    public void DoSpellCasting()
    {
        spellCastTimer -= Time.deltaTime;

        if(currentTarget == null)
            currentTarget = FindTargetInDomain();
        if(currentTarget != null && spellCastTimer < 0)
        {
            CastSpell(currentTarget);
            spellCastTimer = 1f / (spellsPerSecond);
            currentTarget = null;
        }
    }
    private void CastSpell(Transform target)
    {
       if(skillUpgradeType == SkillUpgradeType.Domain_EchoSpam)
       {
            Vector3 offset = Random.value < .5f ? Vector3.left : Vector3.right;
            player.skillManager.timeEchoSkill.CreateTimeEcho(target.position + offset);
        }
       else if(skillUpgradeType == SkillUpgradeType.Domain_ShardSpam)
       {
            player.skillManager.shardSkill.CreateRawShard(target, true);
        }
    }
    public float GetDomainDuration()
    {
        switch (skillUpgradeType)
        {
            case SkillUpgradeType.Domain_SlowingDown:
                return slowDownDuration;
            case SkillUpgradeType.Domain_EchoSpam:
                return echoCastDuration;
            case SkillUpgradeType.Domain_ShardSpam:
                return shardCastDuration;
            default:
                return 0f;
        }
    }
    public float GetDomainSlowPercent()
    {
        switch (skillUpgradeType)
        {
            case SkillUpgradeType.Domain_SlowingDown:
                return slowDownPercent;
            case SkillUpgradeType.Domain_EchoSpam:
                return echoCastDomainSlowPercent;
            case SkillUpgradeType.Domain_ShardSpam:
                return shardCastDomainSlowPercent;
            default:
                return 0f;
        }
    }
    public float GetSpellPerCast()
    {
        switch (skillUpgradeType)
        {
            case SkillUpgradeType.Domain_EchoSpam:
                return echoesPerCast;
            case SkillUpgradeType.Domain_ShardSpam:
                return shardsPerCast;
            default:
                return 0f;
        }
    }
    public bool InstantDomain()
    {
        return skillUpgradeType != SkillUpgradeType.Domain_EchoSpam 
            && skillUpgradeType != SkillUpgradeType.Domain_ShardSpam;
    }

    public void AddTarget(Enemy enemy)
    {
        if (!trappedEnemies.Contains(enemy))
            trappedEnemies.Add(enemy);
    }
    public void ClearTargets()
    {
        foreach(var enemy in trappedEnemies)
        {
            enemy.StopSlowDown();
        }
        trappedEnemies = new List<Enemy>();
    }
}
