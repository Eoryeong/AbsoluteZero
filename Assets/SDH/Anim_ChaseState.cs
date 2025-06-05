using UnityEngine;

public class Anim_ChaseState : AnimalState
{
    public Anim_ChaseState(Animal animal) : base(animal)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("Chase State Entered");
        animal.animator.SetBool("isRun", true);
    }

    public override void UpdateState()
    {
        base.UpdateState();
        animal.agent.SetDestination(animal.target.position);
        if (animal.distanceToTarget <= animal.attackRange)
        {
            animal.ChangeState(animal.attackState);
        }
        else if (animal.distanceToTarget > animal.detectionRange)
        {
            if(Random.Range(0f, 1f) < animal.idleProbability)
            {
                animal.ChangeState(animal.idleState);
            }
            else
            {
                animal.ChangeState(animal.wanderState);
            }
        }
    }

    public override void ExitState()
    {
        base.ExitState();
        animal.animator.SetBool("isRun", false);
    }

}
