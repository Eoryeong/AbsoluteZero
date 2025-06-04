using UnityEngine;
using UnityEngine.Rendering.UI;

public class IdleState : AnimalState
{
    public IdleState(Animal animal) : base(animal)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        anim.agent.isStopped = true; // 정지 상태로 설정
        Debug.Log("Idle State Entered");
    }

    public override void UpdateState()
    {
        base.UpdateState();
                
        if(anim.distanceToTarget <= anim.detectionRange)
        {
            anim.ChangeState(anim.chaseState);
        }
        else
        {
            //추적 안할때
        }
    }

    public override void ExitState()
    {
        base.ExitState();
        anim.agent.isStopped = false; // 이동 가능 상태로 설정
    }


}
