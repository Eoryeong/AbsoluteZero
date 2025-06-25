using UnityEngine;

public class PlayerSitWalkState : PlayerGroundState
{
	public PlayerSitWalkState(PlayerControll player, PlayerStateMachine stateMachine, string animBoolName)
		: base(player, stateMachine, animBoolName)
	{
	}

	public override void Enter()
	{
		base.Enter();
		applySpeed = player.sitSpeed;

	}

	public override void Update()
	{
		base.Update();
		MoveLogic();
		ChangeState();
	}

	public override void Exit()
	{
		base.Exit();
	}

	protected override void ChangeState()
	{
		base.ChangeState();
		if (Input.GetKeyUp(KeyCode.LeftControl))
		{
			if (player.onRifle)
				stateMachine.ChangeState(player.rifleIdleState);
			else
				stateMachine.ChangeState(player.idleState);
		}
		else if (xInput == 0 && zInput == 0)
			stateMachine.ChangeState(player.sitState);
	}
}
