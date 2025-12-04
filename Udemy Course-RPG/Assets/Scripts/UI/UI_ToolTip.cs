using Unity.VisualScripting;
using UnityEngine;

public class UI_ToolTip : MonoBehaviour
{
    private RectTransform rectTransform;
    [SerializeField] private Vector3 offset;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    public virtual void  ShowToolTip(bool show,RectTransform targetRect )
    {
        if(show == false)
        {
            rectTransform.position = new Vector3(9000, 9000, 0);
            return;
        }
        UpdatePosition(targetRect);

    }
    private void UpdatePosition(RectTransform targetRect)
    {
        float screenCenterX = Screen.width / 2f;
        float screenTop = Screen.height;
        float screenBottom = 0f;
 
        Vector2 targetPosition = targetRect.position;

        targetPosition.x = targetPosition.x > screenCenterX
            ? targetPosition.x - offset.x
            : targetPosition.x + offset.x;
        float toolTipHalf = rectTransform.sizeDelta.y / 2f;
        float TopY = targetPosition.y + toolTipHalf;
        float BottomY = targetPosition.y - toolTipHalf;
        if (TopY > screenTop)
        {
            targetPosition.y = screenTop - toolTipHalf - offset.y;
        }
        else if (BottomY < screenBottom)
        {
            targetPosition.y = screenBottom + toolTipHalf + offset.y;
        }

        rectTransform.position = targetPosition;
    }
}
