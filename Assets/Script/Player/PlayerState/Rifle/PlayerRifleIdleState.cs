using UnityEngine;

public class PlayerRifleIdleState : PlayerGroundState
{
    public PlayerRifleIdleState(PlayerControll player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.onRifle = true;
    }

    public override void Update()
    {
        base.Update();
        ChangeState();
        MoveLogic();
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
            if (Input.GetKeyDown(KeyCode.LeftShift))
                stateMachine.ChangeState(player.rifleRunState);
            else
                stateMachine.ChangeState(player.rifleWalkState);
        }
        else if (Input.GetKeyDown(KeyCode.LeftControl))
            stateMachine.ChangeState(player.rifleSitIdleState);
        else if (Input.GetMouseButtonDown(1))
            stateMachine.ChangeState(player.rifleAimState);
    }
}
