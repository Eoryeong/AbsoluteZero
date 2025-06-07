using UnityEngine;

public class PlayerStateMachine
{
    private PlayerState currentState;

    public void InitState(PlayerState newState)
    {
        currentState = newState;
        currentState.Enter();
    }

    public void ChangeState(PlayerState newState)
    {
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public void Update()
    {
        currentState.Update();
    }
}
