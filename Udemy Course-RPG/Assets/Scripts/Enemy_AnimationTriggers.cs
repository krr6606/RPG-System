using UnityEngine;

public class Enemy_AnimationTriggers : Entity_AnimationTriggers
{
    Enemy enemy;
    private Enemy_VFX enemyVFX;
    protected override void Awake()
    {
        base.Awake();
        enemy = GetComponentInParent<Enemy>();
        enemyVFX = GetComponentInParent<Enemy_VFX>();
    }

    private void EnableCounterWindow()
    {
        enemyVFX.EnableCounterWindowVFX(true);
        enemy.EnableCounterWindow(true);
    }
    private void disableCounterWindow()
    {
        enemyVFX.EnableCounterWindowVFX(false);
        enemy.EnableCounterWindow(false);
    }
}
