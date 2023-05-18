using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : CommonState
{
    [SerializeField]
    private float _jumpPower = 5f, _animationThreshhold = 0.1f;
    private float _timer = 0;

    public override void OnEnterState()
    {
        _agentAnimator.OnAnimationEndTrigger += JumpEndHandle;
        _agentMovement.IsActiveMove = false;

        //debuging�� �ڵ� ���߿� ��ź�������� �����Ǵ°ɷ� �ٲ�� ��
        Vector3 dir = _agentInput.GetCurrentInputDirection();
        if(dir.magnitude < 0.1f)
        {
            dir = _agentController.transform.forward;
        }
        _agentMovement.SetRotation(dir + _agentController.transform.position);

        _agentMovement.StopImmediately();
        _agentMovement.SetMovementVelocity();
        
    }

    public override void OnExitState()
    {
        
    }

    public override bool OnUpdateState()
    {
        return false;
    }
}
