using UnityEngine;

public class PlayerLoggingState : PlayerState
{
    public PlayerLoggingState(PlayerControll player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Exit()
    {
        base.Exit();
    }

    protected override void ChangeState()
    {
        // 나무 베기 종료시 idle 이동
    }
}
