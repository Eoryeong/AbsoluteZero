using UnityEngine;

public class PlayerAirState : PlayerState
{
	private Vector3 FallingStartPos;
	private Vector3 FallingEndPos;

	public PlayerAirState(PlayerControll player, PlayerStateMachine stateMachine, string animBoolName)
		: base(player, stateMachine, animBoolName)
	{
	}

	public override void Enter()
	{
		base.Enter();
		FallingStartPos = player.transform.position;
	}

	public override void Update()
	{
		base.Update();
		player.characterController.Move(player.velocity * Time.deltaTime);
		ChangeState();
		Debug.Log("에어 상태");
	}

	public override void Exit()
	{
		base.Exit();
		player.velocity.x = 0;
		player.velocity.z = 0;

		FallingEndPos = player.transform.position;

		float hight = FallingStartPos.y - FallingEndPos.y;

		if (hight > player.fallingHight)
		{
			PlayerStatusManager.Instance.TakeDamage(hight * player.fallDamageRate);
		}
	}

	protected override void ChangeState()
	{
		if (player.characterController.isGrounded)
		{
			SoundManager.Instance.PlayFootstep(SoundManager.FootstepType.Snow);
			Debug.Log("에어 사운드 오류");
			if (player.onRifle)
				stateMachine.ChangeState(player.rifleIdleState);
			else
				stateMachine.ChangeState(player.idleState);

		}
	}
}
