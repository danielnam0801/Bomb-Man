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
        _actionData.IsGround = false;
        _actionData.IsJumping = true;
       
        //debuging용 코드 나중에 폭탄터질때만 점프되는걸로 바꿔야 함
        Jump();

    }

    private void Jump()
    {
        _actionData.JumpCall = false;
        _actionData.JumpCnt++;

        Vector3 playerPos = GameManager.Instance.ReturnVector3PosXZ(_agentController.transform.position);
        Vector3 bombPos = GameManager.Instance.ReturnVector3PosXZ(_actionData.DynaBombPoint);

        playerPos = _agentController.transform.position;
        bombPos = _actionData.DynaBombPoint;

        float distance = Vector3.Distance(playerPos, bombPos) / 2; // 2는 보간치
        Vector3 dir = (playerPos - bombPos).normalized;
        Vector3 shootDir = dir + Vector3.up;
        shootDir /= distance;

        shootDir = new Vector3(Mathf.Clamp(shootDir.x, -1f, 1f), 
            Mathf.Clamp(shootDir.y, -1f, 1f), Mathf.Clamp(shootDir.z, -1f, 1f));

        Debug.Log($"Dir '{shootDir.x} : {shootDir.y} : {shootDir.z}'");
        //_agentMovement.SetRotation(dir + _agentController.transform.position);

        _agentMovement.StopImmediately();
        _agentMovement.SetExplosion(_agentController.CharacterData.JumpPower, shootDir);
    }

    private void JumpEndHandle()
    {
        _agentController.ChangeState(Core.StateType.Normal);
    }

    

    public override void OnExitState()
    {
        _agentAnimator.OnAnimationEndTrigger -= JumpEndHandle;
        _actionData.JumpCnt = 0;
        _actionData.IsJumping = false;
    }

    public override bool OnUpdateState()
    {
        if (_actionData.IsGround) JumpEndHandle();
        if (_actionData.JumpCall) Jump();
        return false;
    }
}
