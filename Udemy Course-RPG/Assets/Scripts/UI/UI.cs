using UnityEngine;

public class UI : MonoBehaviour
{
    public UI_SkillToolTip toolTip;

    private void Awake()
    {
        if (toolTip == null)
        {
            toolTip = GetComponentInChildren<UI_SkillToolTip>();
        }
    }
}
