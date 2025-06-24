using UnityEngine;

public class PlayerRifleAimState : PlayerGroundState
{
    public PlayerRifleAimState(PlayerControll player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.anim.SetBool("IsAim", true);
        player.ChangeCameraStandRifle();
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
        if (xInput != 0 || zInput != 0)
        {
            player.anim.SetBool("IsAim", false);
            if (Input.GetKeyDown(KeyCode.LeftShift))
                stateMachine.ChangeState(player.rifleRunState);
            else
                stateMachine.ChangeState(player.rifleWalkState);
        }
        else if (Input.GetKeyDown(KeyCode.LeftControl))
            stateMachine.ChangeState(player.rifleSitAimState);
        else if (Input.GetMouseButtonDown(1))
        {
            stateMachine.ChangeState(player.rifleIdleState);
            player.anim.SetBool("IsAim", false);
        }
            
    }
}
