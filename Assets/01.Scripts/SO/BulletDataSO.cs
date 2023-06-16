using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Assets/BulletData")]
public class BulletDataSO : ScriptableObject
{
    public float speed;
    public float damage;
    public AudioClip hitSound;
}
