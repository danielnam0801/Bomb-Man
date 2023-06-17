using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RBBossSingleAttack : RbBossAttack
{
    [SerializeField] SingleProjectile singleProjectile;
    [SerializeField] Transform ShootPoint;
    [SerializeField] UnityEvent ChargeEvent;

    Action attackAct;
    
    public bool shootBulletCall = false;
    public bool endAttack = false;

    float singleDelay = 2.5f;

    public override void Attack(Action endAct)
    {
        this.endAct = endAct;
        //endAttack = false;
        _controller.AgentAnimator.UseSide(true);
        shootBulletCall = false;
        attackAct = null;
    }

    public override void AttackAnimationEndHandle()
    {
        endAct?.Invoke();
    }

    public override void AttackAnimationEventHandle()
    {
        attackAct?.Invoke();
    }

    public override void CancelAttack()
    {
        _controller.AgentAnimator.UseSide(false);
    }

    public override void PreAttack() //달리는 애니메이션 젤 처음 부분에 Event설정
    {
        _controller.NavMovement.SetSpeed(10);
        _controller.NavMovement.MoveToTarget(GameManager.Instance.PlayerOriginTrm.position);
        StartCoroutine(nameof(CheckTransShootIdle));
    }

    private void SetPreMovingAnim(out int idir)
    {
        _controller.NavMovement.StopImmediately();
        Vector3 dir = (GameManager.Instance.PlayerOriginTrm.position - transform.position).normalized;
        idir = (Vector3.Cross(transform.forward, dir).y) < 0 ? -1 : 1; // 적이 오른쪽에 있으면 왼쪽(-1)으로 가야됨
        _controller.AgentAnimator.SetRandDir(idir); // -1 ~ 0
        _controller.AgentAnimator.ChangeRunDirTrigger();
    }


    Vector3 _targetVector;
    private float RotateSpeed = 120f;
    private void SetTarget()
    {
        _targetVector = _controller.TargetTrm.position - transform.position;
        _targetVector.y = 0; //타겟을 바라보는 방향을 구해주는 매서드
    }


    float transAtkDis = 10;
    IEnumerator CheckTransShootIdle()
    {
        int idir;
        while (true)
        {
            float distance = Vector3.Distance(GameManager.Instance.PlayerOriginTrm.position, transform.position);
            if (distance <= transAtkDis) // 공격 전환 사거리
            {
                SetPreMovingAnim(out idir);
                break;
            }
            yield return null;
        }

        // 좌우로 이동
        float movingTime = 1f;
        float t = 0;
        while (t < movingTime)
        {
            Vector3 dir = (GameManager.Instance.PlayerOriginTrm.position - transform.position);
            dir.y = 0;
            _controller.transform.rotation = Quaternion.LookRotation(dir);
            
            Vector3 pos = transform.position;
            pos += idir == -1 ? Vector3.left : Vector3.right;
            _controller.NavMovement.MoveToTarget(pos);
            t += Time.deltaTime;
            yield return null;
        }
        //옆으로 원을 그리며 이동한 후
        //발사 대기상태 돌입
        _controller.NavMovement.StopImmediately();
        _controller.AgentAnimator.ShootWaitTrigger();

        ShootWaitEvent(); // 총쏘기 전까지 기다리는 동안 실행할것 // ex : 차징

        float delayTime = singleDelay;
        _controller.AgentAnimator.SetShootingType(0); //single = 0

        t = 0;
        while (!shootBulletCall)
        {
            SetTarget();
            Vector3 currentFrontVector = transform.forward;
            float angle = Vector3.Angle(currentFrontVector, _targetVector);

            //돌려야하고
            Vector3 result = Vector3.Cross(currentFrontVector, _targetVector);

            float sign = result.y > 0 ? 1 : -1;
            _controller.transform.rotation
                = Quaternion.Euler(0, sign * RotateSpeed * Time.deltaTime, 0)
                    * _controller.transform.rotation;
            
            t += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(0.2f);

        // 총쏠 준비 끝
        //SetAttackActInit(thisAttackType);
        _controller.AgentAnimator.SetAttackTrigger(RBBossAIBrain.AttackType.SingleShoot, true); // 총쏘는 애니메이션 실행
        yield return new WaitUntil(() => endAttack == true);
        endAttack = false;
        endAct?.Invoke();
    }

    private void ShootWaitEvent()
    {
        SingleShoot();
    }

    private void SingleShoot()
    {
        UtilMono.Instance.AddDelayCoroutine( () => ChargeEvent?.Invoke() , 0.5f);
        SingleProjectile projectile = PoolManager.Instance.Pop(singleProjectile.gameObject.name) as SingleProjectile;
        projectile.transform.SetParent(ShootPoint);
        projectile.transform.localPosition = Vector3.zero;
        projectile.AttackSet(this);
        projectile.Charge(singleDelay);
        attackAct += projectile.Shoot;
    }
}
