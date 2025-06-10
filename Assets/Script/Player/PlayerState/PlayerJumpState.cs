using UnityEngine;

public class PlayerJumpState : PlayerState
{
	public PlayerJumpState(PlayerControll player, PlayerStateMachine stateMachine, string animBoolName)
		: base(player, stateMachine, animBoolName)
	{
	}

	public override void Enter()
	{
		base.Enter();
		JumpLogic();
	}

	public override void Update()
	{
		base.Update();
		ChangeState();
	}

	public override void Exit()
	{
		base.Exit();
	}

	protected override void ChangeState()
	{
		if (player.rb.linearVelocity.y < 0)
			stateMachine.ChangeState(player.airState);
	}

	private void JumpLogic()
	{			
		player.rb.AddForce(Vector3.up * player.jumpForce, ForceMode.Impulse);
	}
}
