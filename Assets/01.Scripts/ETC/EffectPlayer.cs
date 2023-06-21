using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class EffectPlayer : PoolableMono
{
    [SerializeField]
    protected List<ParticleSystem> _particles;

    [SerializeField]
    protected List<VisualEffect> _effects;

    Rigidbody2D rb;
    AudioSource audioSource;
    
    private void Awake()
    {
        GetComponentsInChildren<ParticleSystem>(_particles);
        GetComponentsInChildren<VisualEffect>(_effects);
        audioSource = GetComponent<AudioSource>();
    }

    public void SetPositionAndRotation(Vector3 pos, Quaternion rot)
    {
        transform.SetPositionAndRotation(pos, rot);
    }

    public void StartPlay(float endTime)
    {
        audioSource.PlayOneShot(audioSource.clip);
        if (_particles != null)
        {
            _particles.ForEach(p => p.Play());
        }
        if (_effects != null)
        {
            _effects.ForEach(e => e.Play());
        }

        StartCoroutine(Timer(endTime));
    }

    public void StopPlay()
    {
        StopAllCoroutines();
        PoolManager.Instance.Push(this);
    }

    private IEnumerator Timer(float endTime)
    {
        yield return new WaitForSeconds(endTime);
        PoolManager.Instance.Push(this);
    }

    public override void Init()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (_particles != null)
        {
            _particles.ForEach(p => p.Simulate(0));
        }
        if (_effects != null)
        {
            _effects.ForEach(e => e.Stop());
        }
    }

}
