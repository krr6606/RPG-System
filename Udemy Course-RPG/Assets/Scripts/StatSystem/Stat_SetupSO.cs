using UnityEngine;
[CreateAssetMenu(fileName = "Default Stat Setup", menuName = "ScriptableObjects/StatSystem/Stat_SetupSO", order = 1)]
public class Stat_SetupSO : ScriptableObject
{
    [Header("Resources")]
    public float maxHealth = 100f;
    public float healthRegen;

    [Header("Offense - Phyiscal Damage")]
    public float attackSpeed = 1f;
    public float Damage = 10f;
    public float critChance;
    public float critPower = 150f;
    public float armorReduction;

    [Header("Offense - Elemental Damage")]
    public float fireDamage;
    public float iceDamage;
    public float lightningDamage;

    [Header("Defense - Physical Resistance")]
    public float armor;
    public float evasion;

    [Header("Defense - Elemental Resistance")]
    public float fireResist;
    public float iceResist;
    public float lightningResist;

    [Header("Major Stats")]
    public float strength;
    public float agility;
    public float intelligence;
    public float vitality;

}
