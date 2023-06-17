using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RBBossBurstAttack : RbBossAttack
{
    [SerializeField] BurstProjectile burstProjectile;
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

    public override void PreAttack() //�޸��� �ִϸ��̼� �� ó�� �κп� Event����
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
        _targetVector.y = 0; //Ÿ���� �ٶ󺸴� ������ �����ִ� �ż���
    }


    float transAtkDis = 12;
    IEnumerator CheckTransShootIdle()
    {
        while (true)
        {
            float distance = Vector3.Distance(GameManager.Instance.PlayerOriginTrm.position, transform.position);
            if (distance <= transAtkDis) // ���� ��ȯ ��Ÿ�
            {
                break;
            }
            yield return null;
        }

        //������ ���� �׸��� �̵��� ��
        //�߻� ������ ����
        _controller.NavMovement.StopImmediately();

        _controller.AgentAnimator.SetShootingType(1); //burst = 1
    
        while (true)
        {
            SetTarget();
            Vector3 currentFrontVector = transform.forward;
            float angle = Vector3.Angle(currentFrontVector, _targetVector);

            if (angle >= 10f)
            {
                //�������ϰ�
                Vector3 result = Vector3.Cross(currentFrontVector, _targetVector);

                float sign = result.y > 0 ? 1 : -1;
                _controller.transform.rotation
                    = Quaternion.Euler(0, sign * RotateSpeed * Time.deltaTime, 0)
                        * _controller.transform.rotation;
            }
            else //������ 10���� ���Դٸ�
            {
                break;          
            }
            yield return null;
        }

        yield return new WaitForSeconds(0.2f);

        _controller.AgentAnimator.SetAttackTrigger(RBBossAIBrain.AttackType.BurstShoot, true); // �ѽ�� �ִϸ��̼� ����
    }

    private void BurstShoot()
    {
        BurstProjectile burst = PoolManager.Instance.Pop(burstProjectile.gameObject.name) as BurstProjectile;
        burst.transform.position = ShootPoint.position;
        burst.Shoot();

        
    }
}
