using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalState : CommonState
{
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
        return false;
    }
}
