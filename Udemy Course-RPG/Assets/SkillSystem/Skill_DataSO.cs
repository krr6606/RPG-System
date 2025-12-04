using UnityEngine;
[CreateAssetMenu(fileName = "Skill Data", menuName = "ScriptableObjects/Skill Data - ", order = 1)]

public class Skill_DataSO : ScriptableObject
{
    public int cost;


    [Header("Skill Info")]
    public string Name;
    [TextArea]
    public string Description;
    public Sprite Icon;
}
