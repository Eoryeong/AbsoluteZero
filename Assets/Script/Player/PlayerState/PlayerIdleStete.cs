using UnityEngine;

public class PlayerIdleStete : PlayerGroundState
{
	public PlayerIdleStete(PlayerControll player, PlayerStateMachine stateMachine, string animBoolName) 
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
		if(xInput != 0 || zInput != 0)
		{
			if (Input.GetKeyDown(KeyCode.LeftShift))
				stateMachine.ChangeState(player.runState);
			else
				stateMachine.ChangeState(player.walkState);
		}
		else if(Input.GetKeyDown(KeyCode.LeftControl))
			stateMachine.ChangeState(player.sitState);
		else if(Input.GetKeyDown(KeyCode.Space))
			stateMachine.ChangeState(player.jumpState);
	}
}
