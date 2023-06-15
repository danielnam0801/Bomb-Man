using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentAnimator : MonoBehaviour
{
    private readonly int _speedHash = Animator.StringToHash("speed");
    private readonly int _isAirboneHash = Animator.StringToHash("is_airbone");

    private readonly int _attackHash = Animator.StringToHash("attack");
    private readonly int _isAttackHash = Animator.StringToHash("is_attack");

    private readonly int _isDeadHash = Animator.StringToHash("is_dead");
    private readonly int _deadTriggerHash = Animator.StringToHash("dead");

    private readonly int _hurtTriggerHash = Animator.StringToHash("hurt");
    private readonly int _isHitHash = Animator.StringToHash("is_hit");

    private readonly int detectHash = Animator.StringToHash("detect");

    private Animator _animator;
    public Animator Animator => _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void SetAttackState(bool state)
    {
        _animator.SetBool(_isAttackHash, state);
    }

    public virtual void SetAttackTrigger(bool value)
    {
        if (value)
            _animator.SetTrigger(_attackHash);
        else
            _animator.ResetTrigger(_attackHash); //이전 트리거 값이 남아있지 않도록 지워져야 한다.
    }

    public void SetSpeed(float value)
    {
        _animator.SetFloat(_speedHash, value);
    }

    public void SetAirbone(bool value)
    {
        _animator.SetBool(_isAirboneHash, value);
    }

    public void SetIsHit(bool value)
    {
        _animator.SetBool(_isHitHash, value);
    }


    public void SetHurtTrigger(bool value)
    {
        if (value)
            _animator.SetTrigger(_hurtTriggerHash);
        else
            _animator.ResetTrigger(_hurtTriggerHash);
    }
   
    public void StopAnimator(bool value)
    {
        _animator.speed = value ? 0 : 1;
    }

    public void SetDead()
    {
        _animator.SetTrigger(_deadTriggerHash);
        _animator.SetBool(_isDeadHash, true);
    }

    public void SetDetectPlayer()
    {
        Animator.SetTrigger(detectHash);
    }


    #region 애니메이션 이벤트 영역
    public event Action OnAnimationEndTrigger = null; //애니메이션이 종료될때마다 트리거 되는 이벤트임.
    public event Action OnAnimationEventTrigger = null; //애니메이션 내의 이벤트 트리거
    public event Action OnPreAnimationEventTrigger = null; //전조 애니메이션 트리거

    public void OnAnimationEnd() //애니메이션이 종료되면 이게 실행된다.
    {
        OnAnimationEndTrigger?.Invoke();
    }

    public void OnAnimationEvent()
    {
        OnAnimationEventTrigger?.Invoke();
    }

    public void OnPreAnimationEvent()
    {
        OnPreAnimationEventTrigger?.Invoke();
    }
    #endregion
    #region RBBoss영역
    private readonly int _jumpHash = Animator.StringToHash("jump");
    private readonly int _dashHash = Animator.StringToHash("dash");
    private readonly int _dashEndHash = Animator.StringToHash("dashEnd");
    private readonly int _shootHash = Animator.StringToHash("shoot");

    public void SetDashEndTrigger() => Animator.SetTrigger(_dashEndHash);
    public void SetAttackTrigger(RBBossAIBrain.AttackType atkType, bool value)
    {
        if (value)
        {
            switch (atkType)
            {
                case RBBossAIBrain.AttackType.Dash:
                    Animator.SetTrigger(_dashHash);
                    break;
                case RBBossAIBrain.AttackType.Shoot:
                    Animator.SetTrigger(_shootHash);
                    break;
                case RBBossAIBrain.AttackType.Jump:
                    Animator.SetTrigger(_jumpHash);
                    break;
            }
        }
        else
        {
            switch (atkType)
            {
                case RBBossAIBrain.AttackType.Dash:
                    Animator.ResetTrigger(_dashHash);
                    break;
                case RBBossAIBrain.AttackType.Shoot:
                    Animator.ResetTrigger(_shootHash);
                    break;
                case RBBossAIBrain.AttackType.Jump:
                    Animator.ResetTrigger(_jumpHash); //이전 트리거 값이 남아있지 않도록 지워져야 한다.
                    break;
            }
        }
    }
    #endregion

}
