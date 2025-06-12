using UnityEngine;

public class Goat : Animal
{
    protected override void InitializeStatus()
    {
        maxHP = 200f;
        speed = 6f;
        wanderSpeed = 3f;
        fleeSpeed = 8f;
        detectionRange = 15f;
        wanderRadius = 20f;
        wanderTimeMin = 8f;
        wanderTimeMax = 10f;
        idleTimeMin = 10;
        idleTimeMax = 15f;
        wanderProbability = 0.4f;
        idleProbability = 0.5f;
        fleeTime = 5f;
        animalType = AnimalType.Prey;

        base.InitializeStatus();
    }
}
