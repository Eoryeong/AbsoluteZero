using UnityEngine;

public class PlayerRifleAimState : PlayerGroundState
{
    public PlayerRifleAimState(PlayerControll player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("발사");
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    protected override void ChangeState()
    {
        base.ChangeState();
        if (Input.GetKeyDown(KeyCode.LeftControl))
            stateMachine.ChangeState(player.rifleSitAimState);
        else if (Input.GetMouseButtonDown(1))
            stateMachine.ChangeState(player.rifleIdleState);
    }
}
