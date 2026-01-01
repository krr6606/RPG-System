using UnityEngine;
using System.Collections;
public class VFX_AutoController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    [SerializeField]private bool autoDestroy = true;
    [SerializeField]private float autoDestroyTime = 1f;

    [SerializeField] bool canFade;
    [SerializeField] float fadeSpeed = 1f;
    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }
    private void Start()
    {
        if(canFade)
        {
            StartCoroutine(FadeOutCoroution());
        }

        if (autoDestroy)
        {
            Destroy(gameObject, autoDestroyTime);
        }
    }
    private IEnumerator FadeOutCoroution()
    {
        Color color = Color.white;
        while (color.a > 0)
        {
            color.a -= fadeSpeed * Time.deltaTime;
            spriteRenderer.color = color;
            yield return null;
        }
        spriteRenderer.color = color;
    }
}
