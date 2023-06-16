using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanAttackCheckDecision : AIDecision
{
    public RBBossAIBrain.AttackType skill;

    public override bool MakeDecision()
    {
        return _enemyController.AIBrain.CheckAttack(skill);
    }
}
