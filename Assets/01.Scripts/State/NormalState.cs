using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class NormalState : CommonState
{
    KeyCode debugKey = KeyCode.LeftShift;
    [SerializeField] Transform _ShootPoint;
    [SerializeField] Transform _targetPointCircle;
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
        _agentInput.OnMovementKeyPress += OnMovementHandle; //���� �� Ű�Է� ����
        _agentInput.OnAttackKeyPress += OnAttackHandle;
        _agentInput.OnJumpKeyPress += OnJumpingHandle;

        _agentMovement.IsActiveMove = true;
    }

    public override void OnExitState()
    {
        _agentMovement.StopImmediately();
        _agentInput.OnMovementKeyPress -= OnMovementHandle; //������ Ű�Է� ��������
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
        //����Ű�� ó�� ���� ���� ���ݻ��·� ��ȯ�Ǵµ� 
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
        float distanceX = endPos.x - startPos.x;
        float distanceZ = endPos.z - startPos.z;
        float Distance = Vector3.Distance(startPos, endPos);

        Vector3 cp1 = new Vector3(startPos.x + distanceX / 3, Distance/2, startPos.z + distanceZ / 3);
        Vector3 cp2 = new Vector3(startPos.x + distanceX / 3 * 2, Distance/2, startPos.z + distanceZ / 3 * 2);

        Vector3[] positions = new Vector3[RenderPositionMaxCnt + 1];
        positions = DOCurve.CubicBezier.GetSegmentPointCloud(startPoint: startPos,
            startControlPoint: cp1, endPoint: endPos, endControlPoint: cp2, RenderPositionMaxCnt);
        _lineRenderer.SetPositions(positions);

        _targetPointCircle.position = endPos;

        drawPos = false;
        
    }
}
