using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.AI;

public class Animal : MonoBehaviour
{
    public enum AnimalType
    {
        Predator,
        Prey
    }
    // 기본 스탯
    public float HP = 100f;
    public float maxHP = 100f;
    public float speed = 3.5f;
    public float wanderSpeed = 2f;
    public float fleeSpeed = 5f;
    public float detectionRange = 10f;
    public float attackRange = 2f;
    public float attackDamage = 10f;
    public float attackCooldown = 2f;
    public float fleeTime = 5f;

    // 기타 변수
    public float wanderRadius = 5f;
    public float wanderTimeMin = 2f;
    public float wanderTimeMax = 5f;
    public float idleTimeMin = 1f;
    public float idleTimeMax = 3f;
    public float wanderProbability = 0.5f;
    public float idleProbability = 0.5f;
    public AnimalType animalType = AnimalType.Predator;
    bool isInDamagedState = false;

    // 컴포넌트
    public NavMeshAgent agent;
    public Transform target;
    public Animator animator;
    public Collider col;

    // 스테이트
    public AnimalState currentState;
    public Anim_ChaseState chaseState;
    public Anim_IdleState idleState;
    public Anim_AttackState attackState;
    public Anim_WanderState wanderState;
    public Anim_FleeState fleeState;
    public Anim_DamagedState damagedState;
    public Anim_DeadState deadState;

    // 런타임 변수
    public float distanceToTarget;
    public bool isDead = false;


    protected virtual void Awake()
    {
        InitializeComponents();
        InitializeStates();
        InitializeStatus();
    }
    protected virtual void Start()
    {
        ChangeState(GetInitState());
    }
    protected virtual void Update()
    {
        currentState?.UpdateState();
        CheckTargetDistance();
    }

    public void ChangeState(AnimalState newState)
    {
        isInDamagedState = (newState == damagedState);
        if (currentState != null)
            currentState.ExitState();

        currentState = newState;
        currentState.EnterState();
    }


    #region 초기화 메소드들
    protected virtual void InitializeComponents()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        col = GetComponent<Collider>();
        target = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    protected virtual void InitializeStates()
    {
        chaseState = new Anim_ChaseState(this);
        idleState = new Anim_IdleState(this);
        attackState = new Anim_AttackState(this);
        wanderState = new Anim_WanderState(this);
        fleeState = new Anim_FleeState(this);
        damagedState = new Anim_DamagedState(this);
        deadState = new Anim_DeadState(this);
    }

    protected virtual void InitializeStatus()
    {
        agent.speed = speed;
        HP = maxHP;
        isDead = false;
    }

    protected virtual AnimalState GetInitState()
    {
        return idleState;
    }
    #endregion


    protected virtual void CheckTargetDistance()
    {
        if (target != null)
        {
            distanceToTarget = Vector3.Distance(transform.position, target.position);
        }
    }

    public virtual void TakeActiontoTarget()
    {
        switch (animalType)
        {
            case AnimalType.Predator:
                ChangeState(chaseState);
                return;
            case AnimalType.Prey:
                ChangeState(fleeState);
                return;
            default:
                ChangeState(chaseState);
                return;
        }
    }

    public virtual void TakeDamage(float damage)
    {
        if (isDead || isInDamagedState) return;

        HP -= damage;
        if (HP <= 0)
        {
            Die();
        }
        else
        {
            ChangeState(damagedState);
        }
    }

    protected virtual void Die()
    {
        ChangeState(deadState);
    }

    public virtual void OnIdleEnter() { }
    public virtual void OnIdleUpdate() { }
    public virtual void OnIdleExit() { }

    public virtual void OnAttackEnter() { }
    public virtual void OnAttackUpdate() { }
    public virtual void OnAttackExit() { }

    public virtual void OnWanderEnter() { }
    public virtual void OnWanderUpdate() { }
    public virtual void OnWanderExit() { }

    public virtual void OnFleeEnter() { }
    public virtual void OnFleeUpdate() { }
    public virtual void OnFleeExit() { }

    public virtual void OnChaseEnter() { }
    public virtual void OnChaseUpdate() { }
    public virtual void OnChaseExit() { }

}
