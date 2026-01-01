using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class UI_TreeNode : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    private UI ui;
    private RectTransform rectTransform;
    private UI_SkillTree skillTree;
    private UI_TreeConnectHandler connectionHandler;


    [Header("Unlock Details")]
    public UI_TreeNode[] neededNodes;
    public UI_TreeNode[] conflictNodes;
    public bool isUnlocked = false;
    public bool isLocked = false;

    public Skill_DataSO skillData;
    [SerializeField] private string skillName;
    [SerializeField] private Image skillIcon;
    [SerializeField] private int skillCost;
    [SerializeField] private string LockedColorHex = "#CBDFBF";
    private Color lastColor;

    void Awake()
    {
        ui = GetComponentInParent<UI>();
        rectTransform = GetComponent<RectTransform>();
        skillTree = GetComponentInParent<UI_SkillTree>();
        connectionHandler = GetComponent<UI_TreeConnectHandler>();
    }
    void Start()
    {

        UpdateIconColor(GetColorByHex(LockedColorHex));
        if(skillData.unlockedByDefault)
            Unlock();


    }
    public void Refund()
    {
        isUnlocked = false;
        isLocked = false;
        skillTree.AddSkillPoint(skillData.cost);
        connectionHandler.ConnectionImageUnlockedColor(false);
        UpdateIconColor(GetColorByHex(LockedColorHex));
    }
    private void Unlock()
    {
        isUnlocked = true;
        skillTree.RemoveSkillPoint(skillData.cost);
        LockConflictNodes();
        connectionHandler.ConnectionImageUnlockedColor(true);
        UpdateIconColor(Color.white);

        skillTree.playerSkillManager.GetSkillByType(skillData.skillType).SetSkillUpgrade(skillData.upgradeData);
    }
    private bool isCanBeUnlocked()
    {
        if (isUnlocked || isLocked)
            return false;
        if (!skillTree.HasEnoughPoints(skillData.cost))
            return false;

        foreach (var node in neededNodes)
        {
            if (!node.isUnlocked)
                return false;
        }
        foreach (var node in conflictNodes)
        {
            if (node.isUnlocked)
                return false;
        }

        return true;
    }
    private void LockConflictNodes()
    {
        foreach (var node in conflictNodes)
        {
            node.isLocked = true;
            node.LockChildNodes();
        }
    }
    public void LockChildNodes()
    {
        isLocked = true;
        foreach (var node in connectionHandler.GetChildNodes())
            node.LockChildNodes();

    }

    private void UpdateIconColor(Color color)
    {
        if (skillIcon == null)
        {
            return;
        }
        lastColor = skillIcon.color;
        skillIcon.color = color;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Tree node clicked: " + gameObject.name);
        if (isCanBeUnlocked())
        {
            Unlock();
        }
        else if (isLocked)
        {
            ui.toolTip.LockedSkillEffect();
        }
        else
        {
            Debug.Log("Tree node cannot be unlocked: " + gameObject.name);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.toolTip.ShowToolTip(true, rectTransform, this);
        if (isUnlocked || isLocked)
            return;

        ToggleNodeHighlight(true);


    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.toolTip.ShowToolTip(false, rectTransform);
        if (isUnlocked || isLocked)
            return;

        ToggleNodeHighlight(false);


    }
    private void ToggleNodeHighlight(bool highlight)
    {
        if (highlight)
        {
            Color newColor = Color.white * 0.9f;
            newColor.a = 1f;
            UpdateIconColor(newColor);
        }
        else
        {
            UpdateIconColor(lastColor);
        }
    }
    private Color GetColorByHex(string hex)
    {
        Color color;
        if (ColorUtility.TryParseHtmlString(hex, out color))
        {
            return color;
        }
        return Color.white;
    }
    private void OnDisable()
    {
        if(isLocked)
            UpdateIconColor(GetColorByHex(LockedColorHex));

        if(isUnlocked)
            UpdateIconColor(Color.white);
    }
    private void OnValidate()
    {
        if (skillData == null || skillIcon == null)
            return;
        skillName = skillData.Name;
        skillIcon.sprite = skillData.Icon;
        skillCost = skillData.cost;
        gameObject.name = "TreeNode - " + skillName;
    }

}
