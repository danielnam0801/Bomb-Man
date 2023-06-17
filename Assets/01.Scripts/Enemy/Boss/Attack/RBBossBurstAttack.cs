using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RBBossBurstAttack : RbBossAttack
{
    [SerializeField] Dynamite burstProjectile;
    [SerializeField] Transform ShootPoint;
    [SerializeField] UnityEvent ChargeEvent;

    Action attackAct;

    public bool shootBulletCall = false;
    public bool endAttack = false;

    
    public override void Attack(Action endAct)
    {
        this.endAct = endAct;
        shootBulletCall = false;
        attackAct = null;
    }

    public override void AttackAnimationEndHandle()
    {
        UtilMono.Instance.AddDelayCoroutine(() => endAct?.Invoke(), 1f);
    }

    public override void AttackAnimationEventHandle()
    {
        BurstShoot();
    }

    public override void CancelAttack()
    {
        
    }

    public override void PreAttack() //달리는 애니메이션 젤 처음 부분에 Event설정
    {
        _controller.NavMovement.SetSpeed(10);
        _controller.NavMovement.MoveToTarget(GameManager.Instance.PlayerOriginTrm.position);
        StartCoroutine(nameof(CheckTransShootIdle));
    }

    Vector3 _targetVector;
    private float RotateSpeed = 120f;
    private void SetTarget()
    {
        _targetVector = _controller.TargetTrm.position - transform.position;
        _targetVector.y = 0; //타겟을 바라보는 방향을 구해주는 매서드
    }


    float transAtkDis = 12;
    IEnumerator CheckTransShootIdle()
    {
        while (true)
        {
            float distance = Vector3.Distance(GameManager.Instance.PlayerOriginTrm.position, transform.position);
            if (distance <= transAtkDis) // 공격 전환 사거리
            {
                break;
            }
            yield return null;
        }

        //옆으로 원을 그리며 이동한 후
        //발사 대기상태 돌입
        _controller.NavMovement.StopImmediately();

        _controller.AgentAnimator.SetShootingType(1); //burst = 1
    
        while (true)
        {
            SetTarget();
            Vector3 currentFrontVector = transform.forward;
            float angle = Vector3.Angle(currentFrontVector, _targetVector);

            if (angle >= 10f)
            {
                //돌려야하고
                Vector3 result = Vector3.Cross(currentFrontVector, _targetVector);

                float sign = result.y > 0 ? 1 : -1;
                _controller.transform.rotation
                    = Quaternion.Euler(0, sign * RotateSpeed * Time.deltaTime, 0)
                        * _controller.transform.rotation;
            }
            else //각도도 10도로 들어왔다면
            {
                break;          
            }
            yield return null;
        }

        yield return new WaitForSeconds(0.2f);

        _controller.AgentAnimator.SetAttackTrigger(RBBossAIBrain.AttackType.BurstShoot, true); // 총쏘는 애니메이션 실행
    }

    private void BurstShoot()
    {
        Vector3 startPos = ShootPoint.position;
        Vector3 randomAddPos = UnityEngine.Random.insideUnitSphere * 1.5f;
        randomAddPos.y = 0;
        Vector3 endPos = GameManager.Instance.PlayerOriginTrm.position + randomAddPos;

        Vector3 cp1 = new Vector3(startPos.x, startPos.y + UnityEngine.Random.Range(-2f, 2f), startPos.z);
        Vector3 cp2 = new Vector3(endPos.x, startPos.y, endPos.z);

        Vector3[] positions = new Vector3[20 + 1];
        positions = DOCurve.CubicBezier.GetSegmentPointCloud(startPoint: startPos,
            startControlPoint: cp1, endPoint: endPos, endControlPoint: cp2, 20);

        Dynamite burst = PoolManager.Instance.Pop(burstProjectile.gameObject.name) as Dynamite;
        burst.Damage = this.Damage;
        burst.isEnemyBomb = true;
        burst.Shoot(startPos, cp1, cp2, endPos, 20, UnityEngine.Random.Range(15f, 30f));
    }
}
