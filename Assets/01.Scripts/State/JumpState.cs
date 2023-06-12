using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class JumpState : CommonState
{
    [SerializeField]
    Transform bombCheckPoint;
    [SerializeField]
    private float _animationThreshhold = 0.1f;
    private float _timer = 0;

    public UnityEvent playingLandingEvent;

    public override void OnEnterState()
    {
        _agentAnimator.OnAnimationEndTrigger += JumpEndHandle;
        _agentMovement.IsActiveMove = false;

        //debuging용 코드 나중에 폭탄터질때만 점프되는걸로 바꿔야 함
        Jump();

    }

    private void Jump()
    {
        _actionData.JumpCall = false;
        _actionData.JumpCnt++;

        Vector3 playerPos = GameManager.Instance.ReturnVector3PosXZ(_agentController.transform.position);
        Vector3 bombPos = GameManager.Instance.ReturnVector3PosXZ(_actionData.DynaBombPoint);

        float distance = Vector3.Distance(playerPos, bombPos);
        Vector3 dir = (playerPos - bombPos).normalized;
        Vector3 shootDir = dir / distance;
        shootDir += Vector3.up / 2;

        Debug.Log($"Dir '{shootDir.x} : {shootDir.y} : {shootDir.z}'");
        //_agentMovement.SetRotation(dir + _agentController.transform.position);

        _agentMovement.StopImmediately();
        _agentMovement.SetDoJump(_agentController.CharacterData.JumpPower, shootDir);
    }

    private void JumpEndHandle()
    {
        playingLandingEvent.Invoke();
        //_agentController.ChangeState(Core.StateType.Normal);
    }

    public override void OnExitState()
    {
        _actionData.JumpCnt = 0;
        _agentAnimator.OnAnimationEndTrigger -= JumpEndHandle;
    }

    public override bool OnUpdateState()
    {
        if (_actionData.JumpCall) Jump();
        return false;
    }
}
