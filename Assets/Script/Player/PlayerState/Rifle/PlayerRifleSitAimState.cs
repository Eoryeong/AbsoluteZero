using UnityEngine;

public class PlayerRifleSitAimState : PlayerGroundState
{
    public PlayerRifleSitAimState(PlayerControll player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        player.ChangeCameraCrouchRifle();
        base.Enter();
        player.anim.SetBool("IsAim", true);
    }

    public override void Update()
    {
        base.Update();
        ChangeState();
        if (Input.GetMouseButtonDown(0))
        {
            // 장탄, 총알 검사
            player.anim.SetTrigger("OnFire");
            player.FireRifleBullet();
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    protected override void ChangeState()
    {
        base.ChangeState();
        if (Input.GetKeyUp(KeyCode.LeftControl))
            stateMachine.ChangeState(player.rifleAimState);
        else if (xInput != 0 || zInput != 0)
        {
            stateMachine.ChangeState(player.rifleSitWalkState);
            player.anim.SetBool("IsAim", false);
        }
        else if (Input.GetMouseButtonDown(1))
        {
            stateMachine.ChangeState(player.rifleSitIdleState);
            player.anim.SetBool("IsAim", false);
        }
    }
}
