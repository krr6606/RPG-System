using System.Collections;
using UnityEngine;

public class Entity_StatusHendler : MonoBehaviour
{
    private Entity entity;
    private Entity_VFX entity_VFX;
    private Entity_Stat entity_Stat;
    private ElementType ElementType = ElementType.None;

    void Awake()
    {
        entity = GetComponent<Entity>();
        entity_VFX = GetComponent<Entity_VFX>();
        entity_Stat = GetComponent<Entity_Stat>();
    }
    public void ApplyChilledEffect(float duration, float slowAmount)
    {
        float iceResistance = entity_Stat.GetElementalResistance(ElementType.Ice);
        float reducedDuration = duration * (1 - iceResistance);

        StartCoroutine(CilledEffectCoroutine(reducedDuration,slowAmount));


    }
    private IEnumerator CilledEffectCoroutine(float duration, float slowAmount)
    {
        entity.SlowDownEntity(duration, slowAmount);
        ElementType = ElementType.Ice;
        entity_VFX.PlayStatusVFX(duration,ElementType.Ice);
        yield return new WaitForSeconds(duration);
        ElementType = ElementType.None;
    }
    public bool canBeApplied(ElementType elementType)
    {
        return ElementType == ElementType.None;
    }
}
