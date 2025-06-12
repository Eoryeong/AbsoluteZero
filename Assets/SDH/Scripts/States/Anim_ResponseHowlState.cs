using UnityEngine;

public class Anim_ResponseHowlState : AnimalState
{

    float responseTimer;

    public Anim_ResponseHowlState(Animal animal) : base(animal)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        animal.animator.SetBool("isRun", true);
        animal.agent.isStopped = false;
        responseTimer = 6f;
    }

    public override void UpdateState()
    {
        base.UpdateState();
        responseTimer -= Time.deltaTime;
        animal.agent.SetDestination(animal.target.position);
        if(animal.distanceToTarget <= animal.attackRange)
        {
            animal.ChangeState(animal.attackState);
            return;
        }

        if (responseTimer <= 0f)
        {
            animal.ChangeState(animal.chaseState);
        }
    }

    public override void ExitState()
    {
        base.ExitState();
        animal.animator.SetBool("isRun", false);
    }

   
}
