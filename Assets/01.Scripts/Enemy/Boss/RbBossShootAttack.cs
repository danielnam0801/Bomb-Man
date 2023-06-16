using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RbBossShootAttack : RbBossAttack
{
    [SerializeField] SingleProjectile singleProjectile;
    [SerializeField] BurstProjectile burstProjectile;
    [SerializeField] AutoProjectile autoProjectile;
    public override void Attack(Action endAct)
    {
        endAct?.Invoke();
    }

    public override void AttackAnimationEndHandle()
    {
       
    }

    public override void AttackAnimationEventHandle()
    {
        
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

    private void SetPreMovingAnim(out int idir)
    {
        _controller.AgentAnimator.SetAttackTrigger(RBBossAIBrain.AttackType.Shoot, true);

        Vector3 dir = (GameManager.Instance.PlayerOriginTrm.position - transform.position).normalized;
        idir = (Vector3.Cross(transform.forward, dir).y) < 0 ? -1 : 1; // ���� �����ʿ� ������ ����(-1)���� ���ߵ�
        _controller.AgentAnimator.SetRandDir(idir); // -1 ~ 0
    }

    float transAtkDis = 6;
    IEnumerator CheckTransShootIdle()
    {
        int idir;
        while (true)
        {
            float distance = Vector3.Distance(GameManager.Instance.PlayerOriginTrm.position, transform.position);
            if(distance <= transAtkDis) // ���� ��ȯ ��Ÿ�
            {
                SetPreMovingAnim(out idir);
                break;
            }
            yield return null;  
        }

        // �¿�� �̵�
        float movingTime = 2f;
        float t = 0;
        float speed = 2f;
        while(t < movingTime)
        {
            Vector3 pos = new Vector3(transAtkDis * Mathf.Cos(t * speed) * idir,
                                        transform.position.y,
                                        transAtkDis * Mathf.Sin(t * speed) * idir
                                      );
            _controller.NavMovement.MoveToTarget(pos);
            t += Time.deltaTime;
            yield return null;
        }
        //������ ���� �׸��� �̵��� ��
        //�߻� ������ ����
        _controller.AgentAnimator.SetAttackTrigger(RBBossAIBrain.AttackType.Shoot, true);

        float delayTime;
        SetShootingType(out delayTime);

        yield return new WaitForSeconds(delayTime);
    }

    private void SetShootingType(out float delayTime)
    {
        delayTime = 0;
        int attackType = UnityEngine.Random.Range(0, 3);

        switch (attackType)
        {
            case 0:// Single
                SingleShoot();
                delayTime = 2f;
                break;
            case 1:// Burst
                BurstShoot();
                delayTime = 1f;
                break;
            case 2:// AutoShoot
                AutoShoot();
                delayTime = 0.5f;
                break;
        }
        _controller.AgentAnimator.SetShootingType(attackType);
    }

    private void SingleShoot()
    {
        
    }

    private void BurstShoot()
    {
        
    }

    private void AutoShoot()
    {
        
    }
}
