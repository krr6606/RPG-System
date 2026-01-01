using UnityEngine;

public class Skill_Base : MonoBehaviour
{

    public Player player { get; private set; }

    public DamageScaleData damageScaleData {get; protected set; }

    [Header("General Details")]
    [SerializeField] protected SkillType skillType;
    [SerializeField] protected SkillUpgradeType skillUpgradeType;
    [SerializeField] protected float cooldownTime;
    private float lastUsedTime;

    protected virtual void Awake()
    {
        player = GetComponentInParent<Player>();
    }
    private void Start()
    {
        lastUsedTime -= cooldownTime;
    }
    public virtual void TryUseSkill()
    {
       
    }
    public void SetSkillUpgrade(UpgradeData upgradeData)
    {
        skillUpgradeType = upgradeData.upgradeType;
        cooldownTime = upgradeData.cooldownTime;
        damageScaleData = upgradeData.damageScaleData;
    }
    public bool canUseSkill()
    {
        if (skillUpgradeType == SkillUpgradeType.None)
        {
            Debug.Log("Skill not Unlocked");
            return false;
        }
        if (OnCooldown())
        {
            Debug.Log("Skill on Cooldown");
            return false;

        }
        return true;
    }

    protected bool Unlocked(SkillUpgradeType upgradeType) => upgradeType == skillUpgradeType;
    protected bool OnCooldown()
    {
        //이전 사용 시간 + 쿨타임이 현재 시간보다 크면 쿨타임 중
        return Time.time < lastUsedTime + cooldownTime;
    }
    public void SetSkillOnCooldown()
    {
        lastUsedTime = Time.time;
    }
    public void ResetCooldownBy(float cooldownReduction)
    {
        lastUsedTime = lastUsedTime + cooldownReduction;
    }
    public void ResetCooldown()
    {
        lastUsedTime = Time.time;
    }
}
