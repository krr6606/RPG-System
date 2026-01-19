using UnityEngine;

public class Player_DomainExpansionState : PlayerState
{
    private Vector2 originalPosition;
    private float originalGravity;
    private float finalRiseDistance;

    private bool isLevitating;
    private bool createDomain;
    private bool riseEnded;
    public Player_DomainExpansionState(Player player, StateMachin stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        originalPosition = player.transform.position;
        originalGravity = rb.gravityScale;
        rb.gravityScale = 0;
        finalRiseDistance = GetAvalibaleRiseDistance();
        Debug.Log("Final Rise Distance: " + finalRiseDistance);
        player.SetVelocity(0, player.riseSpeed);
        isLevitating = false;
        riseEnded = false;
    }
    public override void Update()
    {
        base.Update();

        if(Vector2.Distance(originalPosition, player.transform.position) >= finalRiseDistance && isLevitating == false)
        {
            Livitate();
            Debug.Log("Levitate");
        }
        if (isLevitating)
        {
            skillManager.Skill_DomainExpansion.DoSpellCasting();
            if (stateTimer < 0)
            {
                rb.gravityScale = originalGravity;
                isLevitating = false;
                riseEnded = true;
                stateMachine.ChangeState(player.idleState);
            }
        }
        if( riseEnded == false && isLevitating == false)
        {
            player.SetVelocity(0, player.riseSpeed);
        }
    }
    public override void Exit()
    {
        base.Exit();

        createDomain = false;
    }
    private void Livitate()
    {
        isLevitating = true;
        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 0;

        stateTimer = skillManager.Skill_DomainExpansion.GetDomainDuration();

        if (createDomain == false)
        {
            createDomain = true;
            skillManager.Skill_DomainExpansion.CreateDomain();

        }
    }
    private float GetAvalibaleRiseDistance()
    {
        float maxRiseHeight = player.riseMaxDistance;
        RaycastHit2D hit = Physics2D.Raycast(originalPosition, Vector2.up, maxRiseHeight + 1,player.groundLayer);
        if (hit.collider != null)
        {
            return hit.distance - 1.5f;
        }
        return maxRiseHeight;
    }
}
