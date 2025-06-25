using UnityEngine;

public class Anim_DeadState : AnimalState
{
    Vector3 ps;
    public Anim_DeadState(Animal animal) : base(animal)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        animal.OnDeadEnter();
        ps = animal.transform.position;
        animal.isDead = true;
        animal.agent.isStopped = true;
        animal.animator.SetTrigger("Dead");
        animal.col.isTrigger = false;
        animal.agent.enabled = false;
        animal.animalCorpse.enabled = true;
        animal.interactObject.enabled = true;
        animal.ChangeLayer();
    }

    public override void UpdateState()
    {
        base.UpdateState();
        animal.OnDeadUpdate();
        animal.transform.position = ps;
    }

    public override void ExitState()
    {
        base.ExitState();
        animal.OnDeadExit();
    }
    
}
