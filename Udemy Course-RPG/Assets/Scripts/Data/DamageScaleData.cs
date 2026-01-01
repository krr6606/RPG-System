using UnityEngine;
using System;
[Serializable]

public class DamageScaleData
{
    [Header("Damage Scales")]
    public float physicalDamageScale = 1;
    public float elementalDamageScale = 1;

    [Header("chill")]
    public float chillDuration = 2.4f;
    public float chillSlowAmountScale = 0.2f;

    [Header("burn")]
    public float burnDuration = 3f;
    public float burnDamageScale = 1;

    [Header("electric")]
    public float electricDuration = 2f;
    public float electricDamageScale = 1;
    public float electricCharge = 0.4f;
}
