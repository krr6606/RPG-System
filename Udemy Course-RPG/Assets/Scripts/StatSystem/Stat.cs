using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class Stat 
{
    [SerializeField] private float baseValue;
    [SerializeField] private List<StatModifier> statModifiers = new List<StatModifier>();
    private float finalValue;
    private bool isDirty = true;
    public float GetValue()
    {
        if (isDirty)
        {
            finalValue = GetFinalValue();
            isDirty = false;
        }
        return finalValue;
    }
    public void AddModifier(StatModifier modifier)
    {
        statModifiers.Add(modifier);
        isDirty = true;
    }
    public void AddModifier(float value, string sourceID)
    {
        StatModifier modifier = new StatModifier(value, sourceID);
        statModifiers.Add(modifier);
        isDirty = true;

    }
    public void RemoveModifierBySource(string sourceID)
    {
        statModifiers.RemoveAll(modifier => modifier.SourceID == sourceID);
        isDirty = true;
    } 
    public float GetFinalValue()
    {
        float totalModifier = 0f;
        foreach (StatModifier modifier in statModifiers)
        {
            totalModifier += modifier.Value;
        }
        finalValue = baseValue + totalModifier;
        return finalValue;
    }
    public void SetBaseValue(float newValue)
    {
        baseValue = newValue;
        isDirty = true;
    }
}
    [Serializable]
    public class StatModifier
    {
        public float Value;
        public string SourceID;
        
        public StatModifier(float value, string sourceID)
        {
            Value = value;
            SourceID = sourceID;
        }
    }
