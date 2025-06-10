using Unity.Android.Gradle.Manifest;
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
		applySpeed = player.walkSpeed;
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
		if (player.velocity.y < 0)
			stateMachine.ChangeState(player.airState);
	}

	private void JumpLogic()
	{
		player.velocity.y = Mathf.Sqrt(player.jumpForce * 2f * player.gravity);
	}
}
