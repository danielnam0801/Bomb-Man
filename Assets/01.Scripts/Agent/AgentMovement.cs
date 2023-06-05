using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentMovement : MonoBehaviour
{
    [SerializeField] float _gravity = -9.8f;
    private AgentAnimator _agentAnimator;

    private Vector3 _movementVelocity;
    public Vector3 MovementVelocity => _movementVelocity;
    private float _verticalVelocity;
    private Vector3 _inputVelocity;

    public bool IsActiveMove;
    public bool IsJumping;
    AgentController _agentController;
    Rigidbody rb;

    private void Awake()
    {
        _agentController = GetComponent<AgentController>();
        _agentAnimator = transform.Find("Visual").GetComponent<AgentAnimator>();
        rb = GetComponent<Rigidbody>();
    }

    public void SetMovementVelocity(Vector3 value)
    {
        _inputVelocity = value;
        _movementVelocity = value;
    }

    public void SetDoJump(float jumpPower, Vector3 dir)
    {
        rb.AddForce(dir * jumpPower, ForceMode.Impulse);
        _agentController.ChangeState(Core.StateType.Normal);
    }

    private void CalculatePlayerMovement()
    {
        _inputVelocity.Normalize();
        
        _movementVelocity = Quaternion.Euler(0, -45f, 0) * _inputVelocity;

        _agentAnimator?.SetSpeed(_movementVelocity.sqrMagnitude);

        _movementVelocity *= _agentController.CharacterData.MoveSpeed * Time.fixedDeltaTime;
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

    public Vector3 ResetOrientation(Vector3 dir)
    {
        return Quaternion.Euler(0, -45f, 0) * dir;
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
        
        Vector3 move = new Vector3(_movementVelocity.x, rb.velocity.y, _movementVelocity.z);
        rb.velocity = move;

        //_agentAnimator?.SetAirbone(!_characterController.isGrounded);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            IsJumping = false;
        }
    }
}
