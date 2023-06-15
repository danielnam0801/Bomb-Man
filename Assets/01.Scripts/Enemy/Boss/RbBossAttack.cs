using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RbBossAttack : EnemyAttack
{
    protected AIActionData _actionData;
    protected RobotBossPhaseData _phaseData;
    protected BossController _controller;
    public float Damage;

    protected Action endAct;

    protected virtual void Awake()
    {
        _controller = transform.parent.GetComponent<BossController>();
        _actionData = transform.parent.Find("AI").GetComponent<AIActionData>();
        _phaseData = transform.parent.Find("AI").GetComponent<RobotBossPhaseData>();
    }

    public abstract void Attack(Action act);
    public abstract void PreAttack();
    public abstract void CancelAttack();
    public abstract void AttackAnimationEndHandle();
    public abstract void AttackAnimationEventHandle();
}
