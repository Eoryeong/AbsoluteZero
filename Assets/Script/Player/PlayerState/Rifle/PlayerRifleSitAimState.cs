using UnityEngine;

public class PlayerRifleSitAimState : PlayerGroundState
{
    public PlayerRifleSitAimState(PlayerControll player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
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
            stateMachine.ChangeState(player.rifleAimState);
        else if (Input.GetMouseButtonDown(1))
            stateMachine.ChangeState(player.rifleSitIdleState);
    }
}
