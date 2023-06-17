using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Assets/BulletData")]
public class BulletDataSO : ScriptableObject
{
    public float speed;
    public int damage;
    public AudioClip hitSound;
}
