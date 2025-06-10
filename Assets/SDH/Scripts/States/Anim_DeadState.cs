using UnityEngine;

public class Anim_DeadState : AnimalState
{
    public Anim_DeadState(Animal animal) : base(animal)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        animal.OnDeadEnter();

        animal.isDead = true;
        animal.agent.isStopped = true;
        animal.animator.SetTrigger("Dead");
    }

    public override void UpdateState()
    {
        base.UpdateState();
        animal.OnDeadUpdate();
    }

    public override void ExitState()
    {
        base.ExitState();
        animal.OnDeadExit();
    }
    
}
