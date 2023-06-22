using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

public class Dynamite : PoolableMono
{
    private float bombDelay = 1f;
    public float BombDelay { get => bombDelay; set { bombDelay = value; } }
    private float positionMoveSpeed;

    Vector3[] positions;
    Vector3[] secondPositions;

    public event Action ThrowStart;
    public event Action FallEnd;
    public event Action BombAct;

    Collider col;
    Rigidbody rb;

    public bool dynaActive = true;
    public bool isEnemyBomb = false;

    [SerializeField] EffectPlayer bombEffect;

    float playerBombRad = 1f, bossBombRad = 1.5f, bossHitCriRad = 1.3f;
    float fontSize;

    public int Damage { get; set; }

    private void Awake()
    {
        col = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
    }

    public override void Init()
    {
        rb.velocity = Vector3.zero;
        dynaActive = true;
        isHit = false;
        BombAct += () => dynaActive = false;
        BombAct += () => VFXManager.Instance.SpawningEffect(bombEffect, transform.position, Quaternion.identity);
        BombAct += () => Explode();
    }

    private void Explode()
    {

        if (!isEnemyBomb)
        {
            float radius = PlayerManager.Instance.
                AgentController.CharacterData.CanApplyBombRadius;

            Collider[] playerBomb = Physics.OverlapSphere(transform.position, radius);

            foreach (var a in playerBomb)
            {
                if (a != null)
                {
                    if (a.gameObject.CompareTag("Player"))
                    {
                        PlayerManager.Instance.ActionData.JumpCall = true;
                        PlayerManager.Instance.ActionData.DynaBombPoint = transform.position;
                    }
                    else if(a.gameObject.CompareTag("Enemy"))
                    {
                        int fontSize = 10;
                        Color fontColor = Color.white;

                        IDamageable damageable;
                        if (a.transform.TryGetComponent<IDamageable>(out damageable))
                        {
                            float distance = Vector3.Distance(transform.position, a.transform.position);
                            if (distance < bossHitCriRad)
                            {
                                Damage = Damage * 2;
                                fontSize = 15;
                                fontColor = Color.red;
                            }
                            else
                            {
                                Damage -= (int)(2 * (distance - bossHitCriRad));
                                fontSize = 10;
                                fontColor = Color.white;
                            }
                            damageable.OnDamage(Damage, a.transform.position, transform.position - a.transform.position);
                        }

                        PopupText text = PoolManager.Instance.Pop("PopupText") as PopupText;
                        text.StartPopup(text: Damage.ToString(), pos: transform.position + new Vector3(0, 0.5f),
                                        fontSize: fontSize, color: fontColor);
                    }
                }
            }
        }
        else { // 利捞 金 气藕老锭
            float radius = this.bossBombRad;
            Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
            foreach(var a in colliders)
            {
                if (a != null)
                {
                    if (a.CompareTag("Player"))
                    {
                        IDamageable damageable;
                        if(a.transform.TryGetComponent<IDamageable>(out damageable))
                        {
                            float distance = Vector3.Distance(transform.position, a.transform.position);
                            Damage = (int)((float)Damage / distance);
                            damageable.OnDamage(Damage, a.transform.position, transform.position - a.transform.position);
                        }
                    }
                }
            }
            
        }

        PoolManager.Instance.Push(this);
    }

    public void Shoot(Vector3 startPos, Vector3 cp1, Vector3 cp2, Vector3 endPos, int pointsCnt, float speed = 50f)
    {
        transform.position = startPos;
        positions = new Vector3[pointsCnt + 1];
        positions = DOCurve.CubicBezier.GetSegmentPointCloud(startPoint: startPos,
            startControlPoint: cp1, endPoint: endPos, endControlPoint: cp2, pointsCnt);

        positionMoveSpeed = Vector3.Distance(startPos, endPos);
        StartCoroutine(Shooting(speed));
    }

    private bool isHit = false;

    IEnumerator Shooting(float speed)
    {
        ThrowStart?.Invoke();
        for (int i = 0; i < positions.Length - 2; i++)
        {
            if (isHit) break;
            rb.velocity = (positions[i + 1] - positions[i]) * speed;
            yield return new WaitForSeconds(0.0125f);
        }
    
        FallEnd?.Invoke();
        yield return new WaitForSeconds(BombDelay);

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
        isHit = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        //BombAct?.Invoke();
    }
}
