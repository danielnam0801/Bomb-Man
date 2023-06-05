using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class NormalState : CommonState
{
    KeyCode debugKey = KeyCode.LeftShift;
    [SerializeField] Transform _ShootPoint;
    LineRenderer _lineRenderer;
    public int RenderPositionMaxCnt = 60;

    bool drawPos = false;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();    
        _lineRenderer.positionCount = RenderPositionMaxCnt;
    }

    public override void OnEnterState()
    {
        _agentMovement.StopImmediately();
        _agentInput.OnMovementKeyPress += OnMovementHandle; //들어올 때 키입력 구독
        _agentInput.OnAttackKeyPress += OnAttackHandle;
        _agentInput.OnJumpKeyPress += OnJumpingHandle;

        _agentMovement.IsActiveMove = true;
    }

    public override void OnExitState()
    {
        _agentMovement.StopImmediately();
        _agentInput.OnMovementKeyPress -= OnMovementHandle; //나갈때 키입력 구독해제
        _agentInput.OnAttackKeyPress -= OnAttackHandle;
        _agentInput.OnJumpKeyPress -= OnJumpingHandle;

        _agentMovement.IsActiveMove = false;
    }

    private void OnMovementHandle(Vector3 obj)
    {
        _agentMovement.SetMovementVelocity(obj);
    }

    private void OnJumpingHandle()
    {
        _agentController.ChangeState(Core.StateType.Jump);
    }

    private void OnAttackHandle()
    {
        //공격키를 처음 누른 순간 공격상태로 전환되는데 
        _agentMovement.SetRotation(_agentInput.GetMouseWorldPosition());
        _agentController.ChangeState(Core.StateType.Attack);
    }

    public override bool OnUpdateState()
    {
        if (Input.GetKey(debugKey) && drawPos == false)
        {
            DrawTrajectory();
        }
        return false;
    }

    private void DrawTrajectory()
    {
        drawPos = true;

        Vector3 startPos = _ShootPoint.position;
        Vector3 endPos = _agentInput.GetMouseWorldPosition();

        Vector3[] positions = new Vector3[RenderPositionMaxCnt + 1];

        for(int i = 0; i < RenderPositionMaxCnt; i++)
        {
            float t = i / (RenderPositionMaxCnt - 1);
            Vector3 pos = Vector3.Slerp(startPos, endPos, t);
            Debug.Log("Pos " + t);
            positions[i] = pos;
        }


        _lineRenderer.SetPositions(positions);

        drawPos = false;
        
    }
}
