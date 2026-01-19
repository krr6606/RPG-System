using System;
using UnityEngine;

public class SkillObgect_Shard : SkillObject_Base
{
    public event Action OnShardExploded;
    private Skill_Shard skill_Shard;
    [SerializeField] private GameObject vfxPrefab;

    private Transform target;
    private float speed;


    private void Update()
    {
        if(target == null)
            return;
        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
    }
    public void MoveTowardsTarget(float speed,Transform newTarget = null)
    {
        target = newTarget == null ? TargetTracking()   :  newTarget;
        this.speed = speed;
    }

    public void SetupShard(Skill_Shard skill_Shard)
    {
        this.skill_Shard = skill_Shard;
        entityStat = skill_Shard.player.entityStat;
        damageScaleData = skill_Shard.damageScaleData;
        float detonationDelay = skill_Shard.GetDetonationDelay();
        Invoke(nameof(Explode), detonationDelay);
    }
    public void SetupShard(Skill_Shard skill_Shard, float detonationDelay, bool canMove, float shardSpeed, Transform target = null)
    {
        this.skill_Shard = skill_Shard;
        entityStat = skill_Shard.player.entityStat;
        damageScaleData = skill_Shard.damageScaleData;

        Invoke(nameof(Explode), detonationDelay);
        if(canMove)
        {
            MoveTowardsTarget(shardSpeed,target);
        }
    }
    public void Explode()
    {
        DamageEnemiesInRadius(targetCheck, checkRadius);
        SpriteRenderer spriteRenderer = Instantiate(vfxPrefab, transform.position, Quaternion.identity).GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.color = skill_Shard.player.playerVFX.GetElementColor(usedELementType);
        OnShardExploded?.Invoke();
        Destroy(gameObject);
    }
 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & EnemyMask) != 0)
        {
            Explode();
            Debug.Log("Shard exploded on enemy hit");
        }
    }

}

