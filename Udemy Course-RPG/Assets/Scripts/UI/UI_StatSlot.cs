using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_StatSlot : MonoBehaviour,IPointerEnterHandler, IPointerExitHandler
{
    private Player_Stats playerStats;
    private RectTransform rectTransform;
    private UI ui;

    [SerializeField] private StatType statSlotType;
    [SerializeField] private TextMeshProUGUI statName;
    [SerializeField] private TextMeshProUGUI statValue;

    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.statToolTip.ShowToolTip(true, rectTransform, statSlotType);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.statToolTip.ShowToolTip(false, null);
    }
    private void OnValidate()
    {
        gameObject.name = "UI_StatSlot - " + GetStatNameByType(statSlotType);
        statName.text = GetStatNameByType(statSlotType);
    }
    private void Awake()
    {
        playerStats = FindFirstObjectByType<Player_Stats>();
        rectTransform = GetComponent<RectTransform>();
        ui = GetComponentInParent<UI>();
    }
    public void UpdateStatValue()
    {
        Stat stat = playerStats.GetStatByType(statSlotType);
        if (stat == null && statSlotType != StatType.ElementalDamage) return;

        float value = 0;

        switch (statSlotType)
        {
            // Major Stats
            case StatType.Strangth:
                value = playerStats.majorStats.strength.GetValue();
                break;
            case StatType.Agility:
                value = playerStats.majorStats.agility.GetValue();
                break;
            case StatType.Intelligence:
                value = playerStats.majorStats.intelligence.GetValue();
                break;
            case StatType.Vitality:
                value = playerStats.majorStats.vitality.GetValue();
                break;

            //Offence Stats
            case StatType.Damage:
                value = playerStats.GetBaseDamage();
                break;
            case StatType.CritChance:
                value = playerStats.GetCritChance();
                break;
            case StatType.CritPower:
                value = playerStats.GetCritPower();
                break;
            case StatType.ArmorReduction:
                value = playerStats.GetArmorReduction() * 100;
                break;
            case StatType.AtackSpeed:
                value = playerStats.offenceStats.attackSpeed.GetValue() * 100;
                break;

            // Defence Stats
            case StatType.MaxHealth:
                value = playerStats.GetMaxHP();
                break;
            case StatType.healthRegen:
                value = playerStats.statResourceGroup.healthRegen.GetValue();
                break;
            case StatType.Evasion:
                value = playerStats.GetEvasion();
                break;
            case StatType.Armor:
                value = playerStats.GetBaseArmor();
                break;

            // Elemental Stats
            case StatType.FireDamage:
                value = playerStats.offenceStats.fireDamage.GetValue();
                break;
            case StatType.IceDamage:
                value = playerStats.offenceStats.iceDamage.GetValue();
                break;
            case StatType.LightningDamage:
                value = playerStats.offenceStats.lightningDamage.GetValue();
                break;
            case StatType.ElementalDamage:
                value = playerStats.GetElementalDamage(out ElementType elementType, 1);
                break;

            //Elemental Resistances
            case StatType.FireResistance:
                value = playerStats.GetElementalResistance(ElementType.Fire);
                break;
            case StatType.IceResistance:
                value = playerStats.GetElementalResistance(ElementType.Ice);
                break;
            case StatType.LightningResistance:
                value = playerStats.GetElementalResistance(ElementType.Lightning);
                break;
        }

        statValue.text = IsPercentageStat(statSlotType) ? value.ToString("F1")   + "%" : value.ToString("F0");
    }

    private string GetStatNameByType(StatType statType)
    {
        switch (statType)
        {
            case StatType.MaxHealth:
                return "최대 체력";
            case StatType.healthRegen:
                return "채력 재생력";
            case StatType.Strangth:
                return "힘";
            case StatType.Agility:
                return "민첩";
            case StatType.Intelligence:
                return "지능";
            case StatType.Vitality:
                return "활력";
            case StatType.AtackSpeed:
                return "공격 속도";
            case StatType.Damage:
                return "피해량";
            case StatType.CritChance:
                return "치명타 확률";
            case StatType.CritPower:
                return "치명타 피해량";
            case StatType.ArmorReduction:
                return "방어구 관통";
            case StatType.FireDamage:
                return "화염 피해";
            case StatType.IceDamage:
                return "냉기 피해";
            case StatType.LightningDamage:
                return "번개 피해";
            case StatType.Armor:
                return "방어력";
            case StatType.Evasion:
                return "회피율";
            case StatType.IceResistance:
                return "냉기 저항력";
            case StatType.FireResistance:
                return "화염 저항력";
            case StatType.LightningResistance:
                return "번개 저항력";
            case StatType.ElementalDamage:
                return "원소 피해";
            default:
                return "???";

        }
    }

    private bool IsPercentageStat(StatType statType)
    {
        switch (statType)
        {
            case StatType.CritChance:
            case StatType.ArmorReduction:
            case StatType.Evasion:
            case StatType.IceResistance:
            case StatType.FireResistance:
            case StatType.LightningResistance:
                return true;
            default:
                return false;
        }
    }

}
