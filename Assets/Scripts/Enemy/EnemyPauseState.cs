using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// a state that does nothing
public class EnemyPauseState : EnemyBaseState
{
    public override void EnterState(EnemyStateManager enemy) {}

    public override void UpdateState(EnemyStateManager enemy) {}

    public override void ExitState(EnemyStateManager enemy) {}
}

