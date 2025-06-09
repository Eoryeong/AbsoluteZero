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
		Vector3 move = (player.transform.right * xInput + player.transform.forward * zInput).normalized;
		Vector3 velocity = move * applySpeed;

		player.rb.linearVelocity = velocity;
	}

	protected override void ChangeState()
	{
		if (!player.GroundCheck())
			stateMachine.ChangeState(player.airState);
	}
}
