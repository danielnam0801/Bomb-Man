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
    int remainAttackCnt = 0;

    public override void Attack(Action endAct)
    {
        if (_phaseData.CurrentPhase == 1 || _phaseData.CurrentPhase == 0) remainAttackCnt = 0;
        else if(_phaseData.CurrentPhase == 2) remainAttackCnt = 1;
        else if(_phaseData.CurrentPhase == 3) remainAttackCnt = 2;

        isAir = false;
        isJumpStart = false;
        isFirstIn = true;
        this.endAct = endAct;
        StartCoroutine(Jump(0));
        _controller.AgentAnimator.BossJumpStartEventTrigger += JumpStart;
    }

    public void JumpStart()
    {
        JumpStartEvent?.Invoke(); //ShakeFeedfback
        isJumpStart = true;
    }

    public override void AttackAnimationEndHandle()
    {
        if(remainAttackCnt == 0)
        {
            _controller.AgentAnimator.BossJumpStartEventTrigger -= JumpStart;
            UtilMono.Instance.AddDelayCoroutine(() => endAct?.Invoke(),1f);
        }
        else
        {
            remainAttackCnt--;
            RotateSpeed += 20;
            StartCoroutine(Jump(1f));
        }
    }

    public override void AttackAnimationEventHandle()
    {
        JumpEndEvent?.Invoke();
        EffectPlayer landVFX = PoolManager.Instance.Pop(landEffect.gameObject.name) as EffectPlayer;
        landVFX.transform.position = _controller.transform.position;
        alertCircle.gameObject.SetActive(false);
        landVFX.StartPlay(4f);
    }

    public override void CancelAttack()
    {
        _controller.NavMovement.ResetNavAgent();
        remainAttackCnt = 0;
    }

    public override void PreAttack()
    {
        isAir = true;
        _controller.AgentAnimator.StopAnimator(true);
    }

    Vector3 targetPos;
    Vector3 _targetVector;
    private float RotateSpeed = 100f;
    bool isFirstIn = true;
    IEnumerator Jump(float delayTime)
    {
        isJumpStart = false;
        _controller.NavMovement.StopNavigation();
        
        #region ȸ��
        while (true)
        {
            _targetVector = _controller.TargetTrm.position - transform.position;
            _targetVector.y = 0; //Ÿ���� �ٶ󺸴� ������ �����ִ� �ż���
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
        #endregion

        if (!isFirstIn)// �̰�� ó�� ���ö� �ִϸ��̼��� ��������ֹǷ� �Ǵٽ� �����ų �ʿ䰡 ���⿡ ������
        {
            //ȸ���� ������ ��������
            _controller.AgentAnimator.Animator.
                Play("Jump", -1, 0f);
        }
        isFirstIn = false;

        yield return new WaitUntil(() => isJumpStart);
        
        Vector3 startPos = _controller.transform.position;
        targetPos = GameManager.Instance.TargetGroundPos();//���� ���� ���ϰ�
        Vector3 startControl = (targetPos - transform.position) / 4;

        alertCircle.gameObject.SetActive(true);
        
        alertCircle.position = targetPos;

        float distanceX = targetPos.x - startPos.x;
        float distanceZ = targetPos.z - startPos.z;

        float yHeight = 0;
        Vector3 cp1 = new Vector3(distanceX / 3, yHeight, distanceZ / 3) + startPos;
        Vector3 cp2 = new Vector3(distanceX / 3 * 3, yHeight, distanceZ / 3 * 3) + startPos;


        float _jumpSpeed = 0.5f;
        float _frameSpeed;

         Vector3[] _bezierPoints = DOCurve.CubicBezier.GetSegmentPointCloud(startPos,
            cp1, targetPos, cp2, 60);
        _frameSpeed = _jumpSpeed / 60;

        for(int i = 0; i < _bezierPoints.Length; i++) //�ϴ� ���ݸ� ����
        {
            yield return new WaitForSeconds(_frameSpeed);
            _controller.transform.position = _bezierPoints[i];
            if(i == _bezierPoints.Length - 1)
            {
                _controller.AgentAnimator.StopAnimator(false);
            }
        }
    }
}
