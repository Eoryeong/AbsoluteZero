using UnityEngine;

public class PlayerWalkState : PlayerGroundState
{
	public PlayerWalkState(PlayerControll player, PlayerStateMachine stateMachine, string animBoolName)
		: base(player, stateMachine, animBoolName)
	{
	}

	public override void Enter()
	{
		base.Enter();
		applySpeed = player.walkSpeed;
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
		if (xInput == 0 && zInput == 0)
			stateMachine.ChangeState(player.idleStete);
		else if (Input.GetKeyDown(KeyCode.LeftShift))
			stateMachine.ChangeState(player.runState);
		else if(Input.GetKeyDown(KeyCode.LeftControl))
			stateMachine.ChangeState(player.sitState);
		else if(Input.GetKeyDown(KeyCode.Space))
			stateMachine.ChangeState(player.jumpState);
	}
}
