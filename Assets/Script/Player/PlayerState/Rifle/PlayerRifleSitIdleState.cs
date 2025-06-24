using UnityEngine;

public class PlayerRifleSitIdleState : PlayerGroundState
{
    public PlayerRifleSitIdleState(PlayerControll player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        player.ChangeCameraCrouchRifle();
        base.Enter();
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
        if (Input.GetKeyUp(KeyCode.LeftControl))
            stateMachine.ChangeState(player.rifleIdleState);
        else if (xInput != 0 || zInput != 0)
            stateMachine.ChangeState(player.rifleSitWalkState);
        else if (Input.GetMouseButtonDown(1))
            stateMachine.ChangeState(player.rifleSitAimState);
    }
}
