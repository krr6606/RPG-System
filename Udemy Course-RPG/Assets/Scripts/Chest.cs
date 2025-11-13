using UnityEngine;

public class Chest : MonoBehaviour ,IDamagable
{
    private Rigidbody2D rb => GetComponentInChildren<Rigidbody2D>();
    private Entity_VFX entityVFX => GetComponent<Entity_VFX>();
    [Header("Chest Settings")]
    [SerializeField] private Vector2 launchVelocity = new Vector2(0, 2f);

    public bool TakeDamage(float damageAmount, Transform damageDealer)
    {
        GetComponentInChildren<Animator>()?.SetBool("ChestOpen", true);
        rb.linearVelocity = launchVelocity;
        rb.angularVelocity = Random.Range(-150f, 150f);
        entityVFX.PlayOnDamageVFX();
        return true;
    }

}
