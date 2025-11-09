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

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        originalMaterial = spriteRenderer.material;
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
