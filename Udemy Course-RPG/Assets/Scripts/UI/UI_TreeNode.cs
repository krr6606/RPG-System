using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class UI_TreeNode : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    private UI ui;
    private RectTransform rectTransform;

    [SerializeField] private Skill_DataSO skillData;
    [SerializeField] private string skillName;
    [SerializeField] private Image skillIcon;
    private string LockedColorHex = "#CBDFBF";
    private Color lastColor;
    public bool isUnlocked = false;
    public bool isLocked = false;
    
    void Awake()
    {
        ui = GetComponentInParent<UI>();
        rectTransform = GetComponent<RectTransform>();
    }
    void Start()
    {

      UpdateIconColor(GetColorByHex(LockedColorHex ));

    }
    private void Unlock()
    {
        isUnlocked = true;
        UpdateIconColor(Color.white);
    }
    private bool isCanBeUnlocked()
    {
        if (isUnlocked || isLocked)
            return false;
        return true;
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
         else 
         {
            Debug.Log("Tree node cannot be unlocked: " + gameObject.name);
         }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.toolTip.ShowToolTip(true, rectTransform,skillData);
        if (!isUnlocked)
        {
            UpdateIconColor(Color.white * 0.9f);
            Debug.Log("Show tooltip for tree node: " + gameObject.name);

        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.toolTip.ShowToolTip(false, rectTransform);
        if (!isUnlocked)
        {
            UpdateIconColor(lastColor);
            Debug.Log("Hide tooltip for tree node: " + gameObject.name);

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
    private void OnValidate()
    {
        if (skillData == null || skillIcon == null)
            return;
        skillName = skillData.Name;
        skillIcon.sprite = skillData.Icon;
        gameObject.name = "TreeNode - " + skillName;
    }

}
