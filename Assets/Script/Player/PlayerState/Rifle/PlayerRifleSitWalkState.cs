using UnityEngine;

public class PlayerRifleSitWalkState : PlayerGroundState
{
    public PlayerRifleSitWalkState(PlayerControll player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        applySpeed = player.sitSpeed;
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
            stateMachine.ChangeState(player.rifleIdleState);
        else if (xInput == 0 && zInput == 0)
            stateMachine.ChangeState(player.rifleSitIdleState);
    }
}
