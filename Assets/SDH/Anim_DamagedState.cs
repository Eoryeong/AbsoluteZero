using UnityEngine;

public class Anim_DamagedState : AnimalState
{
    float damageTimer = 1f;
    public Anim_DamagedState(Animal animal) : base(animal)
    {
    }

    public override void EnterState()
    {
        base.EnterState();

        animal.agent.isStopped = true;

        animal.animator.SetTrigger("Damaged");

        damageTimer = 1f;
    }

    public override void UpdateState()
    {
        base.UpdateState();
        damageTimer -= Time.deltaTime;

        if (damageTimer < 0f)
        {
            animal.TakeActiontoTarget();
        }
    }

    public override void ExitState()
    {
        base.ExitState();
        animal.agent.isStopped = false;
    }

}
