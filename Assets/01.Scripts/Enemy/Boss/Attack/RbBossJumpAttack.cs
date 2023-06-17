using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RbBossJumpAttack : RbBossAttack
{
    public UnityEvent JumpStartEvent;
    public UnityEvent JumpEndEvent;
    [SerializeField] EffectPlayer landEffect;
    [SerializeField] Transform alertCircle;

    bool isJumpStart;
    bool isAir;
    public override void Attack(Action endAct)
    {
        isAir = false;
        isJumpStart = false;
        this.endAct = endAct;
        StartCoroutine(nameof(Jump));
        _controller.AgentAnimator.BossJumpStartEventTrigger += JumpStart;
    }

    public void JumpStart()
    {
        JumpStartEvent?.Invoke(); //ShakeFeedfback
        isJumpStart = true;
    }

    public override void AttackAnimationEndHandle()
    {
        _controller.AgentAnimator.BossJumpStartEventTrigger -= JumpStart;
        endAct?.Invoke();
    }

    public override void AttackAnimationEventHandle()
    {
        JumpEndEvent?.Invoke();
        EffectPlayer landVFX = PoolManager.Instance.Pop(landEffect.gameObject.name) as EffectPlayer;
        landVFX.transform.position = _controller.transform.position;
        landVFX.StartPlay(4f);
    }

    public override void CancelAttack()
    {
        _controller.NavMovement.ResetNavAgent();
        alertCircle.gameObject.SetActive(false);
    }

    public override void PreAttack()
    {
        isAir = true;
        _controller.AgentAnimator.StopAnimator(true);
    }

    Vector3 targetPos;
    IEnumerator Jump()
    {
        _controller.NavMovement.StopNavigation();
        yield return new WaitUntil(() => isJumpStart);

        Vector3 startPos = _controller.transform.position;
        targetPos = GameManager.Instance.PlayerOriginTrm.position;//점프 지점 구하고
        Vector3 startControl = (targetPos - transform.position) / 4;

        alertCircle.gameObject.SetActive(true);
        alertCircle.position = targetPos;

        float distanceX = targetPos.x - startPos.x;
        float distanceZ = targetPos.z - startPos.z;

        float yHeight = 7;
        Vector3 cp1 = new Vector3(distanceX / 3, yHeight, distanceZ / 3) + startPos;
        Vector3 cp2 = new Vector3(distanceX / 3 * 2, yHeight, distanceZ / 3 * 2) + startPos;


        float _jumpSpeed = 0.2f;
        float _frameSpeed;

         Vector3[] _bezierPoints = DOCurve.CubicBezier.GetSegmentPointCloud(startPos,
            cp1, targetPos, cp2, 60);
        _frameSpeed = _jumpSpeed / 60;

        for(int i = 0; i < _bezierPoints.Length; i++) //일단 절반만 점프
        {
            yield return new WaitForSeconds(_frameSpeed);
            _controller.transform.position = _bezierPoints[i];
            if (i == _bezierPoints.Length / 2) yield return new WaitForSeconds(0.5f);
            if(i == _bezierPoints.Length - 2)
            {
                _controller.AgentAnimator.StopAnimator(false);
            }
        }
    }
}
