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
    public float JumpPower = 30f;
    public float CanApplyBombRadius = 5f;
}
