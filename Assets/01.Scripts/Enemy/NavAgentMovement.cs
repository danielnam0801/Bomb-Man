using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavAgentMovement : MonoBehaviour
{
    private NavMeshAgent _navAgent;
    public NavMeshAgent NavAgent => _navAgent;

    [SerializeField]
    private float _knockBackSpeed = 4f, _gravity = -9.8f, _knockBackTime = 0.4f;
    private float _verticalVelocity;
    private Vector3 _knockBackVelocity;
    private Vector3 _movementVelocity;

    private CharacterController _characterController;
    private bool _isControllerMode = false; //현재 네브메시 모드냐 컨트롤러 모드냐 
    private float _knockBackStartTime;
    private Action EndKnockBackAction; //넉백이 끝나고 실행해야할 콜백이 있다면 여기다 저장해두었다가 실행

    private AIActionData _aiActionData;

    protected void Awake()
    {
        _navAgent = GetComponent<NavMeshAgent>();
        _characterController = GetComponent<CharacterController>();
        _aiActionData = transform.Find("AI").GetComponent<AIActionData>();
    }

    public void SetInitData(float speed)
    {
        _navAgent.speed = speed;
        _navAgent.isStopped = false;  //1번 추가
        _isControllerMode = false;  //2번 추가
    }

    public bool CheckIsArrived()
    {
        //pathPending은 경로를 계산중일때 true 경로 계산이 모두 끝난중이면 false
        if(_navAgent.pathPending == false && _navAgent.remainingDistance <= _navAgent.stoppingDistance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void StopImmediately()
    {
        _navAgent.SetDestination(transform.position);
    }

    public void MoveToTarget(Vector3 pos)
    {
        _navAgent.SetDestination(pos);
    }

    public void StopNavigation()
    {
        _navAgent.isStopped = true;
    }

    public void KnockBack(Action EndCallback = null)
    {
        //_navAgent.updatePosition = false;
        //_navAgent.updateRotation = false;

        _navAgent.enabled = false;
        _knockBackStartTime = Time.time;
        _isControllerMode = true;
        _knockBackVelocity = _aiActionData.HitNormal * -1 * _knockBackSpeed;

        EndKnockBackAction = EndCallback; //넉백끝나면 이거 실행하도록 조치하고
    }

    private bool CalculateKnockBack()
    {
        float spendTime = Time.time - _knockBackStartTime; //넉백되고 지금까지 흐른 시간
        float ratio = spendTime / _knockBackTime;
        _movementVelocity = Vector3.Lerp(_knockBackVelocity, Vector3.zero, ratio) * Time.fixedDeltaTime;

        return ratio < 1;
    }

    public void ResetNavAgent()
    {
        _characterController.enabled = true;
        _navAgent.enabled = true;
        _navAgent.isStopped = false;
    }

    private void FixedUpdate()
    {
        if (_isControllerMode == false) return;

        if(_characterController.isGrounded == false)
        {
            _verticalVelocity = _gravity * Time.fixedDeltaTime;
        }else
        {
            _verticalVelocity = _gravity * 0.3f * Time.fixedDeltaTime;
        }

        if(CalculateKnockBack()) //아직 넉백중이다.
        {
            Vector3 move = _movementVelocity + _verticalVelocity * Vector3.up;
            _characterController.Move(move);
        }else  //넉백이 끝났다.
        {
            _isControllerMode = false;
            //_characterController.enabled = false;
            EndKnockBackAction?.Invoke();

            //여기서 원래 네비메시도 켜줘야 한다.
        }
    }
}
