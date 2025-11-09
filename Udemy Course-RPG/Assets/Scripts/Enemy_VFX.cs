using UnityEngine;

public class Enemy_VFX : Entity_VFX
{
    [Header("Counter Attack Window")]
    [SerializeField ] private GameObject counterWindowVFX;

    public void EnableCounterWindowVFX(bool enable)
    {

            counterWindowVFX?.SetActive(enable);

    }
}
