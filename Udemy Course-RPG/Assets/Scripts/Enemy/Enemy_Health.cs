using UnityEngine;

public class Enemy_Health : Entity_Health
{
    private Enemy enemy => GetComponent<Enemy>();
    public override bool TakeDamage(float damageAmount, Transform damageDealer)
    {
        bool wasHit = base.TakeDamage(damageAmount, damageDealer);
        if(!wasHit)
        {
            return false;
        }
        if (damageDealer.CompareTag("Player"))
        {
            enemy.TryEnterBattleState(damageDealer);
        }
        // Additional enemy-specific damage logic can be added here
        Debug.Log("Enemy took " + damageAmount + " damage.");
        return true;
    }
}
