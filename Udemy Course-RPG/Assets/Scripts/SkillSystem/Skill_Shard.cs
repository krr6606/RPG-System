using System.Collections;
using UnityEngine;

public class Skill_Shard : Skill_Base
{
    private SkillObgect_Shard currentShard;
    private Entity_Health playerHealth;
    [SerializeField] private GameObject skillObjectPrefab;
    [SerializeField] private float detonationDelay = 2f;
    [Header("Shard Moving Values")]
    public float shardSpeed = 5f;

    [Header("MultiCast Values")]
    [SerializeField] int maxShardCharges = 3;
    [SerializeField] int currentShardCharges = 0;
    [SerializeField] bool isRecharging = false;

    [Header("Teleport Values")]
    [SerializeField] private float shardExistDuration = 5f;
    [Header("Health Rewind Values")]
    [SerializeField] private float savedHealthPercentage;
    protected override void Awake()
    {
        base.Awake();
        playerHealth = player.GetComponentInParent<Entity_Health>();
    }
    private void Start()
    {
        currentShardCharges = maxShardCharges;
    }
    public void CreateShard()
    {
        float Delay = GetDetonationDelay();
        GameObject shard = Instantiate(skillObjectPrefab, transform.position, Quaternion.identity);
        currentShard = shard.GetComponent<SkillObgect_Shard>();
        currentShard.SetupShard(this);

        if(Unlocked(SkillUpgradeType.Shard_Teleport) || Unlocked(SkillUpgradeType.Shard_TeleportHpRewind))
        {
            currentShard.OnShardExploded += ForceCooldown;
        }
    }
    public void CreateRawShard(Transform target = null, bool canMoveShards = false)
    {
        bool canMove = canMoveShards == false ? Unlocked(SkillUpgradeType.Shard_MoveToEnemy) || Unlocked(SkillUpgradeType.Shard_MultiCast) : true;
        GameObject shard = Instantiate(skillObjectPrefab, transform.position, Quaternion.identity);
        shard.GetComponent<SkillObgect_Shard>().SetupShard(this, GetDetonationDelay(), canMove, shardSpeed,target);
    }

    public override void TryUseSkill()
    {
        if (!canUseSkill()) return;
        if (Unlocked(SkillUpgradeType.Shard))
        {
            HandleShardRegular();
        }
        if (Unlocked(SkillUpgradeType.Shard_MoveToEnemy))
        {
            HandleShardMoveToTarget();
        }
        if (Unlocked(SkillUpgradeType.Shard_MultiCast))
        {
            HandleShardMultiCast();
        }
        if(Unlocked(SkillUpgradeType.Shard_Teleport))
        {
            HandleShardTeleport();
        }
        if(Unlocked(SkillUpgradeType.Shard_TeleportHpRewind))
        {
            HandleShardTeleportHpRewind();
        }
    }
    private void HandleShardTeleportHpRewind()
    {
        if (currentShard == null)
        {
            CreateShard();
            savedHealthPercentage = playerHealth.GetHealthPercentage();
        }
        else
        {

            SwapPlayerAndShard();
            playerHealth.SetHealthToPercent(savedHealthPercentage);
            SetSkillOnCooldown();
        }
    }
    private void HandleShardTeleport()
    {
        if (currentShard == null)
        {
            CreateShard();
        }
        else
        {
            SwapPlayerAndShard();

            SetSkillOnCooldown();
        }
    }
    private void SwapPlayerAndShard()
    {
        Vector3 shardPosition = currentShard.transform.position;
        currentShard.transform.position = player.transform.position;
        player.TeleportToPosition(shardPosition);
        currentShard.Explode();
    }
    private void HandleShardMultiCast()
    {
        if (currentShardCharges < 0)
            return;
        CreateShard();
        currentShard.MoveTowardsTarget(shardSpeed);
        currentShardCharges--;
        if (!isRecharging)
            StartCoroutine(ShardRechargeCoroutine());
    }

    private IEnumerator ShardRechargeCoroutine()
    {
        isRecharging = true;
        while (currentShardCharges < maxShardCharges)
        {
            yield return new WaitForSeconds(cooldownTime);
            currentShardCharges++;
        }
        isRecharging = false;
    }
    private void HandleShardMoveToTarget()
    {
        CreateShard();
        currentShard.MoveTowardsTarget(shardSpeed);
        SetSkillOnCooldown();
    }
    private void HandleShardRegular()
    {
        CreateShard();
        SetSkillOnCooldown();
    }

    public float GetDetonationDelay()
    {
        if(Unlocked(SkillUpgradeType.Shard_Teleport) || Unlocked(SkillUpgradeType.Shard_TeleportHpRewind))
        {
            return shardExistDuration;
        }
        return detonationDelay;
    }
    public void ForceCooldown()
    {
        if (!OnCooldown())
        {
            SetSkillOnCooldown();
            currentShard.OnShardExploded -= ForceCooldown;
        }
    }
}
