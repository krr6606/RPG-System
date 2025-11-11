using UnityEngine;

public class VFX_AutoController : MonoBehaviour
{
    [SerializeField]private bool autoDestroy = true;
    [SerializeField]private float autoDestroyTime = 1f;
    private void Start()
    {
        if (autoDestroy)
        {
            Destroy(gameObject, autoDestroyTime);
        }
    }
}
