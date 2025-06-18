using UnityEngine;

public class PlayerRifleWalkState : PlayerGroundState
{
    public PlayerRifleWalkState(PlayerControll player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        applySpeed = player.walkSpeed;
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
        base.ChangeState();
        if (xInput == 0 && zInput == 0)
            stateMachine.ChangeState(player.rifleIdleState);
        else if (Input.GetKeyDown(KeyCode.LeftShift))
            stateMachine.ChangeState(player.rifleRunState);
        else if (Input.GetKeyDown(KeyCode.LeftControl))
            stateMachine.ChangeState(player.rifleSitIdleState);
        /*else if (Input.GetKeyDown(KeyCode.Space))
            stateMachine.ChangeState(player.jumpState);*/
    }
}
