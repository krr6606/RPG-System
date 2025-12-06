using System;
using UnityEngine;
[Serializable]
public class UI_TreeConnectDetails
{
    public UI_TreeConnectHandler childNode;
    public NodeDirectionType directionType;
  [Range(100f,350f)]  public float Length;
}
public class UI_TreeConnectHandler : MonoBehaviour
{
    private RectTransform rect => GetComponent<RectTransform>();

    [SerializeField] private UI_TreeConnectDetails[] ConnectionDetails;
    [SerializeField] private UI_TreeConnection[] Connections;

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

                Connections[i].DirectConnection(ConnectionDetails[i].directionType, ConnectionDetails[i].Length);
                Vector2 targetPos = Connections[i].GetConnectionPoint(rect);
                ConnectionDetails[i].childNode?.SetPosition(targetPos);
        }
    }
    public void SetPosition(Vector2 position) => rect.anchoredPosition = position;
}
