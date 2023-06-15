using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public abstract class AIDecision : MonoBehaviour
{
    protected AIActionData _aIActionData;
    protected BossController _enemyController;
    protected RobotBossPhaseData _enemyPhaseData;

    public bool IsReverse = false;

    public virtual void SetUp(Transform parentRoot)
    {
        _enemyController = parentRoot.GetComponent<BossController>();
        _aIActionData = parentRoot.Find("AI").GetComponent<AIActionData>();
        _enemyPhaseData = parentRoot.Find("AI").GetComponent<RobotBossPhaseData>();
    }

    public abstract bool MakeDecision();
}
