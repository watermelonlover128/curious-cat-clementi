using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    BaseState currentState;

    void Update()
    {
        currentState?.UpdateState(this);
    }

    public void ChangeState(BaseState state)
    {
        currentState?.ExitState(this);
        currentState = state;
        currentState.EnterState(this);
    }

    public BaseState checkCurrentState()
    {
        return currentState;
    }
}
