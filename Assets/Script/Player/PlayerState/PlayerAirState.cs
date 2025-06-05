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
		SetCurrentVelocity();
		ChangeState();
	}

	public override void Exit()
	{
		base.Exit();
	}

	protected override void ChangeState()
	{
		if (player.GroundCheck())
			stateMachine.ChangeState(player.idleStete);
	}

	private void SetCurrentVelocity()
	{
		Vector3 currentVelocity = new Vector3(player.rb.linearVelocity.x, 0f, player.rb.linearVelocity.z);

		player.rb.linearVelocity = new Vector3(currentVelocity.x, player.rb.linearVelocity.y, currentVelocity.z);
	}
}
