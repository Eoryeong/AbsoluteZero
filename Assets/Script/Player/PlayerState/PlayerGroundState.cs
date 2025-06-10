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
	}

	protected void MoveLogic()
	{
		Vector3 move = player.transform.right * xInput+ player.transform.forward * zInput;
		move = move.normalized * applySpeed;

		Vector3 finalMove = move * Time.deltaTime + Vector3.up * player.velocity.y * Time.deltaTime;
		player.characterController.Move(finalMove);
	}

	protected override void ChangeState()
	{
		if (!player.characterController.isGrounded)
			stateMachine.ChangeState(player.airState);
	}
}
