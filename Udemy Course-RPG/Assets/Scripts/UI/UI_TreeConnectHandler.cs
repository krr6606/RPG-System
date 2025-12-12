using System;
using UnityEngine;
using UnityEngine.UI;
[Serializable]
public class UI_TreeConnectDetails
{
    public UI_TreeConnectHandler childNode;
    public NodeDirectionType directionType;
    [Range(100f,350f)]  public float Length;
    [Range(-45f,45f)] public float AngleOffset;
}
public class UI_TreeConnectHandler : MonoBehaviour
{
    private RectTransform rect => GetComponent<RectTransform>();

    [SerializeField] private UI_TreeConnectDetails[] ConnectionDetails;
    [SerializeField] private UI_TreeConnection[] Connections;

    private Image connectionImage;
    private Color originalColor;

    private void Start()
    {
        if(connectionImage != null)
        {
            originalColor = connectionImage.color;
        }
            
    }

    private void OnValidate()
    {
        if(ConnectionDetails.Length <= 0 || Connections.Length <= 0) return;
        if (Connections.Length != ConnectionDetails.Length)
        {
            Debug.LogWarning("Connections and Details length mismatch in UI_TreeConnectHandler.");
            return;
        }
        UpdateConnections();
    }
    private void UpdateConnections()
    {
        if (Connections == null || ConnectionDetails == null) return;
        for (int i = 0; i < ConnectionDetails.Length; i++)
        {

                Connections[i].DirectConnection(ConnectionDetails[i].directionType, ConnectionDetails[i].Length, ConnectionDetails[i].AngleOffset);
                Vector2 targetPos = Connections[i].GetConnectionPoint(rect);
            if(ConnectionDetails[i].childNode == null) continue;

            ConnectionDetails[i].childNode.SetPosition(targetPos);
            ConnectionDetails[i].childNode.SetConnectionImage(Connections[i].GetConnectionImage());
            ConnectionDetails[i].childNode.transform.SetAsLastSibling();
        }
    }
    public void UpdateAllConnections()
    {
        UpdateConnections();

        foreach(var node in ConnectionDetails)
        {
            if(node.childNode == null) continue;
            node.childNode.UpdateConnections();
        }
    }
    public void ConnectionImageUnlockedColor(bool unlocked)
    {
        if(connectionImage == null) return;
        connectionImage.color = unlocked ? Color.white : originalColor;
    }
    public void SetConnectionImage(Image image) => connectionImage = image;
    public void SetPosition(Vector2 position) => rect.anchoredPosition = position;
}
