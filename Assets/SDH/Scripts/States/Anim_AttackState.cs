using UnityEngine;

public class Anim_AttackState : AnimalState
{
    float attackCooldownTimer;
    bool isRotate = false;


    public Anim_AttackState(Animal animal) : base(animal)
    {
    }


    public override void EnterState()
    {
        base.EnterState();
        animal.OnAttackEnter();

        Debug.Log("Attack State Entered");
        animal.agent.isStopped = true;
        animal.agent.velocity = Vector3.zero;
        attackCooldownTimer = 0f;

        animal.Attack();
    }

    public override void UpdateState()
    {
        base.UpdateState();
        animal.OnAttackUpdate();

        //공격이 끝났을때
        attackCooldownTimer += Time.deltaTime;
        if (attackCooldownTimer >= 1.5f)
        {
            RotateTowardsTarget();
        }
        if (attackCooldownTimer >= 2.0f)
        {
            isRotate = false;
            animal.animator.SetBool("isRotate", false);
            if (animal.distanceToTarget > animal.attackRange)
            {
                animal.ChangeState(animal.chaseState);
            }
            if (animal.distanceToTarget <= animal.attackRange)
            {
                animal.ChangeState(animal.attackState);
            }
            attackCooldownTimer = 0;
        }

    }
    public override void ExitState()
    {
        base.ExitState();
        animal.OnAttackExit();

        animal.agent.isStopped = false;
    }


    private void RotateTowardsTarget()
    {
        Vector3 direction = animal.target.position - animal.transform.position;
        direction.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        float angleDifference = Quaternion.Angle(animal.transform.rotation, targetRotation);
        if (angleDifference > 1f)
        {
            if (!isRotate)
            {
                isRotate = true;
                animal.animator.SetBool("isRotate", true);
            }
            animal.transform.rotation = Quaternion.Slerp(animal.transform.rotation, targetRotation, 5*Time.deltaTime);
        }
        else
        {
            if (isRotate)
            {
                isRotate = false;
                animal.animator.SetBool("isRotate", false);
            }
        }

    }
}
