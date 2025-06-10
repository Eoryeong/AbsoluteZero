using Unity.Android.Gradle.Manifest;
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
		GetInput();
		Gravity();
	}

	public virtual void Exit()
    {

    }

    protected abstract void ChangeState();

	private void GetInput()
	{
		xInput = Input.GetAxisRaw("Horizontal");
		zInput = Input.GetAxisRaw("Vertical");
	}

	protected void Gravity()
    {
		if (player.characterController.isGrounded && player.velocity.y < 0)
		{
			player.velocity.y = -2f; // 바닥에 붙어있도록
		}
		else
		{
			player.velocity.y += player.gravity * Time.deltaTime;
		}
	}
}
