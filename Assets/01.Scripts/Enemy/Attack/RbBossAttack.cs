using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RbBossAttack : MonoBehaviour
{
    protected AIActionData _actionData;
    protected RobotBossPhaseData _phaseData;

    protected virtual void Awake()
    {
        _actionData = transform.Find("AI").GetComponent<AIActionData>();
        _phaseData = transform.Find("AI").GetComponent<RobotBossPhaseData>();
    }

    public abstract void Attack(Action EndAct);
    public abstract void PreAttack();
    public abstract void CancelAttack();
}
