using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.VFX;
using System;
using UnityEngine.Events;

public class SingleProjectile : Projectile
{
    private VisualEffect _beamMuzzle;

    private LineRenderer _lineRenderer;
    private Light _beamLight;

    private RBBossSingleAttack _bossShootAttack;
    ChargeShakeFeedback _chargeShakeFeedback;

    public UnityEvent chargeSound;
    public UnityEvent shootSound;

    [SerializeField]
    private float _beamLength = 10f;
    public LayerMask WhatIsEnemy; //무엇이 적인지 식별 넣어놔야 자기들끼리 안때림

    [SerializeField]
    private float _beamTime = 0.6f;

    Vector3 dir;
    
    protected override void Awake()
    {
        base.Awake();
        _lineRenderer = GetComponent<LineRenderer>();
        _beamMuzzle = transform.Find("BeamMuzzle").GetComponent<VisualEffect>();
        _beamLight = transform.Find("BeamMuzzle/BeamLight").GetComponent<Light>();
        _chargeShakeFeedback = GetComponent<ChargeShakeFeedback>();
        
        Init();
    }

    public void AttackSet(RBBossSingleAttack rbBossShootAttack)
    {
        if(_bossShootAttack == null)    
            _bossShootAttack = rbBossShootAttack;
    }

    public void PreCharging()
    {
        _beamMuzzle.Play();
        _beamLight.enabled = true;
    }

    public override void Shoot()
    {
        dir = _bossShootAttack.transform.forward;
        dir.y -= 0.3f;
        //dir = GameManager.Instance.PlayerOriginTrm.position - transform.position;
        StartCoroutine(FireBeam(bulletData.damage, dir));
    }

    public void Charge(float time)
    {
        chargeSound?.Invoke();
        PreCharging();
         Sequence seq = DOTween.Sequence();
        Tween scaleUp = _beamMuzzle.transform.DOScale(Vector3.one, time).SetEase(Ease.InQuint);
        seq.Append(scaleUp);
        Vector3 shakeValue = Vector3.zero;
        Tween shake = DOVirtual.Vector3(shakeValue, new Vector3(4, 4, 0), time,
            onVirtualUpdate: t => _chargeShakeFeedback.SetShake(t.x, t.y)).SetEase(Ease.InQuint);
        seq.Join(shake);
        Action shootAct = null;
        shootAct += () => _bossShootAttack.shootBulletCall = true;
        shootAct += () => DOVirtual.Vector3(new Vector3(4,4,0), Vector3.zero, time - 0.5f,
            onVirtualUpdate: t => _chargeShakeFeedback.SetShake(t.x, t.y))
        .OnComplete(()=>_chargeShakeFeedback.ResetShake()).SetEase(Ease.InQuad);
        shootAct += () => shootSound?.Invoke();

        seq.onComplete = () => UtilMono.Instance.AddDelayCoroutine(() =>
            shootAct.Invoke(),
            0.5f);

        dir = GameManager.Instance.PlayerOriginTrm.position - transform.position;
        dir.y = 0;
    }
    
    IEnumerator FireBeam(int damage, Vector3 targetDir)
    {
        float t = 0; 

        while(t < _beamTime)
        {
            _lineRenderer.SetPosition(0, transform.position + targetDir.normalized * 0.5f); //빔의 시작점을 현재 위치로 설정
            Vector3 endPos = transform.position + targetDir * _beamLength;
            _lineRenderer.enabled = true;
            _lineRenderer.SetPosition(1, endPos);
            
            _beamLength = Core.Define.MAPZSIZE;
            
            float r = _lineRenderer.startWidth - 0.1f;
            RaycastHit hit;
            bool isHit = Physics.SphereCast(
                transform.position, r, targetDir.normalized, out hit, _beamLength, WhatIsEnemy);
            if (isHit)
            {
                Debug.Log("맞음 : " + hit.collider.name);
                if (hit.collider.TryGetComponent<IDamageable>(out IDamageable health))
                {
                    health.OnDamage(damage, hit.point, hit.normal);
                }
            }
            t += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        StopBeam();
    }

    public void StopBeam()
    {
        StartCoroutine(StopSequence());
    }

    private IEnumerator StopSequence()
    {
        _lineRenderer.enabled = false;
        _beamLight.enabled = false;
        yield return new WaitForSeconds(0.05f);
        _beamMuzzle.Stop();
        yield return new WaitForSeconds(0.3f);
        _bossShootAttack.endAttack = true;
        Destroy(this);
    }
    public override void Init()
    {
        _beamMuzzle.gameObject.transform.localScale = Vector3.one * 0.1f;
        _lineRenderer.enabled = false;
        _beamLight.enabled = false;
        _beamMuzzle.Stop();
    }
}
