using UnityEngine;

public class UI_SkillTree : MonoBehaviour
{
    public int skillPoints;
    [SerializeField] private UI_TreeConnectHandler[] parentNodes;
    public Player_SkillManager playerSkillManager { get; private set; }
    private void Awake()
    {
        playerSkillManager = FindAnyObjectByType<Player_SkillManager>();
    }

    [ContextMenu("Refund All Skills")]
    public void RefundAllskills()
    {
        UI_TreeNode[] allNodes = GetComponentsInChildren<UI_TreeNode>();
        foreach (var node in allNodes)
        {
            if (node.isUnlocked)
            {
                node.Refund();
            }
        }
    }
    public void RemoveSkillPoint(int cost)
    {
        skillPoints -= cost;
        Debug.Log("Skill Points Remaining: " + skillPoints);
    }
    public void AddSkillPoint(int cost) {
        skillPoints += cost;
    }
    public bool HasEnoughPoints(int cost)
    {
        return skillPoints >= cost;
    }
    private void Start()
    {
        UpdateAllConnections();
    }

    [ContextMenu("Update All Connections")]
    public void UpdateAllConnections()
    {
        foreach (var node in parentNodes)
        {
            node.UpdateAllConnections();
        }
    }
}
