using System.Collections;
using UnityEngine;

public class Wolf : Animal
{
    //늑대 고유 변수
    float sitdigTimer = 4f;
    float howlTimer = 10f;
    public Anim_ResponseHowlState responseHowlState;




    protected override void Start()
    {
        base.Start();
        agent.stoppingDistance = attackRange - 0.1f;
    }
    protected override void InitializeStatus()
    {
        maxHP = 120f;
        speed = 6f;
        wanderSpeed = 3f;
        fleeSpeed = 8f;
        detectionRange = 15f;
        attackRange = 2.5f;
        attackDamage = 5f;
        attackCooldown = 2.5f;
        wanderRadius = 20f;
        wanderTimeMin = 5f;
        wanderTimeMax = 7f;
        idleTimeMin = 8f;
        idleTimeMax = 10f;
        wanderProbability = 0.7f;
        idleProbability = 0.4f;
        fleeTime = 6f;
        animalType = AnimalType.Predator;

        base.InitializeStatus();
    }

    protected override void InitializeStates()
    {
        base.InitializeStates();
        responseHowlState = new Anim_ResponseHowlState(this);
    }

    protected override AnimalState GetInitState()
    {
        return Random.Range(0f, 1f) < wanderProbability ? wanderState : idleState;
        
    }


    public override void OnIdleUpdate()
    {
        sitdigTimer -= Time.deltaTime;
        if (sitdigTimer <= 0f)
        {
            if(Random.Range(0f, 1f) < 0.25f)
            {
                Sit();
            }
            else if(Random.Range(0f, 1f) < 0.5f)
            {
                Dig();
            }
            sitdigTimer = Random.Range(4f, 6f);
        }
    }



    public override void OnChaseUpdate()
    {
        howlTimer -= Time.deltaTime;
        if (howlTimer <= 0f)
        {
            if(Random.Range(0f, 1f) < 0.5f)
            {
                Howl();
                Debug.Log("Wolf is chasing and howling!");
            }
            else
            {
                Debug.Log("Wolf is chasing but not howling.");
            }
            howlTimer = 10f;
        }
    }


    private void Sit()
    {
        animator.SetTrigger("Sit");
    }

    private void Howl()
    {
        animator.SetTrigger("Howl");
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRange*1.5f);
        foreach (Collider collider in colliders)
        {
            Wolf otherwolf = collider.GetComponent<Wolf>();
            otherwolf.HearHowl();
        }
    }
    public void HearHowl()
    {
        if (currentState == idleState || currentState == wanderState)
        {
            ChangeState(responseHowlState);
        }
    }

    private void Dig()
    {
        animator.SetTrigger("Dig");
    }

}
