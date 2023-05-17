using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/CharacterData")]
public class CharacterDataSO : ScriptableObject
{
    public int BaseDamage;
    public float BaseCriticial;
    public float BaseCriticialDamage;
    public float MoveSpeed;
}
