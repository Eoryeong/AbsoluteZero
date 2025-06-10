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
}
