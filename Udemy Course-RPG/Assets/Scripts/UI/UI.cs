using UnityEngine;

public class UI : MonoBehaviour
{
    public UI_SkillToolTip toolTip;
    public UI_SkillTree skillTreeUI;
    private bool skillTreeEnabled = false;
    private void Awake()
    {
        if (toolTip == null)
        {
            toolTip = GetComponentInChildren<UI_SkillToolTip>();
        }
        if (skillTreeUI == null)
        {
            skillTreeUI = GetComponentInChildren<UI_SkillTree>(true);
        }
    }
    public void ToggleSkillTreeUI()
    {
        skillTreeEnabled = !skillTreeEnabled;
        skillTreeUI.gameObject.SetActive(skillTreeEnabled);
        toolTip.ShowToolTip(false, null);
    }
}
