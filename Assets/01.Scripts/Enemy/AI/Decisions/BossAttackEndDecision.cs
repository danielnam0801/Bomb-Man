using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class BossAttackEndDecision : AIDecision
{
    public RBBossAIBrain.AttackType skill;
    FieldInfo _skillInfo;
    public override void SetUp(Transform parentRoot)
    {
        base.SetUp(parentRoot);
        _skillInfo = typeof(RobotBossPhaseData).GetField($"Is{skill.ToString()}ing", BindingFlags.Public | BindingFlags.Instance);
    }

    public override bool MakeDecision()
    {
        return (bool)_skillInfo.GetValue(_enemyPhaseData) == false;
    }
}