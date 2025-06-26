using UnityEngine;

public class PlayerGroundState : PlayerState
{
	public PlayerGroundState(PlayerControll player, PlayerStateMachine stateMachine, string animBoolName) 
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
	}

	public override void Exit()
	{
		base.Exit();
		SetCurrentVelocity();
	}

	private void SetCurrentVelocity()
	{
		Vector3 inputDir = player.transform.right * xInput + player.transform.forward * zInput;
		inputDir = inputDir.normalized * applySpeed;

		player.velocity.x = inputDir.x;
		player.velocity.z = inputDir.z;
	}

	protected void MoveLogic()
	{
        Vector3 move = player.transform.right * xInput + player.transform.forward * zInput;
		move = move.normalized * applySpeed;

		Vector3 finalMove = move + Vector3.up * player.velocity.y;
		player.characterController.Move(finalMove * Time.deltaTime);

        player.anim.SetFloat("ForwardSpeed", applySpeed, 0.2f, Time.deltaTime);        
    }

	protected void MoveSoundChoice()
	{
        if (applySpeed == player.runSpeed)
        {
            player.soundDelay = player.RunSoundDelay;
        }
        else
        {
            player.soundDelay = player.walkSoundDelay;
        }
    }

    protected void MoveSoundApply()
    {
		player.soundDelay -= Time.deltaTime;
		if(player.soundDelay <= 0)
		{
            SoundManager.Instance.PlayFootstep(SoundManager.FootstepType.Snow);
            MoveSoundChoice();
        }
    }

    protected override void ChangeState()
	{
		if (!player.characterController.isGrounded)
			stateMachine.ChangeState(player.airState);
		else if (player.IsOnSteepSlope())
			stateMachine.ChangeState(player.slideState);
	}
}
