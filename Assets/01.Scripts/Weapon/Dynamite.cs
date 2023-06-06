using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

public class Dynamite : PoolableMono
{
    private float bombDelay = 1f;
    private int pointsCount = 60;
    Vector3[] positions;

    public event Action ThrowStart;
    public event Action FallEnd;
    public event Action BombAct;

    [SerializeField] ParticleSystem bombParticle;

    public override void Init()
    {
        
    }
                
    public void Shoot(Vector3 startPos, Vector3 cp1, Vector3 cp2, Vector3 endPos)
    {
        positions = new Vector3[pointsCount + 1];
        positions = DOCurve.CubicBezier.GetSegmentPointCloud(startPoint: startPos,
            startControlPoint: cp1, endPoint: endPos, endControlPoint: cp2, pointsCount);

        StartCoroutine(nameof(Shooting));
    }

    IEnumerator Shooting()
    {
        ThrowStart?.Invoke();
        for(int i = 0; i < positions.Length; i++)
        {
            transform.position = positions[i];
            yield return null;
        }
        
        FallEnd?.Invoke();
        yield return new WaitForSeconds(bombDelay);

        BombAct?.Invoke();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if()
    }

}
