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
		SetCurrentVelocity();
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
	private void SetCurrentVelocity()
	{
		Vector3 currentVelocity = new Vector3(player.rb.linearVelocity.x, 0f, player.rb.linearVelocity.z);

		player.rb.linearVelocity = new Vector3(currentVelocity.x, player.rb.linearVelocity.y, currentVelocity.z);
	}
}
