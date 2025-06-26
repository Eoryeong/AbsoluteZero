using UnityEngine;

public class PlayerSlideState : PlayerState
{
	private Vector3 slideDirection;

	public PlayerSlideState(PlayerControll player, PlayerStateMachine stateMachine, string animBoolName) 
		: base(player, stateMachine, animBoolName)
	{
	}

	public override void Enter()
	{
		base.Enter();
		slideDirection = GetSlopeDirection();
		applySpeed = player.slideSpeed;
    }

	public override void Update()
	{
		base.Update();
		SlideLogic();
		ChangeState();
	}

	public override void Exit()
	{
		base.Exit();
	}

	protected override void ChangeState()
	{
		if (!player.IsOnSteepSlope())
		{
			if (player.onRifle)
			{
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    stateMachine.ChangeState(player.rifleRunState);
                }
                else
                {
                    stateMachine.ChangeState(player.rifleIdleState);
                }
            }
			else
			{
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    stateMachine.ChangeState(player.runState);
                }
                else
                {
                    stateMachine.ChangeState(player.idleState);
                }
            }
		}
	}

	private void SlideLogic()
	{
		Vector3 finalMove = slideDirection * applySpeed + Vector3.up * player.velocity.y;
		player.characterController.Move(finalMove * Time.deltaTime);
	}

	private Vector3 GetSlopeDirection()
	{
		if (Physics.Raycast(player.transform.position, Vector3.down, out RaycastHit hit, 1.5f))
		{
			Vector3 slopeDir = Vector3.ProjectOnPlane(Vector3.down, hit.normal).normalized;
			return slopeDir;
		}
		return Vector3.zero;
	}
}
