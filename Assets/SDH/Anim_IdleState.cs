using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.UI;

public class Anim_IdleState : AnimalState
{
    float idleTime;

    public Anim_IdleState(Animal animal) : base(animal)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("Idle State Entered");
        animal.agent.ResetPath();

        idleTime = Random.Range(animal.idleTimeMin, animal.idleTimeMax);
    }

    public override void UpdateState()
    {
        base.UpdateState();

        if (animal.distanceToTarget <= animal.detectionRange)
        {
            animal.TakeActiontoTarget();
            return;
        }
        idleTime -= Time.deltaTime;
        if (idleTime <= 0f)
        {
            if (Random.Range(0f, 1f) < animal.wanderProbability)
            {
                animal.ChangeState(animal.wanderState);
            }
            else
            {
                idleTime = Random.Range(animal.idleTimeMin, animal.idleTimeMax);
            }
        }
    }

    public override void ExitState()
    {
        base.ExitState();
    }


}
