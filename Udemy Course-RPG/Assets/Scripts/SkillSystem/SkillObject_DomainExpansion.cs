using UnityEngine;

public class SkillObject_DomainExpansion : SkillObject_Base
{
    private Skill_DomainExpansion skillDomainExpansion;


    private float expansionSpeed;
    
    private float duration;

    private float slowDownPercent;

    private Vector3 targetScale;

    private bool isShrinking;
    public void SetupDomain(Skill_DomainExpansion skill_DomainExpansion)
    {
        skillDomainExpansion = skill_DomainExpansion;

        duration = skillDomainExpansion.GetDomainDuration();

        slowDownPercent = skillDomainExpansion.GetDomainSlowPercent();

        float maxSize = skillDomainExpansion.maxDomainSize;
        targetScale = Vector3.one * maxSize;

        expansionSpeed = skillDomainExpansion.domainExpansionSpeed;
        Invoke(nameof(ShrinkDomain), duration);
    }
    private void Update()
    {
        HandleScaling();
    }
    private void HandleScaling()
    {
        float sizeDifference = Mathf.Abs(transform.localScale.x - targetScale.x);
        bool shouldChangeScale = sizeDifference > 0.1f;
        if (shouldChangeScale)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, expansionSpeed * Time.deltaTime);
        }
        if(isShrinking && sizeDifference <= 0.1f)
        {
            skillDomainExpansion.ClearTargets();
            Destroy(gameObject);
        }
    }
    private void ShrinkDomain()
    {
        targetScale = Vector3.zero;
        isShrinking = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if(enemy == null)
            return;

        skillDomainExpansion.AddTarget(enemy);
        enemy.SlowDownEntity(duration, slowDownPercent,true);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy == null)
            return;

        skillDomainExpansion.ClearTargets();
    }
}
