using UnityEngine;

public class Skill_SwordThrow : Skill_Base
{
    private SkillObject_PlayerSword currentSword;

    private float currentThrowForce;
    [Header("Sword Regular Settings")]
    [SerializeField] private GameObject swordPrefab;
    [Range(0f, 10f)]
    [SerializeField] private float throwForce = 6f;

    [Header("Sword Pierce Settings")]
    [SerializeField] private GameObject pierceSwordPrefab;
    public int pierceAmount = 3;
    [Range(0f, 10f)]
    [SerializeField] private float pierceThrowForce = 6f;

    [Header("Sword Spin Setting")]
    [SerializeField] private GameObject spinSwordPrefab;
    public int MaxDistance = 5;
    public float attackPerSecond = 6;
    [Range(0f, 10f)]
    [SerializeField] private float spinThrowForce = 6f;

    [Header("Sword Bounce Settings")]
    [SerializeField] private GameObject bounceSwordPrefab;
    public int bounceCount = 3;
    public float bounceSpeed = 15;
    [Range(0f, 10f)]
    [SerializeField] private float bounceThrowForce = 6f;

    [Header("Trajectory prediction")]
    [SerializeField] private GameObject predictionPrefab;
    [SerializeField] private int numberOfDots = 20;
    [SerializeField] private float spacingBetweenDots = 0.05f;
    private float swordGravity;
    private Transform[] dots;
    private Vector2 confirmDirection;

    private void Start()
    {
       dots = GneratePredictionDots();
        swordGravity = swordPrefab.GetComponent<Rigidbody2D>().gravityScale;
    }
    public void PredictTrajectory(Vector2 dir)
    {
        for (int i = 0; i < numberOfDots; i++)
        {
            dots[i].position = GetTrajectoryPoint(dir, i * spacingBetweenDots);

        }
    }
    public override bool canUseSkill()
    {
        UpdateThrowForce();
        if (currentSword != null)
        {
            currentSword.SwordComebackOn();
            return false;
        }
        return base.canUseSkill();
    }
    public void ThrowSword()
    {
        GameObject swordPrefab = GetSwordPrefab();
        GameObject newSword = Instantiate(swordPrefab, dots[1].position, Quaternion.identity);

        currentSword = newSword.GetComponent<SkillObject_PlayerSword>();
        currentSword.SetupSword(this, GetThrowPower());

        SetSkillOnCooldown();
    }

    private GameObject GetSwordPrefab()
    {

        switch(skillUpgradeType)
        {
            case SkillUpgradeType.SwordThrow:
                return swordPrefab;
            case SkillUpgradeType.SwordThrow_Spin:
                return spinSwordPrefab;
            case SkillUpgradeType.SwordThrow_Pierce:
                return pierceSwordPrefab;
            case SkillUpgradeType.SwordThrow_Bounce:
                return bounceSwordPrefab;
            default:
                return null;
        }

    }
    private Vector2 GetThrowPower() => confirmDirection * currentThrowForce * 10;
    private Vector2 GetTrajectoryPoint(Vector2 dir, float t)
    {
        float scaleTrowForce = currentThrowForce * 10;

        Vector2 initialVelocity = dir * scaleTrowForce;
        Vector2 gravityEffect = 0.5f * Physics2D.gravity * swordGravity * (t * t);

        Vector2 predictedPoint =(initialVelocity * t) + gravityEffect;

        Vector2 playerPosition = transform.root.position;

        return playerPosition + predictedPoint;
    }
    private void UpdateThrowForce()
    {
        switch(skillUpgradeType)
        {
            case SkillUpgradeType.SwordThrow:
                currentThrowForce = throwForce;
                break;
            case SkillUpgradeType.SwordThrow_Spin:
                currentThrowForce = spinThrowForce;
                break;
            case SkillUpgradeType.SwordThrow_Pierce:
                currentThrowForce = pierceThrowForce;
                break;
            case SkillUpgradeType.SwordThrow_Bounce:
                currentThrowForce = bounceThrowForce;
                break;
        }
    }
    public void ConfirmTrajectory(Vector2 dir) => confirmDirection = dir;

    public void EnableDots(bool enable)
    {

        foreach (Transform dot in dots)
        {
            dot.gameObject.SetActive(enable);
        }
    }
    private Transform[] GneratePredictionDots()
    {
        Transform[] dotArray = new Transform[numberOfDots];
        for (int i = 0; i < numberOfDots; i++)
        {
            dotArray[i] = Instantiate(predictionPrefab, transform).transform;
            dotArray[i].gameObject.SetActive(false);
        }
        return dotArray;
    }
}
