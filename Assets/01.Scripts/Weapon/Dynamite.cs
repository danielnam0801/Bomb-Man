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

    float playerBombRad = 1f, bossBombRad = 1.5f, bossHitCriRad = 0.7f; float playerHitCriRad = 0.3f;

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
        //Collider[] colliders = Physics.OverlapSphere(transform.position, radius: 8f);

        //foreach(Collider nearby in colliders)
        //{
        //    Rigidbody rigg = nearby.GetComponent<Rigidbody>();
        //    if(rigg != null)
        //    {
        //        rigg.AddExplosionForce(4000f, transform.position, 8f);
        //    }
        //}

        if (!isEnemyBomb)
        {
            float radius = PlayerManager.Instance.
                AgentController.CharacterData.CanApplyBombRadius;

            Collider[] playerBomb = Physics.OverlapSphere(transform.position, radius);

            foreach (var a in playerBomb)
            {
                if (a.gameObject.CompareTag("Player"))
                {
                    PlayerManager.Instance.ActionData.JumpCall = true;
                    PlayerManager.Instance.ActionData.DynaBombPoint = transform.position;
                }
                else // ÀûÃ¼Å©
                {
                    if (a != null)
                    {
                        IDamageable damageable;
                        if (a.transform.TryGetComponent<IDamageable>(out damageable))
                        {
                            float distance = Vector3.Distance(transform.position, a.transform.position);
                            if (distance < bossHitCriRad) Damage *= 2;
                            else
                            {
                                Damage -= (int)(distance * 5); //°Å¸®°¡ ¸Õ¸¸Å­ »©ÁÜ
                            }
                            Damage = Mathf.Clamp(Damage, 5, 20);
                            Debug.Log("PlayerDamage : " + Damage);
                            damageable.OnDamage(Damage, a.transform.position, transform.position - a.transform.position);
                        }
                    }
                }
            }
        }
        else { // ÀûÀÌ ½ð ÆøÅºÀÏ¶§
            float radius = this.playerBombRad;
            Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
            foreach(var a in colliders)
            {
                if (a != null)
                {
                    if (!a.CompareTag("Enemy"))
                    {
                        IDamageable damageable;
                        if(a.transform.TryGetComponent<IDamageable>(out damageable))
                        {
                            float distance = Vector3.Distance(transform.position, a.transform.position);
                            if (distance < bossHitCriRad) Damage *= 2;
                            else
                            {
                                Damage -= (int)(distance * 10); //°Å¸®°¡ ¸Õ¸¸Å­ »©ÁÜ
                                Debug.Log("EnemyDamage : " + Damage);
                            }
                            Damage = Mathf.Clamp(Damage, 5, 20);
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
