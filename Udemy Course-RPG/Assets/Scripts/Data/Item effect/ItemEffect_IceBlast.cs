using UnityEngine;
[CreateAssetMenu(fileName = "item effect Data - 아이스 블레스트", menuName = "ScriptableObjects/Item Data/Item effect/Ice Blast", order = 1)]
public class ItemEffect_IceBlast : ItemEffectDataSO
{
    [SerializeField] private ElementalEffectData elementalEffectData;
    [SerializeField] private float iceDamage;
    [SerializeField] private LayerMask enemyMask;

    [Space]
    [SerializeField] private float healthPercentTrigger = 0.25f;
    [SerializeField] private float cooldown;
    private float lastTimeUsed = -9999; 
    private bool isNotFirst = false;
    [Header("VFX Obj")]
    [SerializeField] private GameObject iceBlastVFX; 
    [SerializeField] private GameObject onHitVFX;

    private void OnEnable()
    {
        // 에디터에서만 초기화

        lastTimeUsed = -999f;

    }

    public override void ExecuteEffect()
    {
        if(!isNotFirst)
        {
            lastTimeUsed = -9999;

            isNotFirst = true;
        }
        bool noCooldown = Time.time > lastTimeUsed + cooldown;
        bool reachedTreshold = player.health.GetHealthPercentage() <= healthPercentTrigger;

        if(noCooldown && reachedTreshold)
        {

            player.playerVFX.CreateEffectOf(iceBlastVFX, player.transform);
            lastTimeUsed = Time.time;
            DamageEnemiesWithIce();
        } 
    }

    private void DamageEnemiesWithIce()
    {

        Collider2D[] enemies = Physics2D.OverlapCircleAll(player.transform.position, 1.5f, enemyMask);
        Debug.Log(enemies.Length);
        foreach(var target in enemies)
        {
            IDamagable damagable = target.GetComponent<IDamagable>();
            if(damagable == null) continue;
            bool targetGotHit = damagable.TakeDamage(0,iceDamage, ElementType.Ice, player.transform);
            Entity_StatusHendler entity_StatusHendler = target.GetComponent<Entity_StatusHendler>();
            entity_StatusHendler?.ApplyStatusEffect(ElementType.Ice,elementalEffectData);
            if (targetGotHit)
            {
                player.playerVFX.CreateEffectOf(onHitVFX, player.transform);

            }
        }
    }
    public override void Subscribe(Player player)
    {
        base.Subscribe(player);
        player.health.OnTakingDamage += ExecuteEffect;
    }
    public override void Unsubscribe()
    {
        base.Unsubscribe();
        player.health.OnTakingDamage -= ExecuteEffect;
        player = null;
    }
}
