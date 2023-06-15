using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RbBossDashAttack : RbBossAttack
{
    public override void Attack(Action act)
    {
        endAct = act;
        _actionData.IsArrived = false;
        StartCoroutine(nameof(Dash));
    }

    public override void CancelAttack()
    {
        
    }

    public override void PreAttack()
    {
        
    }

    public override void AttackAnimationEndHandle() // RunToStopAnimation에서 사용할것
    {
        endAct?.Invoke();
    }

    public override void AttackAnimationEventHandle() // 
    {
        _controller.NavMovement.SetSpeed(10);
        _controller.NavMovement.MoveToTarget(GameManager.Instance.PlayerTrm.position);   
    }

    IEnumerator Dash()
    {
        while (_actionData.IsArrived == false)
        {
            _actionData.IsArrived = _controller.NavMovement.CheckIsArrived();
            yield return null;
        }
        //도착했을때

        _controller.AgentAnimator.SetDashEndTrigger();
    }

}
