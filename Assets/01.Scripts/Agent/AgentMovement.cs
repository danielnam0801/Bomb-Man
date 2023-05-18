using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentMovement : MonoBehaviour
{
    [SerializeField] float _gravity = -9.8f;
    private CharacterController _characterController;
    private AgentAnimator _agentAnimator;

    private Vector3 _movementVelocity;
    public Vector3 MovementVelocity => _movementVelocity;
    private float _verticalVelocity;
    private Vector3 _inputVelocity;

    public bool IsActiveMove;
    AgentController _controller;

    private void Awake()
    {
        _controller = GetComponent<AgentController>();
        _agentAnimator = transform.Find("Visual").GetComponent<AgentAnimator>();
        _characterController = GetComponent<CharacterController>();
    }

    public void SetMovementVelocity(Vector3 value)
    {
        _inputVelocity = value;
        _movementVelocity = value;
    }

    public void SetDoJump(float jumpPower, Vector3 dir)
    {

    }

    private void CalculatePlayerMovement()
    {
        _inputVelocity.Normalize();
        
        _movementVelocity = Quaternion.Euler(0, -45f, 0) * _inputVelocity;

        _agentAnimator?.SetSpeed(_movementVelocity.sqrMagnitude);

        _movementVelocity *= _controller.CharacterData.MoveSpeed * Time.fixedDeltaTime;
        if(_movementVelocity.sqrMagnitude > 0)
        {
            transform.rotation = Quaternion.LookRotation(_movementVelocity);
        }
    }

    public void SetRotation(Vector3 target)
    {
        Vector3 dir = target - transform.position;
        dir.y = 0;
        transform.rotation = Quaternion.LookRotation(dir);
    }

    public void StopImmediately()
    {
        _movementVelocity = Vector3.zero;
        _agentAnimator?.SetSpeed(0); //이동속도 반영
    }

    private void FixedUpdate()
    {
        if (IsActiveMove)
        {
            CalculatePlayerMovement();
        }

        if (_characterController.isGrounded == false)
        {
            _verticalVelocity = _gravity * Time.fixedDeltaTime;
        }
        else
        {
            _verticalVelocity = _gravity * 0.3f * Time.fixedDeltaTime;
        }

        Vector3 move = _movementVelocity + _verticalVelocity * Vector3.up;
        _characterController.Move(move);
        _agentAnimator?.SetAirbone(!_characterController.isGrounded);
    }
}
