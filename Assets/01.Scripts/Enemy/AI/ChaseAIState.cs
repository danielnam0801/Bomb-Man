using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseAIState : CommonAIState
{
    public override void OnEnterState()
    {
        _enemyController.AgentAnimator.OnAnimationEventTrigger += FootStepHandle;
        _enemyController.AgentAnimator.SetSpeed(0.2f);
    }

    public override void OnExitState()
    {
        _enemyController.AgentAnimator.OnAnimationEventTrigger -= FootStepHandle;
        _enemyController.AgentAnimator.SetSpeed(0);
    }

    private void FootStepHandle()
    {
        _enemyController.VFXManager.PlayFootStep();
    }

    public override bool OnUpdateState()
    {
        _aiActionData.canMove = false;
       
        _enemyController.NavMovement.MoveToTarget(_aiActionData.LastSpotPoint);
        _aiActionData.IsArrived = _enemyController.NavMovement.CheckIsArrived();

        return base.OnUpdateState();
    }
}
