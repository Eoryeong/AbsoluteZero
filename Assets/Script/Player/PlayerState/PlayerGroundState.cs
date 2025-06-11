using UnityEngine;

public class PlayerGroundState : PlayerState
{
	public PlayerGroundState(PlayerControll player, PlayerStateMachine stateMachine, string animBoolName) 
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
		ChangeState();
	}

	public override void Exit()
	{
		base.Exit();
		SetCurrentVelocity();
	}

	private void SetCurrentVelocity()
	{
		Vector3 inputDir = player.transform.right * xInput + player.transform.forward * zInput;
		inputDir = inputDir.normalized * applySpeed;

		player.velocity.x = inputDir.x;
		player.velocity.z = inputDir.z;
	}

	protected void MoveLogic()
	{
		Vector3 move = player.transform.right * xInput + player.transform.forward * zInput;
		move = move.normalized * applySpeed;

		Vector3 finalMove = move + Vector3.up * player.velocity.y;
		player.characterController.Move(finalMove * Time.deltaTime);
	}

	protected override void ChangeState()
	{
		if (!player.characterController.isGrounded)
			stateMachine.ChangeState(player.airState);
	}
}
