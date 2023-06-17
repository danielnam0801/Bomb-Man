using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RBBossAgentAnimator : AgentAnimator
{

    #region RBBoss영역
    private readonly int _jumpHash = Animator.StringToHash("jump");
    private readonly int _dashHash = Animator.StringToHash("dash");
    private readonly int _shootHash = Animator.StringToHash("shoot");
    private readonly int _runDirChangeTriggerHash = Animator.StringToHash("runDirChange");
    private readonly int _shootWaitHash = Animator.StringToHash("shootWait");

    private readonly int detectHash = Animator.StringToHash("detect");
    private readonly int _dashEndHash = Animator.StringToHash("dashEnd");
    private readonly int _walkRandHash = Animator.StringToHash("walkDirValue");
    private readonly int _shootingTypeHash = Animator.StringToHash("shootType");
    private readonly int _useSideHash = Animator.StringToHash("useWalkSideAnim");

    public void SetDashEndTrigger() => Animator.SetTrigger(_dashEndHash);
    public void ChangeRunDirTrigger() => Animator.SetTrigger(_runDirChangeTriggerHash);
    public void ShootWaitTrigger() => Animator.SetTrigger(_shootWaitHash);
    public void UseSide(bool value) => Animator.SetBool(_useSideHash, value);
    public void SetAttackTrigger(RBBossAIBrain.AttackType atkType, bool value)
    {
        if (value)
        {
            switch (atkType)
            {
                case RBBossAIBrain.AttackType.Dash:
                    Animator.SetTrigger(_dashHash);
                    break;
                case RBBossAIBrain.AttackType.SingleShoot:
                case RBBossAIBrain.AttackType.AutoShoot:
                case RBBossAIBrain.AttackType.BurstShoot:
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
                case RBBossAIBrain.AttackType.SingleShoot:
                    Animator.ResetTrigger(_shootHash);
                    break;
                case RBBossAIBrain.AttackType.Jump:
                    Animator.ResetTrigger(_jumpHash); //이전 트리거 값이 남아있지 않도록 지워져야 한다.
                    break;
            }
        }
    }
    public void SetDetectPlayer()
    {
        Animator.SetTrigger(detectHash);
    }

    public void SetRandDir(int value)
    {
        Animator.SetInteger(_walkRandHash, value);
    }

    public void SetShootingType(int value)
    {
        Animator.SetInteger(_shootingTypeHash, value);
    }

    public event Action BossJumpStartEventTrigger = null; //애니메이션이 종료될때마다 트리거 되는 이벤트임.

    public void BossJumpStartEvent() //애니메이션이 종료되면 이게 실행된다.
    {
        BossJumpStartEventTrigger?.Invoke();
    }




    #endregion

}
