using System.Collections;
using UnityEngine;

public class Entity_VFX : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    [Header("VFX Settings")]
    [SerializeField] private Material OnDamageMaterial;
    [SerializeField] private float OnDamageVfxDuration = 0.2f;
    private Material originalMaterial;
    private Coroutine onDamageCoroutine;
    [Header("On Doing Damage VFX")]
    [SerializeField] Color OnDoingDamageVFXColor = Color.white;
    [SerializeField] GameObject OnDoingDamageVFXPrefab;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        originalMaterial = spriteRenderer.material;
    }
    public void CreateOnDoingDamageVFX(Transform spawnPoint)
    {
        if(OnDoingDamageVFXPrefab != null)
        {
           GameObject vfxPrefab = Instantiate(OnDoingDamageVFXPrefab, spawnPoint.position, Quaternion.identity);
              var vfxSpriteRenderer = vfxPrefab.GetComponentInChildren<SpriteRenderer>();
                if(vfxSpriteRenderer != null)
                {
                    vfxSpriteRenderer.color = OnDoingDamageVFXColor;
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
