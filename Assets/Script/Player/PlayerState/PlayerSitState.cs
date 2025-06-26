using UnityEngine;
using UnityEngine.AI;

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

		player.ChangeCameraCrouch();
		player.isCrouch = true;
    }

	public override void Update()
	{
		base.Update();
        MoveLogic();
        ChangeState();
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
		else if (xInput != 0 || zInput != 0)
			stateMachine.ChangeState(player.sitWalkState);
	}
}
