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

        if(distance < _distance)  //시야 안으로 들어왔다면
        {
            _aIActionData.LastSpotPoint = _enemyController.TargetTrm.position;
            _aIActionData.TargetSpotted = true;
        }else
        {
            _aIActionData.TargetSpotted = false;
        }
        return _aIActionData.TargetSpotted;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if(UnityEditor.Selection.activeObject == gameObject)
        {
            Color old = Gizmos.color;
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, _distance);
            Gizmos.color = old;
        }
    }
#endif
}
