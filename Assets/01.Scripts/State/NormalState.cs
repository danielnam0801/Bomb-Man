using UnityEngine;

public class NormalState : CommonState
{
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
        _agentMovement.SetRotation(_agentInput.GetMouseWorldPosition());
        CreateAndShoot();
    }

    private void CreateAndShoot()
    {
        Dynamite dynamite = PoolManager.Instance.
            Pop(BombManager.Instance.SelectBomb()) as Dynamite;
        dynamite.Shoot(_actionData.StartPos, _actionData.cp1, _actionData.cp2, _actionData.EndPos, _actionData.PointCnt);
    }


    public override bool OnUpdateState()
    {
        if(_actionData.JumpCall == true)
        {
            OnJumpingHandle();
        }
        return false;
    }

    
}
