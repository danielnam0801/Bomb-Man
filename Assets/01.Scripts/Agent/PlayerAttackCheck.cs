using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackCheck : MonoBehaviour
{
    [SerializeField] Transform _ShootPoint;
    [SerializeField] Transform _targetPointCircle;
    
    LineRenderer _lineRenderer;
    AgentInput _agentInput;
    ActionData _actionData;

    public int RenderPositionMaxCnt = 20;
    
    bool drawPos = false;

    float currentScrollValue = 0f;
    float minScrollValue = -2f;
    float maxScrollValue = 2f;

    float attackCooltime = 0.75f;
    bool canAttack = true;
    private void Awake()
    {
        _agentInput = GetComponent<AgentInput>();
        _actionData = GetComponent<ActionData>();
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.positionCount = RenderPositionMaxCnt;
    }

    private void Start()
    {
        _actionData.PointCnt = RenderPositionMaxCnt;
    }


    float t = 0f;
    private void Update()
    {
        if (t > attackCooltime)
        {
            t = 0;
            canAttack = true;
        }
        if (_actionData.CanCheckAttack)
        {
            if (Input.mouseScrollDelta.y != 0)
            {
                currentScrollValue = Input.mouseScrollDelta.y / 2;
            }
            currentScrollValue = Mathf.Clamp(currentScrollValue, minScrollValue, maxScrollValue);
            Debug.Log("CurrentScrollValue : " + currentScrollValue);
            if (Input.GetMouseButton(0))
            {
                _lineRenderer.positionCount = RenderPositionMaxCnt;
                _targetPointCircle.gameObject.SetActive(true);
                DrawTrajectory();
                //Debug.Log("INPUTing");
            }
            else if (Input.GetMouseButtonUp(0))
            {
                //Debug.Log("InputUp");
                if (canAttack)
                {
                    canAttack = false;
                    _agentInput.SetPlayAttackHandle();
                }
            }
            else
            {
                _lineRenderer.positionCount = 0;
                _targetPointCircle.gameObject.SetActive(false);
            }
        }
        t += Time.deltaTime;
    }

    private void DrawTrajectory()
    {
        drawPos = true;

        Vector3 startPos = _ShootPoint.position;
        Vector3 endPos = _agentInput.GetMouseWorldPosition();
        float distanceX = endPos.x - startPos.x;
        float distanceZ = endPos.z - startPos.z;
        float Distance = Vector3.Distance(startPos, endPos);

        Debug.Log("y : " + Mathf.Clamp(Distance / 4 * currentScrollValue, 0.2f, 4f));
        //Debug.Log();

        Vector3 cp1 = new Vector3(startPos.x + distanceX / 3, startPos.y + Mathf.Clamp(Distance / 4 * currentScrollValue, 0.2f, 4f), startPos.z + distanceZ / 3);
        Vector3 cp2 = new Vector3(startPos.x + distanceX / 3 * 2, startPos.y + Mathf.Clamp(Distance / 4 * currentScrollValue, 0.2f, 4f), startPos.z + distanceZ / 3 * 2);

        Vector3[] positions = new Vector3[RenderPositionMaxCnt + 1];
        positions = DOCurve.CubicBezier.GetSegmentPointCloud(startPoint: startPos,
            startControlPoint: cp1, endPoint: endPos, endControlPoint: cp2, RenderPositionMaxCnt);

        bool isHit = false;
        int i = 0;
        for (i = 0; i < positions.Length - 1; i++)
        {
            RaycastHit ray;
            if (Physics.Raycast(positions[i], (positions[i + 1] - positions[i]).normalized, out ray,
                Vector3.Distance(positions[i], positions[i + 1]), 1 << LayerMask.NameToLayer("Obstacle")))
            {
                isHit = true;
                endPos = ray.point;
                break;
            }
        }
        if (isHit) {
            _lineRenderer.positionCount = i + 1;
        }

        _actionData.StartPos = startPos;
        _actionData.EndPos = endPos;
        _actionData.cp1 = cp1;
        _actionData.cp2 = cp2;

        _lineRenderer.SetPositions(positions);
        _targetPointCircle.position = endPos;


        drawPos = false;

    }

    private float attackInputDelay = 0.2f;

    IEnumerator AttackDelayCor()
    {
        yield return new WaitForSeconds(attackInputDelay);
        _agentInput.CanCheckAttackInput = true;
    }

    private void OnDisable()
    {
        _lineRenderer.enabled = false;
        if (_targetPointCircle.gameObject.activeSelf) _targetPointCircle.gameObject.SetActive(false);
    }
}
