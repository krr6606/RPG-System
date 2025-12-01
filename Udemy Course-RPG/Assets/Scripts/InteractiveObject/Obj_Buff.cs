using System.Collections;
using UnityEngine;
[System.Serializable]
public class Buff
{
    public StatType Type;
    public float Value;
}

public class Obj_Buff : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Entity_Stat statToModify;
    [Header("Floating Settings")]
    [SerializeField] float floatSpeed = 1f;
    [SerializeField] float floatRange = 0.5f;
    private Vector3 startPos;
    [Header("Buff Settings")]
    [SerializeField] private Buff[] buffs;
    [SerializeField] private string buffName;
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
        statToModify = collision.GetComponent<Entity_Stat>();
        StartCoroutine(BuffCoroutine(buffDuration));
    }
    IEnumerator BuffCoroutine(float duration)
    {
        canBeUsed = false;
        spriteRenderer.color = Color.clear;
        Debug.Log("Buff Applied");
        ApplyBuff(true);
        yield return new WaitForSeconds(duration);
        ApplyBuff(false);
        Debug.Log("Buff Ended");
        Destroy(gameObject);
    }

    private void ApplyBuff(bool isApply)
    {
        foreach (var buff in buffs)
            if(isApply)
                statToModify.GetStatByType(buff.Type).AddModifier(buff.Value, buffName);
            else
                statToModify.GetStatByType(buff.Type).RemoveModifierBySource(buffName);
    }
}
