using UnityEngine;
using UnityEngine.Rendering.UI;

public class Anim_IdleState : AnimalState
{
    public Anim_IdleState(Animal animal) : base(animal)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("Idle State Entered");
        animal.agent.ResetPath();
    }

    public override void UpdateState()
    {
        base.UpdateState();

        if (animal.distanceToTarget <= animal.detectionRange)
        {
            animal.ChangeState(animal.chaseState);
        }
        else
        {
            //추적 안할때
        }
    }

    public override void ExitState()
    {
        base.ExitState();
    }


}
