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
		JumpLogic();
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
	}

	protected override void ChangeState()
	{
		if (player.velocity.y < 0)
			stateMachine.ChangeState(player.airState);
	}

	private void JumpLogic()
	{
		player.velocity.y = Mathf.Sqrt(player.jumpForce * 2f * -player.gravity);
	}
}
