using UnityEngine;

public class Anim_SleepState : AnimalState
{
    float sleepTimer;

    public Anim_SleepState(Animal animal) : base(animal)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        animal.agent.isStopped = true;
        animal.animator.SetTrigger("Sleep");
        sleepTimer = 30f;
    }

    public override void UpdateState()
    {
        base.UpdateState();
        sleepTimer -= Time.deltaTime;
        if (sleepTimer <= 0f)
        {
            if (Random.Range(0f, 1f) < animal.wanderProbability)
            {
                animal.ChangeState(animal.wanderState);
            }
            else
            {
                animal.ChangeState(animal.idleState);
            }
        }
    }

    public override void ExitState()
    {
        base.ExitState();
        animal.agent.isStopped = false;
    }
       
}
