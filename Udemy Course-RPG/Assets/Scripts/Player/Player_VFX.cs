using System.Collections;
using UnityEngine;

public class Player_VFX : Entity_VFX
{
    [Header("Image Echo VFX")]
    [Range(0.01f, 0.2f)]
    [SerializeField] private float imageEchoSpawnInterval = 0.05f;
    [SerializeField] private GameObject imageEchoPrefab;
    private Coroutine imageEchoCoroutine;

    public void StartImageEchoEffect(float duration)
    {
        if(imageEchoCoroutine != null)
        {
            StopCoroutine(imageEchoCoroutine);
        }
        imageEchoCoroutine = StartCoroutine(ImageEchoEffectCoroution(duration));
    }
    private IEnumerator ImageEchoEffectCoroution(float duration)
    {
        float timer = 0f;
        while (timer < duration)
        {
            SpawnImageEcho();
            yield return new WaitForSeconds(imageEchoSpawnInterval);
            timer += imageEchoSpawnInterval;
        }
    }
    private void SpawnImageEcho()
    {
        GameObject imageEcho = Instantiate(imageEchoPrefab, transform.position, Quaternion.identity);
        SpriteRenderer echoSpriteRenderer = imageEcho.GetComponentInChildren<SpriteRenderer>();
        echoSpriteRenderer.sprite = spriteRenderer.sprite;
        echoSpriteRenderer.flipX = spriteRenderer.flipX;

    }
}
