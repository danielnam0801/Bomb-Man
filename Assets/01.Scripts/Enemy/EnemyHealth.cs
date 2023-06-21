using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    public UnityEvent OnHitTriggered = null;
    public UnityEvent OnDeadTriggered = null;

    private AIActionData _aiActionData;
    private RobotBossPhaseData _phaseData;

    public Action<int, int> OnHealthChanged = null;

    public bool IsDead { get; set; }
    
    private int _maxHP;
    private int _currentHP;

    public int MaxHP => _maxHP;
    public int CurrentHP => _currentHP;

    private void Awake()
    {
        _aiActionData = transform.Find("AI").GetComponent<AIActionData>();
        _phaseData = transform.Find("AI").GetComponent<RobotBossPhaseData>();
    }

    public void SetMaxHP(int value)
    {
        _currentHP = _maxHP = value;
        IsDead = false;
    }

    public void OnDamage(int damage, Vector3 point, Vector3 normal)
    {
        if (IsDead) return;

        _aiActionData.HitPoint = point;
        _aiActionData.HitNormal = normal;

        _currentHP -= damage;
        _currentHP = Mathf.Clamp(_currentHP, 0, _maxHP);
        if(_currentHP <= 0)
        {
            IsDead = true;
            OnDeadTriggered?.Invoke();
        }

        int value = (int)(((float)CurrentHP / (float)MaxHP) * 100);
        Debug.Log("Value : " + value);
        if (value >= 60) _phaseData.CurrentPhase = 1;
        else if (value >= 25) _phaseData.CurrentPhase = 2; 
        else if (value >= 0) _phaseData.CurrentPhase = 3; 

        OnHitTriggered?.Invoke();

        //UIManager.Instance.Subscribe(this); //나를 구독해라
        OnHealthChanged?.Invoke(_currentHP, _maxHP); //그리고 전파
        Debug.Log(OnHealthChanged.Method.Name);
    }

}
