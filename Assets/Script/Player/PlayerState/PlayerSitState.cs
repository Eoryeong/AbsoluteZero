using UnityEngine;

public class PlayerSitState : PlayerGroundState
{
	public PlayerSitState(PlayerControll player, PlayerStateMachine stateMachine, string animBoolName)
		: base(player, stateMachine, animBoolName)
	{
	}

	public override void Enter()
	{
		base.Enter();
		applySpeed = 0f;
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
		if(Input.GetKeyUp(KeyCode.LeftShift))
			stateMachine.ChangeState(player.idleState);
		else if (xInput != 0 || zInput != 0)
			stateMachine.ChangeState(player.sitWalkState);
	}
}
