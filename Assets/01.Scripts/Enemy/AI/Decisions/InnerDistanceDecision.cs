using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InnerDistanceDecision : AIDecision
{
    [SerializeField]
    private float _distance = 5f;

    public override bool MakeDecision()
    {
        if (_enemyController.TargetTrm == null) return true;

        float distance = Vector3.Distance(_enemyController.TargetTrm.position, transform.position);

        if(distance < _distance)  //�þ� ������ ���Դٸ�
        {
            _aIActionData.LastSpotPoint = _enemyController.TargetTrm.position;
            _aIActionData.TargetSpotted = true;
        }else
        {
            _aIActionData.TargetSpotted = false;
        }
        return _aIActionData.TargetSpotted;
    }
}
