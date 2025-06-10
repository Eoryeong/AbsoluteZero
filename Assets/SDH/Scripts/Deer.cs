using UnityEngine;

public class Deer : Animal
{

    protected override void InitializeStatus()
    {
        maxHP = 120f;
        speed = 6f;
        wanderSpeed = 3f;
        fleeSpeed = 8f;
        detectionRange = 15f;
        attackRange = 2.5f;
        attackDamage = 5f;
        attackCooldown = 2.5f;
        wanderRadius = 20f;
        wanderTimeMin = 5f;
        wanderTimeMax = 7f;
        idleTimeMin = 8f;
        idleTimeMax = 10f;
        wanderProbability = 0.7f;
        idleProbability = 0.4f;
        fleeTime = 6f;
        animalType = AnimalType.Prey;

        base.InitializeStatus();
    }
}
