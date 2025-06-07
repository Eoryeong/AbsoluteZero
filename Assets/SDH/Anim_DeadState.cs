using UnityEngine;

public class Anim_DeadState : AnimalState
{
    public Anim_DeadState(Animal animal) : base(animal)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        animal.isDead = true;
        animal.agent.isStopped = true;
        animal.animator.SetTrigger("Dead");
    }

    public override void UpdateState()
    {
        base.UpdateState();
    }

    public override void ExitState()
    {
        base.ExitState();
    }
    
}
