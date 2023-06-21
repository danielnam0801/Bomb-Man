using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RbBossDashAttack : RbBossAttack
{
    public UnityEvent dashStartFeedback;

    bool checkPlayer = false;
    protected override void Awake()
    {
        base.Awake();
        checkPlayer = false;
    }

    public override void Attack(Action act)
    {
        endAct = act;
        _actionData.IsArrived = false;
        checkPlayer = true;
    }

    public override void CancelAttack()
    {

    }

    public override void PreAttack()
    {
        dashStartFeedback?.Invoke();
    }

    public override void AttackAnimationEndHandle() // RunToStopAnimation에서 사용할것
    {
        _controller.NavMovement.StopImmediately();
        endAct += () => _controller.AgentAnimator.SetAttackState(false);
        endAct += () => checkPlayer = false;
        UtilMono.Instance.AddDelayCoroutine(() => endAct?.Invoke(), 0.95f);
    }

    public override void AttackAnimationEventHandle() // 
    {
        StartCoroutine(nameof(Dash));
    }

    IEnumerator Dash()
    {
        int setMaxCnt;
        if (_phaseData.CurrentPhase == 1)
            setMaxCnt = 1;
        else if (_phaseData.CurrentPhase == 2)
            setMaxCnt = UnityEngine.Random.Range(1, 3);
        else
            setMaxCnt = UnityEngine.Random.Range(2, 4);

        int currentCnt = 0;
        Vector3 dir = (GameManager.Instance.PlayerOriginTrm.position - transform.position).normalized;
        _controller.NavMovement.MoveToTarget(GameManager.Instance.PlayerOriginTrm.position + dir * 3);
        _controller.NavMovement.SetSpeed(10 + _phaseData.CurrentPhase * 5);

        while (true)
        {
            if (currentCnt >= setMaxCnt) break;
            if (_actionData.IsArrived)
            {
                _actionData.IsArrived = false;
                dir = (GameManager.Instance.PlayerOriginTrm.position - transform.position).normalized;
                _controller.NavMovement.MoveToTarget(GameManager.Instance.PlayerOriginTrm.position + dir * 3);
                _controller.NavMovement.SetSpeed(15 + currentCnt * 5);
                currentCnt++;
            }
            _actionData.IsArrived = _controller.NavMovement.CheckIsArrived();
            yield return null;
        }
        //도착했을때

        _controller.AgentAnimator.SetDashEndTrigger();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Damage");
            if (checkPlayer)
            {
                other.gameObject.GetComponent<IDamageable>()
                    .OnDamage(Damage, transform.position, GameManager.Instance.PlayerOriginTrm.position - transform.position);
            }
        }
    }
}
