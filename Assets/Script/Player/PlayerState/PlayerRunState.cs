using UnityEngine;

public class PlayerRunState : PlayerGroundState
{
	public PlayerRunState(PlayerControll player, PlayerStateMachine stateMachine, string animBoolName)
		: base(player, stateMachine, animBoolName)
	{
	}

	public override void Enter()
	{
		base.Enter();
		applySpeed = player.runSpeed;
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
		if(Input.GetKeyUp(KeyCode.LeftShift))
		{
			if (xInput != 0 || zInput != 0)
				stateMachine.ChangeState(player.walkState);
			else if(xInput == 0 && zInput == 0)
				stateMachine.ChangeState(player.idleStete);
		}
		else if(Input.GetKeyDown(KeyCode.LeftControl))
			stateMachine.ChangeState(player.sitState);
		else if(Input.GetKeyDown(KeyCode.Space))
			stateMachine.ChangeState(player.jumpState);
	}
}
