using System.Collections;
using UnityEngine;

public class Obj_Buff : MonoBehaviour
{

    private Player_Stats statToModify;
    [Header("Floating Settings")]
    [SerializeField] float floatSpeed = 1f;
    [SerializeField] float floatRange = 0.5f;
    private Vector3 startPos;
    [Header("Buff Settings")]
    [SerializeField] private BuffEffectData[] buffs;
    [SerializeField] private string buffName;
    [SerializeField] private float buffDuration = 5f;

    private void Awake()
    {

        startPos = transform.position;

    }
    private void Update()
    {
        float newY = startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatRange;
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        statToModify = collision.GetComponent<Player_Stats>();

        if(statToModify != null && statToModify.CanApplyBuffOf(buffName))
        {
            statToModify.ApplyBuff(buffs, buffDuration, buffName);
            Destroy(gameObject);
        }

    }
}
