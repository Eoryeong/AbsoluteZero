using UnityEngine;

public class Anim_AttackState : AnimalState
{
    public Anim_AttackState(Animal animal) : base(animal)
    {
    }

    float attackCooldownTimer;

    public override void EnterState()
    {
        base.EnterState();
        animal.OnAttackEnter();

        Debug.Log("Attack State Entered");
        animal.agent.isStopped = true;
        attackCooldownTimer = animal.attackCooldown;

        animal.Attack();
    }

    public override void UpdateState()
    {
        base.UpdateState();
        animal.OnAttackUpdate();

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
            attackCooldownTimer = animal.attackCooldown;
        }
        
    }
    public override void ExitState()
    {
        base.ExitState();
        animal.OnAttackExit();

        animal.agent.isStopped = false;
    }

}
