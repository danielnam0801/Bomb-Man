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
}
