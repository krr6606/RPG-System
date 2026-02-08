using UnityEngine;
using TMPro;
public class UI_StatToolTip : UI_ToolTip
{
    private Player_Stats playerStats;
    private TextMeshProUGUI toolTipText;

    protected override void Awake()
    {
        base.Awake();
        playerStats = FindFirstObjectByType<Player_Stats>();
        toolTipText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void ShowToolTip(bool show, RectTransform targetRect, StatType statType)
    {
        base.ShowToolTip(show, targetRect);
        toolTipText.text = GetStatToolTipTextByType(statType);

    }
    public string GetStatToolTipTextByType(StatType statType)
    {
        //한국어로 각 스탯에 대한 설명을 반환합니다.
        switch (statType) { 
            case StatType.MaxHealth:
                return "최대 체력을 나타냅니다.";
            case StatType.healthRegen:
                return "시간당 체력 회복량을 나타냅니다.";
            case StatType.Strangth:
                return "공격력과 치명타 피해량에 영향을 미칩니다.";
            case StatType.Agility:
                return "공격 속도와 회피율에 영향을 미칩니다.";
            case StatType.Intelligence:
                return "속성 공격력에 영향을 미칩니다.";
            case StatType.Vitality:
                return "시간당 체력 회복량에 영향을 미칩니다.";
            case StatType.AtackSpeed:
                return "공격 속도를 증가시킵니다.";
            case StatType.Damage:
                return "물리 공격력을 나타냅니다.";
            case StatType.CritChance:
                return "치명타 확률을 나타냅니다.";
            case StatType.CritPower:
                return "치명타 피해량을 나타냅니다.";
            case StatType.ArmorReduction:
                return "적의 방어력을 감소시키는 비율을 나타냅니다.";
            case StatType.FireDamage:
                return "화염 속성 공격력을 나타냅니다.";
            case StatType.IceDamage:
                return "냉기 속성 공격력을 나타냅니다.";
            case StatType.LightningDamage:
                return "번개 속성 공격력을 나타냅니다.";
            case StatType.Armor:
                return "물리 방어력을 나타냅니다." + "\n최대 85%" + "\n현재 " + playerStats.GetArmorMitigation(0) * 100 + "%";
            case StatType.Evasion:
                return "적의 공격을 회피할 확률을 나타냅니다.";
            case StatType.IceResistance:
                return "냉기 속성 피해에 대한 저항력을 나타냅니다.";
            case StatType.FireResistance:
                return "화염 속성 피해에 대한 저항력을 나타냅니다.";
            case StatType.LightningResistance:
                return "번개 속성 피해에 대한 저항력을 나타냅니다.";
            case StatType.ElementalDamage:
                return "모든 속성 공격력의 합산을 나타냅니다.";
            default:
                return "알 수 없는 스탯입니다.";
        }
    }
}
