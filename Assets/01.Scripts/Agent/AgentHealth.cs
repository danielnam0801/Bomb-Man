using System;
using UnityEngine;
using UnityEngine.Events;

public class AgentHealth : MonoBehaviour, IDamageable
{
    public UnityEvent<int, Vector3, Vector3> OnHitTrigger = null;
    public UnityEvent OnDeadTrigger = null;

    public Action<int, int> OnHealthChanged;

    [SerializeField]
    private HealthAndArmorSO _healthAndArmor;
    private int _currentHealth;

    private AgentController _agentController;
    private void Awake()
    {
        _agentController = GetComponent<AgentController>();
    }

    private void Start()
    {
        _currentHealth = _healthAndArmor.MaxHP; //최대체력으로 채우고
    }

    public void AddHealth(int value)
    {
        _currentHealth = Mathf.Clamp(_currentHealth + value, 0, _healthAndArmor.MaxHP);
    }

    public void DeadProcess()
    {
        UIManager.Instance.CurrentUI.GetComponent<MainUI>().PlayerDieEvent();
    }

    public void OnDamage(int damage, Vector3 point, Vector3 normal)
    {
        if (_agentController.IsDead) return; //사망시 중지

        int calcDamage = Mathf.CeilToInt(damage * (1 - _healthAndArmor.ArmorValue));
        AddHealth(-calcDamage);

        if (_currentHealth == 0)
        {
            OnDeadTrigger?.Invoke();
        }
        else
        {
            //_agentController.ChangeState(Core.StateType.OnHit); //이것도 주석 해제해줄거야
        }

        OnHitTrigger?.Invoke(calcDamage, point, normal);
    }
}
