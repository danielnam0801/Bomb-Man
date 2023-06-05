using System;
using UnityEngine;
using static Core.Define;

public class AgentInput : MonoBehaviour
{
    [SerializeField]
    private LayerMask _whatIsGround;

    public event Action<Vector3> OnMovementKeyPress = null;
    public event Action OnAttackKeyPress = null; //공격키 이벤트
    public event Action OnJumpKeyPress = null; //회피키 이벤트

    private Vector3 _directionInput;
    public Vector3 DirectionInput => _directionInput;

    void Update()
    {
        UpdateMoveInput();
        UpdateAttackInput();
        UpdateJumpInput();
    }

    private void UpdateJumpInput()
    {
        if (Input.GetButtonDown("Jump"))
        {
            OnJumpKeyPress?.Invoke();
        }
    }

    private void UpdateAttackInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnAttackKeyPress?.Invoke();
        }
    }

    private void UpdateMoveInput()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        _directionInput = new Vector3(h, 0, v);
        OnMovementKeyPress?.Invoke(_directionInput);
    }

    public Vector3 GetCurrentInputDirection()
    {
        return Quaternion.Euler(0, -45f, 0) * _directionInput.normalized;
    }

    public Vector3 GetMouseWorldPosition()
    {
        Ray ray = MainCam.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        bool result = Physics.Raycast(ray, out hit, MainCam.farClipPlane,
        _whatIsGround);
        if (result)
        {
            Debug.Log("Hit");
            return hit.point;
        }
        else
        {
            return Vector3.zero;
        }
    }
}
