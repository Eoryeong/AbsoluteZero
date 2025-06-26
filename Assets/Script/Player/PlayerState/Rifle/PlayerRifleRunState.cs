using UnityEngine;

public class PlayerRifleRunState : PlayerGroundState
{
    public PlayerRifleRunState(PlayerControll player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        applySpeed = player.runSpeed;
        MoveSoundChoice();
    }

    public override void Update()
    {
        base.Update();
        MoveLogic();
        ChangeState();
        MoveSoundApply();
    }

    public override void Exit()
    {
        base.Exit();
    }

    protected override void ChangeState()
    {
        base.ChangeState();
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            if (xInput != 0 || zInput != 0)
                stateMachine.ChangeState(player.rifleWalkState);
            else if (xInput == 0 && zInput == 0)
                stateMachine.ChangeState(player.rifleIdleState);
        }
        else if (Input.GetKeyDown(KeyCode.LeftControl))
            stateMachine.ChangeState(player.rifleSitIdleState);
        /*else if (Input.GetKeyDown(KeyCode.Space))
            stateMachine.ChangeState(player.jumpState);*/
    }
}
