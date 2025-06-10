using UnityEngine;

public class PlayerAirState : PlayerState
{
	public PlayerAirState(PlayerControll player, PlayerStateMachine stateMachine, string animBoolName)
		: base(player, stateMachine, animBoolName)
	{
	}

	public override void Enter()
	{
		base.Enter();
	}

	public override void Update()
	{
		base.Update();
		player.characterController.Move(player.velocity * Time.deltaTime);
		ChangeState();
	}

	public override void Exit()
	{
		base.Exit();
		player.velocity.x = 0;
		player.velocity.z = 0;
	}

	protected override void ChangeState()
	{
		if (player.characterController.isGrounded)
			stateMachine.ChangeState(player.idleState);
	}
}
