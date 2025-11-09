using UnityEngine;

public class Player_Combat : Entity_Combat
{
    [Header("Counter Attack Settings")]
    [SerializeField]private float counterRecovery = 0.25f;
    public bool CounterAttackPerformed()
    {
        bool countered = false;
        foreach (var target in GetDetectedTargets())
        {
            ICounterable counterable = target.GetComponent<ICounterable>();
            if (counterable ==null) continue;
            if (counterable.CanBeCountered)
            {
                counterable.HandleCounter();
                countered = true;
            }
        }
        return countered;
    }
    public float GetCounterRecoveryDuration() => counterRecovery;
}
