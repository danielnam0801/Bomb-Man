using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : PoolableMono
{
    [SerializeField] protected BulletDataSO bulletData;
    AudioSource audioSource;
    public abstract void Shoot(Vector3 dir);

    protected virtual void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if(bulletData.hitSound != null)
            audioSource.clip = bulletData.hitSound;
    }

    private void OnCollisionEnter(Collision collision)
    {
        IDamageable damageable;
        if(collision.transform.TryGetComponent<IDamageable>(out damageable))
        {
            damageable.OnDamage((int)bulletData.damage, collision.contacts[0].point, transform.forward);   
        }

        DestoryEvent();
    }

    private void DestoryEvent()
    {
        audioSource.Play(); //hitSoundÃâ·Â
        PoolManager.Instance.Push(this);
    }
}
