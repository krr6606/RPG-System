using UnityEngine;

public class UI_MiniHealthBar : MonoBehaviour
{
    private Entity entity;
    private void Awake()
    {
        entity = GetComponentInParent<Entity>();
    }
    private void OnEnable()
    {
        entity.OnFlipped += HandleOnFlipped;
    }
    private void OnDisable()
    {
        entity.OnFlipped -= HandleOnFlipped;
    }
    private void HandleOnFlipped() => transform.rotation = Quaternion.identity;

}
