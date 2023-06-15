using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RbBossJumpAttack : RbBossAttack
{
    public override void Attack(Action endAct)
    {
        endAct?.Invoke();
    }

    public override void AttackAnimationEndHandle()
    {
        
    }

    public override void AttackAnimationEventHandle()
    {
        
    }

    public override void CancelAttack()
    {

    }

    public override void PreAttack()
    {

    }
}
