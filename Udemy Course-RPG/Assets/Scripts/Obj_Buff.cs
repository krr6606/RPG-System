using System.Collections;
using UnityEngine;

public class Obj_Buff : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    [Header("Floating Settings")]
    [SerializeField] float floatSpeed = 1f;
    [SerializeField] float floatRange = 0.5f;
    private Vector3 startPos;
    [Header("Buff Settings")]
    [SerializeField] private float buffDuration = 5f;
    [SerializeField] private bool canBeUsed = true;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        startPos = transform.position;
    }
    private void Update()
    {
        float newY = startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatRange;
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!canBeUsed)
            return;
        StartCoroutine(BuffCoroutine(buffDuration));
    }
    IEnumerator BuffCoroutine(float duration)
    {
        canBeUsed = false;
        spriteRenderer.color = Color.clear;
        Debug.Log("Buff Applied");
        yield return new WaitForSeconds(duration);
        Debug.Log("Buff Ended");
        Destroy(gameObject);
    }
}
