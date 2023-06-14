using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackEndDecision : AIDecision
{
    public override bool MakeDecision()
    {
        return _aIActionData.IsAttacking == false;
    }
}