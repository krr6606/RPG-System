using UnityEngine;

public class UI_TreeConnection : MonoBehaviour
{
    [SerializeField] private RectTransform rotationPoint;
    [SerializeField] private RectTransform connectionLength;
    [SerializeField] private RectTransform childNodePoint;

    public void DirectConnection(NodeDirectionType directionType, float length)
    {
        float angle = GetAngleFromDirection(directionType);
        rotationPoint.localRotation = Quaternion.Euler(0f, 0f, angle);


        bool shouldBeActive = directionType != NodeDirectionType.None;
        float finalLength = shouldBeActive ? length : 0f;
        connectionLength.sizeDelta = new Vector2(finalLength, connectionLength.sizeDelta.y);
    }
    private float GetAngleFromDirection(NodeDirectionType directionType)
    {
        return directionType switch
        {
            NodeDirectionType.UpLeft => 135f,
            NodeDirectionType.Up => 90f,
            NodeDirectionType.UpRight => 45f,
            NodeDirectionType.Right => 0f,
            NodeDirectionType.DownRight => 315f,
            NodeDirectionType.Down => 270f,
            NodeDirectionType.DownLeft => 225f,
            NodeDirectionType.Left => 180f,
            _ => 0f,
        };
    }
    public Vector2 GetConnectionPoint(RectTransform rectTransform)
{
    RectTransformUtility.ScreenPointToLocalPointInRectangle(
       rectTransform.parent as RectTransform,
       childNodePoint.position,
       null,
       out var localPoint);
    return localPoint;
}
}

public enum NodeDirectionType
{
    None,
    UpLeft,
    Up,
    UpRight,
    Right,
    DownRight,
    Down,
    DownLeft,
    Left
}