using System.Collections;

using System.Text;
using TMPro;
using UnityEngine;

public class UI_SkillToolTip : UI_ToolTip
{
    private UI ui;
    private UI_SkillTree skillTreeUI;
    [SerializeField] private TextMeshProUGUI skillNameText;
    [SerializeField] private TextMeshProUGUI skillDescriptionText;
    [SerializeField] private TextMeshProUGUI skillCooldownText;
    [SerializeField] private TextMeshProUGUI skillRequirementsText;

    [Space]
    [SerializeField] private string metConditionHex;
    [SerializeField] private string unmetConditionHex;
    [SerializeField] private string importantConditionHex;
    [SerializeField] private Color exampleColor;
    [SerializeField] private string LockedSkillText = "루트가 잠긴 스킬";

    private Coroutine textEffectCoroutine;

    protected override void Awake()
    {
        base.Awake();
        ui = GetComponentInParent<UI>();
        skillTreeUI = ui.GetComponentInChildren<UI_SkillTree>(true);
    }
    public override void ShowToolTip(bool show, RectTransform targetRect)
    {

        base.ShowToolTip(show, targetRect);
    }
    public void ShowToolTip(bool show, RectTransform targetRect, UI_TreeNode treeNode)
    {

        if (show == false)
        {
            return;
        }
        Skill_DataSO skillData = treeNode.skillData;
        skillNameText.text = skillData.Name;
        skillDescriptionText.text = skillData.Description;
        skillCooldownText.text = "재사용 대기 시간: " + skillData.upgradeData.cooldownTime + "초";
        skillRequirementsText.text = treeNode.isLocked ? "<color=#CBDFBF>" + LockedSkillText + "</color>" : GetRequirementsText(treeNode.skillData.cost, treeNode.neededNodes, treeNode.conflictNodes);
        base.ShowToolTip(show, targetRect);
    }
    public void LockedSkillEffect()
    {
        if (textEffectCoroutine != null)
            {
            StopCoroutine(textEffectCoroutine);
            }
        textEffectCoroutine = StartCoroutine(TextBlinkEffectCoroutine(skillRequirementsText, 0.25f, 3f));
    }
    private IEnumerator TextBlinkEffectCoroutine(TextMeshProUGUI text, float blinkInterval, float blinkCount)
    {
        for (int i = 0; i < blinkCount; i++)
        {
            text.text = GetColoredText(LockedSkillText, importantConditionHex);
            yield return new WaitForSeconds(blinkInterval);
            text.text = GetColoredText(LockedSkillText, "#CBDFBF");
            yield return new WaitForSeconds(blinkInterval);
        }
    }
    private string GetRequirementsText(int skillCost, UI_TreeNode[] neededNode, UI_TreeNode[] conflictNodes)
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("해금 조건:");
        string costcolor = skillTreeUI.HasEnoughPoints(skillCost) ? metConditionHex : unmetConditionHex;
        stringBuilder.AppendLine($"- <color={costcolor}> {skillCost} 스킬 포인트.</color>");
        foreach (var node in neededNode)
        {
            if (node == null) continue;
            string nodeColor = node.isUnlocked ? metConditionHex : unmetConditionHex;
            stringBuilder.AppendLine(GetColoredText(node.skillData.Name + " 해금", nodeColor));
        }
        if (conflictNodes.Length <= 0)
            return stringBuilder.ToString();

        foreach (var node in conflictNodes)
        {
            if (node == null) continue;
            string nodeColor = node.isUnlocked ? unmetConditionHex : metConditionHex;
            stringBuilder.AppendLine(GetColoredText(node.skillData.Name + "미해금", nodeColor));
        }

        return stringBuilder.ToString();
    }


}
