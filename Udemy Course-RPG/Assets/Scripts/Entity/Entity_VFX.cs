using System.Collections;
using UnityEngine;

public class Entity_VFX : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    Entity entity;
    [Header("VFX Settings")]
    [SerializeField] private Material OnDamageMaterial;
    [SerializeField] private float OnDamageVfxDuration = 0.2f;
    private Material originalMaterial;
    private Coroutine onDamageCoroutine;
    [Header("On Doing Damage VFX")]
    [SerializeField] Color HitVFXColor = Color.white;
    [SerializeField] GameObject HitVFXPrefab;
    [SerializeField] GameObject CritHitVFXPrefab;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        entity = GetComponent<Entity>();
        originalMaterial = spriteRenderer.material;
    }
    public void CreateOnHitVFX(Transform spawnPoint, bool isCrit)
    {
        if(HitVFXPrefab != null)
        {

           GameObject HitPrefab = isCrit ? CritHitVFXPrefab : HitVFXPrefab;
            GameObject vfxPrefab = Instantiate(HitPrefab, spawnPoint.position, Quaternion.identity);
              var vfxSpriteRenderer = vfxPrefab.GetComponentInChildren<SpriteRenderer>();
                if(!isCrit)
                {
                    vfxSpriteRenderer.color = HitVFXColor;
                }
                if(entity.facingDir == -1 && isCrit)
                {
                    vfxPrefab.transform.Rotate(0, 180, 0);
            }
        }
    }
    public void PlayOnDamageVFX()
    {
        if(onDamageCoroutine != null)
            StopCoroutine(onDamageCoroutine);

        onDamageCoroutine = StartCoroutine(OnDamageVFX());
    }
    private IEnumerator OnDamageVFX()
    {
        spriteRenderer.material = OnDamageMaterial;
        yield return new WaitForSeconds(OnDamageVfxDuration);
        spriteRenderer.material = originalMaterial;
    }
}
