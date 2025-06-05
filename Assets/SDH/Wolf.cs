using UnityEngine;

public class Wolf : Animal
{


    protected override void Awake()
    {
        base.Awake();
    }

    protected override void InitializeStatus()
    {
        maxHP = 120f;
        speed = 6f;
        wanderSpeed = 3f;
        fleeSpeed = 8f;
        detectionRange = 15f;
        attackRange = 2.5f;
        attackDamage = 5f;
        attackCooldown = 2f;
        wanderRadius = 20f;
        wanderTimeMin = 5f;
        wanderTimeMax = 10f;
        wanderProbability = 0.7f;
        idleProbability = 0.4f;
        fleeTime = 6f;
        animalType = AnimalType.Predator;

        base.InitializeStatus();
    }

    protected override AnimalState GetInitState()
    {
        return Random.Range(0f, 1f) < 0.7f ? wanderState : idleState;
    }


}
