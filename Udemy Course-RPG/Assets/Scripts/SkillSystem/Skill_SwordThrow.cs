using UnityEngine;

public class Skill_SwordThrow : Skill_Base
{
    private SkillObject_PlayerSword currentSword;

    [Header("Sword Regular Settings")]
    [Range(0f, 20f)]
    [SerializeField] private float throwForce = 6f;
    [SerializeField] private GameObject swordPrefab;
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
        if(currentSword != null)
        {
            currentSword.SwordComebackOn();
            return false;
        }
        return base.canUseSkill();
    }
    public void ThrowSword()
    {
        GameObject newSword = Instantiate(swordPrefab, dots[1].position, Quaternion.identity);

        currentSword = newSword.GetComponent<SkillObject_PlayerSword>();
        currentSword.SetupSword(this, GetThrowPower());
    }
    private Vector2 GetThrowPower() => confirmDirection * throwForce * 10;
    private Vector2 GetTrajectoryPoint(Vector2 dir, float t)
    {
        float scaleTrowForce = throwForce * 10;

        Vector2 initialVelocity = dir * scaleTrowForce;
        Vector2 gravityEffect = 0.5f * Physics2D.gravity * swordGravity * (t * t);

        Vector2 predictedPoint =(initialVelocity * t) + gravityEffect;

        Vector2 playerPosition = transform.root.position;

        return playerPosition + predictedPoint;
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
