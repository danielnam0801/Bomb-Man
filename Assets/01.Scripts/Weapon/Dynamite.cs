using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

public class Dynamite : PoolableMono
{
    private float bombDelay = 1f;
    private float positionMoveSpeed;

    Vector3[] positions;
    Vector3[] secondPositions;

    public event Action ThrowStart;
    public event Action FallEnd;
    public event Action BombAct;

    Collider col;
    Rigidbody rb;

    public bool dynaActive = true;
    

    [SerializeField] EffectPlayer bombEffect;

    private void Awake()
    {
        col = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
    }

    public override void Init()
    {
        rb.velocity = Vector3.zero;
        dynaActive = true;
        BombAct += () => dynaActive = false;
        BombAct += () => VFXManager.Instance.SpawningEffect(bombEffect, transform.position, Quaternion.identity);
        BombAct += () => Explode();
        //ThrowStart += () => col.isTrigger = true;
        //FallEnd += () => col.isTrigger = false;
    }

    private void Explode()
    {
        PlayerManager.Instance.ActionData.JumpCall = true;
        PlayerManager.Instance.ActionData.DynaBombPoint = transform.position;
        PoolManager.Instance.Push(this);
    }

    public void Shoot(Vector3 startPos, Vector3 cp1, Vector3 cp2, Vector3 endPos, int pointsCnt)
    {
        transform.position = startPos;
        positions = new Vector3[pointsCnt + 1];
        positions = DOCurve.CubicBezier.GetSegmentPointCloud(startPoint: startPos,
            startControlPoint: cp1, endPoint: endPos, endControlPoint: cp2, pointsCnt);

        positionMoveSpeed = Vector3.Distance(startPos, endPos);
        StartCoroutine(nameof(Shooting));
    }

    IEnumerator Shooting()
    {
        ThrowStart?.Invoke();

        for (int i = 0; i < positions.Length - 2; i++)
        {
            rb.velocity = (positions[i + 1] - positions[i]) * 50f;
            yield return new WaitForSeconds(0.0125f);
        }

        FallEnd?.Invoke();
        yield return new WaitForSeconds(bombDelay);

        BombAct?.Invoke();
    }


    private void OnDisable()
    {
        BombAct = null;
        ThrowStart = null;
        FallEnd = null;
        dynaActive = false;
        transform.position = Vector3.zero;  
    }

    private void OnCollisionEnter(Collision collision)
    {
        //BombAct?.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        //BombAct?.Invoke();
    }
}
