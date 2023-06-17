using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BossAttackAIState : CommonAIState
{
    public RBBossAIBrain.AttackType thisAttackType;

    protected Vector3 _targetVector;
    protected bool _isActive = false;

    private EnemyDataSO _dataSO;

    private RbBossAttack thisAttack;

    public UnityEvent AttackAnimationEndEvent;
    public UnityEvent AttackCollisionEvent;

    public override void SetUp(Transform agentRoot)
    {
        base.SetUp(agentRoot);
        _dataSO = _enemyController.EnemyData;
    }

    private void Start()
    {
        thisAttack = _aiBrain.GetAttack(thisAttackType);
    }

    public override void OnEnterState()
    {
        _isActive = true;
        _aiActionData.canMove = false;

        _enemyController.AgentAnimator.UseSide(false);

        _enemyController.AgentAnimator.OnAnimationEndTrigger += AttackAnimationEndHandle;
        _enemyController.AgentAnimator.OnAnimationEventTrigger += thisAttack.AttackAnimationEventHandle;
        _enemyController.AgentAnimator.OnPreAnimationEventTrigger += PreAttackHandle;

        _enemyController.NavMovement.StopImmediately();
        _aiBrain.Attack(thisAttackType); // ���н� �ٷ� �ؽ�Ʈ ������Ʈ�� �Ѿ����
    }


    public override void OnExitState()
    {

        _enemyController.AgentAnimator.OnAnimationEndTrigger -= AttackAnimationEndHandle;
        _enemyController.AgentAnimator.OnAnimationEventTrigger -= thisAttack.AttackAnimationEventHandle;
        _enemyController.AgentAnimator.OnPreAnimationEventTrigger -= PreAttackHandle;

        _enemyController.AgentAnimator.SetAttackState(false);
        _enemyController.AgentAnimator.SetAttackTrigger(thisAttackType, false);

        thisAttack.CancelAttack();  //������ �����ϴ��� ��� ��Ű��
        
        _isActive = false;
        _aiActionData.IsAttacking = false;
    }

    private void PreAttackHandle()
    {
        thisAttack.PreAttack();
    }

    private void AttackAnimationEndHandle()
    {
        //�ִϸ��̼��� ������ ���� ���� ��
        AttackAnimationEndEvent?.Invoke();
        _enemyController.NavMovement.ResetSpeed();
    }

    private void SetTarget()
    {
        _targetVector = _enemyController.TargetTrm.position - transform.position;
        _targetVector.y = 0; //Ÿ���� �ٶ󺸴� ������ �����ִ� �ż���
    }

    public override bool OnUpdateState()
    {
        if(base.OnUpdateState())
        {
            return true;
        }

        if(_aiActionData.IsAttacking == false && _isActive)  //��Ƽ��
        {
            SetTarget(); //���� ��ġ�� �����ؼ� targetVector�� ������ְ� 

            //_enemyController.transform.rotation = Quaternion.LookRotation(_targetVector);
            Vector3 currentFrontVector = transform.forward;
            float angle = Vector3.Angle(currentFrontVector, _targetVector);

            if(angle >= 10f)
            {
                //�������ϰ�
                Vector3 result = Vector3.Cross(currentFrontVector, _targetVector);

                float sign = result.y > 0 ? 1 : -1;
                _enemyController.transform.rotation
                    = Quaternion.Euler(0, sign * _dataSO.RotateSpeed * Time.deltaTime, 0) 
                        * _enemyController.transform.rotation;
            }else //������ 10���� ���Դٸ�
            {
                _aiActionData.IsAttacking = true;
                _enemyController.AgentAnimator.SetAttackState(true);
                _enemyController.AgentAnimator.SetAttackTrigger(thisAttackType, true);
            }            
        }

        return false;
    }

}
