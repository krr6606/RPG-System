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
    [Header("Elemental Color Settings")]
    [SerializeField] private Color chillVfxColor = Color.cyan;
    [SerializeField] private Color fireVfxColor = Color.red;
    [SerializeField] private Color electricVfxColor = Color.yellow;
    private Color originalVfxColor;
    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        entity = GetComponent<Entity>();
        originalMaterial = spriteRenderer.material;
        originalVfxColor = HitVFXColor;
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
    public void UpdateOnVfxColor(ElementType elementType)
    {
        switch(elementType)
        {
            case ElementType.Ice:
                HitVFXColor = chillVfxColor;
                break;
            case ElementType.Fire:
                HitVFXColor = fireVfxColor;
                break;
            case ElementType.Lightning:
                HitVFXColor = electricVfxColor;
                break;
            case ElementType.None:
                HitVFXColor = originalVfxColor;
                break;
            default:
                HitVFXColor = originalVfxColor;
                break;
        }
    }
    public void PlayStatusVFX(float duration, ElementType elementType)
    {
        if(elementType == ElementType.Ice)
            StartCoroutine(PlayStatusVFXCoroutine(duration, chillVfxColor));
        if(elementType == ElementType.Fire)
            StartCoroutine(PlayStatusVFXCoroutine(duration, fireVfxColor));
        if(elementType == ElementType.Lightning)
            StartCoroutine(PlayStatusVFXCoroutine(duration, electricVfxColor));
    }
    public void StopStatusVFX()
    {
        StopAllCoroutines();
        spriteRenderer.color = Color.white;
        spriteRenderer.material = originalMaterial;
    }
    private IEnumerator PlayStatusVFXCoroutine(float duration, Color effectColor)
    {
        float tickInterval = 0.25f;
        float timer = 0f;

        Color lightColor = effectColor * 1.2f;
        Color darkColor = effectColor * 0.8f;
        bool toggle = false;
        while (timer < duration)
        {
            spriteRenderer.color = toggle ? lightColor : darkColor;
            // Assume UpdateVFXColor is a method that updates the VFX color
            // UpdateVFXColor(currentColor);
            toggle = !toggle;
            timer += tickInterval;
            yield return new WaitForSeconds(tickInterval);
       
        }
        spriteRenderer.color = Color.white;
    }
    private IEnumerator OnDamageVFX()
    {
        spriteRenderer.material = OnDamageMaterial;
        yield return new WaitForSeconds(OnDamageVfxDuration);
        spriteRenderer.material = originalMaterial;
    }
}
