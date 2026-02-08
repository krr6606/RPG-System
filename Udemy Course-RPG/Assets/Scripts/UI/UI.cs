using UnityEngine;

public class UI : MonoBehaviour
{
    public UI_SkillToolTip toolTip{ get; private set; }
    public UI_ItemToolTip itemToolTip { get; private set; }
    public UI_StatToolTip statToolTip { get; private set; }

    public UI_SkillTree skillTreeUI { get; private set; }
    public UI_Inventory inventoryUI { get; private set; }
    private bool skillTreeEnabled = false;
    private bool inventoryEnabled = false;
    private void Awake()
    {
        if (toolTip == null)
        {
            toolTip = GetComponentInChildren<UI_SkillToolTip>();
        }
        if (itemToolTip == null)
        {
            itemToolTip = GetComponentInChildren<UI_ItemToolTip>();
        }
        if (statToolTip == null)
        {
            statToolTip = GetComponentInChildren<UI_StatToolTip>();
        }
        if (skillTreeUI == null)
        {
            skillTreeUI = GetComponentInChildren<UI_SkillTree>(true);
        }
        if (inventoryUI == null)
        {
            inventoryUI = GetComponentInChildren<UI_Inventory>(true);
        }

        skillTreeEnabled = skillTreeUI.gameObject.activeSelf;
        inventoryEnabled = inventoryUI.gameObject.activeSelf;
    }
    public void ToggleSkillTreeUI()
    {
        skillTreeEnabled = !skillTreeEnabled;
        skillTreeUI.gameObject.SetActive(skillTreeEnabled);
        toolTip.ShowToolTip(false, null);

    }
    public void ToggleStatusUI()
    {
        inventoryEnabled = !inventoryEnabled;
        inventoryUI.gameObject.SetActive(inventoryEnabled);
        statToolTip.ShowToolTip(false, null);
        itemToolTip.ShowToolTip(false, null);
    }
}
