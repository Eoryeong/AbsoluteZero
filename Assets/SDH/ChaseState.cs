using UnityEngine;

public class ChaseState : AnimalState
{
    public ChaseState(Animal animal) : base(animal)
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
        anim.agent.SetDestination(anim.target.position);
        if(anim.distanceToTarget <= anim.attackRange)
        {
            // 공격 로직 추가
        }
        else if (anim.distanceToTarget > anim.detectionRange)
        {
            anim.ChangeState(anim.idleState);
        }
    }

    public override void ExitState()
    {
        base.ExitState();
    }

}
