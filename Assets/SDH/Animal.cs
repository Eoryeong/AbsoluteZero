using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.AI;

public class Animal : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform target;
    public float speed = 3.5f;
    public float detectionRange = 10f;
    public float attackRange = 2f;
    public ChaseState chaseState;
    public IdleState idleState;
    public float distanceToTarget;

    AnimalState currentState;

    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
        target = GameObject.FindGameObjectWithTag("Player")?.transform;


        chaseState = new ChaseState(this);
        idleState = new IdleState(this);

    }
    protected virtual void Start()
    {
        ChangeState(idleState);

    }
    protected virtual void Update()
    {
        currentState?.UpdateState();
        if (target != null)
        {
            distanceToTarget = Vector3.Distance(transform.position, target.position);
        }
    }

    public void ChangeState(AnimalState newState)
    {
        if (currentState != null)
            currentState.ExitState();

        currentState = newState;
        currentState.EnterState();
    }

}
