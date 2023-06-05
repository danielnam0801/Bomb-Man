using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class JumpState : CommonState
{
    [SerializeField]
    private float _animationThreshhold = 0.1f;
    private float _timer = 0;

    public UnityEvent playingLandingEvent;

    public override void OnEnterState()
    {
        _agentAnimator.OnAnimationEndTrigger += JumpEndHandle;
        _agentMovement.IsActiveMove = false;

        //debuging용 코드 나중에 폭탄터질때만 점프되는걸로 바꿔야 함
        Vector3 dir = _agentInput.GetCurrentInputDirection();
        if(dir.magnitude < 0.1f)
        {
            dir = _agentController.transform.forward + _agentController.transform.up;
        }
        _agentMovement.SetRotation(dir + _agentController.transform.position);

        _agentMovement.StopImmediately();
        _agentMovement.SetDoJump(_agentController.CharacterData.JumpPower, dir);
        
    }

    private void JumpEndHandle()
    {
        playingLandingEvent.Invoke();
        //_agentController.ChangeState(Core.StateType.Normal);
    }

    public override void OnExitState()
    {
        _agentAnimator.OnAnimationEndTrigger -= JumpEndHandle;
    }

    public override bool OnUpdateState()
    {
        return false;
    }
}
