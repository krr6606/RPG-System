using TMPro;
using UnityEngine;

public class UI_SkillToolTip : UI_ToolTip
{
    [SerializeField] private TextMeshProUGUI skillNameText;
    [SerializeField] private TextMeshProUGUI skillDescriptionText;
    [SerializeField] private TextMeshProUGUI skillRequirementsText;

    public override void ShowToolTip(bool show, RectTransform targetRect)
    {

        base.ShowToolTip(show, targetRect);
    }
    public void ShowToolTip(bool show, RectTransform targetRect, Skill_DataSO skillData)
    {

        if (show == false)
        {
            return;
        }
        skillNameText.text = skillData.Name;
        skillDescriptionText.text = skillData.Description;
        skillRequirementsText.text = "Requirements: \n" + " -" + skillData.cost +" skill point.";
        base.ShowToolTip(show, targetRect);
    }
}
