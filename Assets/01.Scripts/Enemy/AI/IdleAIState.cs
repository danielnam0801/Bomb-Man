using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleAIState : CommonAIState
{
    public override void OnEnterState()
    {
        
    }

    public override void OnExitState()
    {
        _enemyController.AgentAnimator.SetDetectPlayer();
        PlayerManager.Instance.FindPlayer();
    }

    public override bool OnUpdateState()
    {
        return base.OnUpdateState();
    }
}
