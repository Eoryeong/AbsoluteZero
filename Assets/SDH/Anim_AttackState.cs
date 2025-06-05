using UnityEngine;

public class Anim_AttackState : AnimalState
{
    public Anim_AttackState(Animal animal) : base(animal)
    {
    }

    float attackCooldownTimer = 0f;

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("Attack State Entered");
        animal.agent.isStopped = true;
        animal.animator.SetTrigger("Att");
        attackCooldownTimer = animal.attackCooldown;
    }

    public override void UpdateState()
    {
        base.UpdateState();
        //공격이 끝났을때
        attackCooldownTimer -= Time.deltaTime;
        if(attackCooldownTimer <= 0f)
        {
            if (animal.distanceToTarget > animal.attackRange)
            {
                animal.ChangeState(animal.chaseState);
            }
            if (animal.distanceToTarget <= animal.attackRange)
            {
                animal.ChangeState(animal.attackState);
            }
        }
        
    }
    public override void ExitState()
    {
        base.ExitState();
        animal.agent.isStopped = false;
    }

}
