using UnityEngine;

public abstract class PlayerState 
{
    protected PlayerControll player;
    protected PlayerStateMachine stateMachine;
    protected string animBoolName;

    protected float xInput;
    protected float zInput;
    protected float applySpeed;


	public PlayerState(PlayerControll player, PlayerStateMachine stateMachine, string animBoolName)
	{
		this.player = player;
		this.stateMachine = stateMachine;
		this.animBoolName = animBoolName;
	}

    public virtual void Enter()
    {

    }

    public virtual void Update()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        zInput = Input.GetAxisRaw("Vertical");
    }

    public virtual void Exit()
    {

    }

    protected abstract void ChangeState();
}
