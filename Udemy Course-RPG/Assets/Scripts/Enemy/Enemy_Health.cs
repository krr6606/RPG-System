using UnityEngine;

public class Enemy_Health : Entity_Health
{
    private Enemy enemy => GetComponent<Enemy>();
    public override void TakeDamage(float damageAmount, Transform damageDealer)
    {
        base.TakeDamage(damageAmount, damageDealer);
        if(isDead)
        {
            return;
        }
        if (damageDealer.CompareTag("Player"))
        {
            enemy.TryEnterBattleState(damageDealer);
        }
        // Additional enemy-specific damage logic can be added here
        Debug.Log("Enemy took " + damageAmount + " damage.");
    }
}
