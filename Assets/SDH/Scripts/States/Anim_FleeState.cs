using UnityEngine;
using UnityEngine.AI;

public class Anim_FleeState : AnimalState
{
    Vector3 fleeDestination;
    Vector3 fleeDir;
    Vector3 randomOffset;
    float fleeTimer;


    public Anim_FleeState(Animal animal) : base(animal)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        animal.OnFleeEnter();

        Debug.Log("Flee State Entered");
        fleeTimer = animal.fleeTime;
        SetFleeDestination();
        animal.agent.speed = animal.fleeSpeed;
        animal.animator.SetBool("isRun", true);
        animal.agent.isStopped = false;
    }

    public override void UpdateState()
    {
        base.UpdateState();
        animal.OnFleeUpdate();


        if (!animal.agent.pathPending && (!animal.agent.hasPath || animal.agent.remainingDistance < 0.5f))
        {
            SetFleeDestination();
        }

        fleeTimer -= Time.deltaTime;

        if (fleeTimer <= 0f || animal.distanceToTarget > animal.detectionRange)
        {
            if (Random.Range(0f, 1f) < animal.idleProbability)
            {
                animal.ChangeState(animal.idleState);
            }
            else
            {
                animal.ChangeState(animal.wanderState);
            }
        }
    }

    public override void ExitState()
    {
        base.ExitState();
        animal.OnFleeExit();

        animal.animator.SetBool("isRun", false);
        animal.agent.speed = animal.speed;
    }

    private void SetFleeDestination()
    {
        fleeDir = (animal.transform.position - animal.target.position).normalized;
        randomOffset = Random.onUnitSphere * (animal.wanderRadius * 0.5f);
        fleeDestination = fleeDir * animal.wanderRadius + randomOffset;
        ConfirmDestination();
    }

    private void ConfirmDestination()
    {
        if (fleeDestination.y >= 5f)
        {
            SetFleeDestination();
        }
        else
        {
            fleeDestination += animal.transform.position;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(fleeDestination, out hit, 5f, NavMesh.AllAreas))
            {
                fleeDestination = hit.position;
                animal.agent.SetDestination(fleeDestination);
                Debug.Log("목표설정완료");
                animal.agent.isStopped = false;
            }
            else
            {
                SetFleeDestination();
            }

        }
    }
}
