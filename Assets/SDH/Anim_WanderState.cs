using UnityEngine;
using UnityEngine.AI;

public class Anim_WanderState : AnimalState
{
    Vector3 wanderDestination;
    float wanderTime;


    public Anim_WanderState(Animal animal) : base(animal)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        animal.OnWanderEnter();

        animal.animator.SetBool("isWalk", true); 
        wanderTime = Random.Range(animal.wanderTimeMin, animal.wanderTimeMax);
        SetRandomDestination();
        animal.agent.speed = animal.wanderSpeed;   
    }

    public override void UpdateState()
    {
        base.UpdateState();
        animal.OnWanderUpdate();


        if (animal.distanceToTarget <= animal.detectionRange)
        {
            animal.TakeActiontoTarget();
            return;
        }

        if (!animal.agent.pathPending && (!animal.agent.hasPath || animal.agent.remainingDistance < 0.5f))
        {
            SetRandomDestination();
        }

        wanderTime -= Time.deltaTime;
        if (wanderTime <= 0f)
        {
            if (Random.Range(0f, 1f) < animal.idleProbability)
            {
                animal.ChangeState(animal.idleState);
            }
            else
            {
                wanderTime = Random.Range(animal.wanderTimeMin, animal.wanderTimeMax);
            }
        }
    }

    public override void ExitState()
    {
        base.ExitState();
        animal.OnWanderExit();

        animal.animator.SetBool("isWalk", false);
        animal.agent.speed = animal.speed;
    }

    private void SetRandomDestination()
    {
        wanderDestination = Random.onUnitSphere * animal.wanderRadius;
        ConfirmDestination();
    }

    private void ConfirmDestination()
    {
        if (wanderDestination.y >= 5f)
        {
            SetRandomDestination();
        }
        else
        {
            wanderDestination += animal.transform.position;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(wanderDestination, out hit, 5f, NavMesh.AllAreas))
            {
                wanderDestination = hit.position;
                animal.agent.SetDestination(wanderDestination);
                animal.agent.isStopped = false;
            }
            else
            {
                SetRandomDestination();
            }

        }
    }
}
