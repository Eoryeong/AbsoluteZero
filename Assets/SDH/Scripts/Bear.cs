using UnityEngine;

public class Bear : Animal
{

    float sleepProbability = 0.2f;
    float sleepChangeTimer = 10f;
    public Anim_SleepState sleepState;
    public float angerGauge = 0f;

    protected override void InitializeStatus()
    {
        maxHP = 200f;
        speed = 6f;
        wanderSpeed = 3f;
        fleeSpeed = 8f;
        detectionRange = 15f;
        attackRange = 3f;
        attackDamage = 5f;
        attackCooldown = 3f;
        wanderRadius = 20f;
        wanderTimeMin = 10f;
        wanderTimeMax = 15f;
        idleTimeMin = 10;
        idleTimeMax = 15f;
        wanderProbability = 0.3f;
        idleProbability = 0.6f;
        fleeTime = 6f;
        animalType = AnimalType.Predator;

        base.InitializeStatus();
    }

    protected override void InitializeStates()
    {
        sleepState = new Anim_SleepState(this);
        base.InitializeStates();
    }

    protected override AnimalState GetInitState()
    {
        return Random.Range(0f, 1f) < wanderProbability ? wanderState : idleState;
    }


    public override void Attack()
    {
        if(angerGauge >= 100f)
        {
            animator.SetTrigger("StrongAtt");
            angerGauge = 0f;
        }
        else
        {
            if(Random.Range(0f, 1f) < 0.5f)
            {
                animator.SetTrigger("Att");
            }
            else
            {
                animator.SetTrigger("Att2");
            }
            angerGauge += 10f;
        }
    }

    void Trysleep()
    {
        if (Random.Range(0f, 1f) < sleepProbability)
        {
            ChangeState(sleepState);
        }
    }

    public override void OnIdleUpdate()
    {
        sleepChangeTimer -= Time.deltaTime;
        if (sleepChangeTimer <= 0f)
        {
            Trysleep();
            sleepChangeTimer = 10f;
        }

    }

    public override void OnChaseUpdate()
    {
        angerGauge += Time.deltaTime * 5f;
    }

    public override void OnAttackExit()
    {
        animator.ResetTrigger("Att");
        animator.ResetTrigger("Att2");
        animator.ResetTrigger("StrongAtt");
    }
}
