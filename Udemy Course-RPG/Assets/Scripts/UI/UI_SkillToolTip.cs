using System.Text;
using TMPro;
using UnityEngine;

public class UI_SkillToolTip : UI_ToolTip
{
    private UI_SkillTree skillTreeUI;
    [SerializeField] private TextMeshProUGUI skillNameText;
    [SerializeField] private TextMeshProUGUI skillDescriptionText;
    [SerializeField] private TextMeshProUGUI skillRequirementsText;

    [Space]
    [SerializeField] private string metConditionHex;
    [SerializeField] private string unmetConditionHex;
    [SerializeField] private Color exampleColor;
    [SerializeField] private string LockedSkillText = "<color=#CBDFBF>루트가 잠긴 스킬</color>";

    protected override void Awake()
    {
        base.Awake();
        skillTreeUI = GetComponentInParent<UI_SkillTree>();
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
        skillRequirementsText.text = treeNode.isLocked ? LockedSkillText : GetRequirementsText(treeNode.skillData.cost, treeNode.neededNodes, treeNode.conflictNodes);
        base.ShowToolTip(show, targetRect);
    }
    private string GetRequirementsText(int skillCost, UI_TreeNode[] neededNode, UI_TreeNode[] conflictNodes)
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("해금 조건:");
        string costcolor = skillTreeUI.HasEnoughPoints(skillCost) ? metConditionHex : unmetConditionHex;
        stringBuilder.AppendLine($"- <color={costcolor}> {skillCost} 스킬 포인트.</color>");
        foreach (var node in neededNode)
        {
            string nodeColor = node.isUnlocked ? metConditionHex : unmetConditionHex;
            stringBuilder.AppendLine($"- <color={nodeColor}>{node.skillData.Name} 해금.</color>");
        }
        if (conflictNodes.Length <= 0)
            return stringBuilder.ToString();

        foreach (var node in conflictNodes)
        {
            string nodeColor = node.isUnlocked ? unmetConditionHex : metConditionHex;
            stringBuilder.AppendLine($"- <color={nodeColor}>{node.skillData.Name} 미해금.</color>");
        }

        return stringBuilder.ToString();
    }
}
