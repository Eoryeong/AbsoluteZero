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
            animal.ChangeState(animal.idleState);
        }
    }

    public override void ExitState()
    {
        base.ExitState();
    }

}
