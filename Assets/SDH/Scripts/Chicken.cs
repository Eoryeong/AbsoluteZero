using UnityEngine;
using static Animal;

public class Chicken : Animal
{
    protected override void InitializeStatus()
    {
        maxHP = 120f;
        speed = 6f;
        wanderSpeed = 3f;
        fleeSpeed = 8f;
        detectionRange = 20f;
        wanderRadius = 20f;
        wanderTimeMin = 5f;
        wanderTimeMax = 7f;
        idleTimeMin = 5f;
        idleTimeMax = 7f;
        wanderProbability = 0.7f;
        idleProbability = 0.4f;
        fleeTime = 5f;
        animalType = AnimalType.Prey;

        base.InitializeStatus();
    }


}
