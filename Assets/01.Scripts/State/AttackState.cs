using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : CommonState
{
    [SerializeField] PoolableMono _dynamite;

    Dynamite dynamite;
    public override void OnEnterState()
    {
        dynamite = PoolManager.Instance.Pop(_dynamite.name) as Dynamite;
        dynamite.Shoot(_actionData.StartPos, _actionData.cp1, _actionData.cp2, _actionData.EndPos, _actionData.PointCnt);
    }   

    public override void OnExitState()
    {
        PoolManager.Instance.Push(dynamite);
    }

    public override bool OnUpdateState()
    {
        if(dynamite.dynaActive == false)
        {
            _agentController.ChangeState(Core.StateType.Normal);
        }
        return false;   
    }
}
