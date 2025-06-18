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
        if (player.navMeshObstacle != null)
            player.navMeshObstacle.height = player.characterController.height * 0.5f;
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
        if (player.navMeshObstacle != null)
            player.navMeshObstacle.height = player.characterController.height;
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
