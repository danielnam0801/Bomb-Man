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
    
    public int MaxHP => _healthAndArmor.MaxHP;

    bool canDamaged = true;

    private void Awake()
    {
        _agentController = GetComponent<AgentController>();
    }

    private void Start()
    {
        _currentHealth = _healthAndArmor.MaxHP; //�ִ�ü������ ä���
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
        if (_agentController.IsDead) return; //����� ����
        if (!canDamaged) return;
        canDamaged = false;
        UtilMono.Instance.AddDelayCoroutine(() => canDamaged = true, 0.2f);
        int calcDamage = Mathf.CeilToInt(damage * (1 - _healthAndArmor.ArmorValue));
        AddHealth(-calcDamage);

        if (_currentHealth == 0)
        {
            OnDeadTrigger?.Invoke();
        }
        else
        {
            //_agentController.ChangeState(Core.StateType.OnHit); //�̰͵� �ּ� �������ٰž�
        }

        Debug.Log("PlayerHITT");
        OnHitTrigger?.Invoke(calcDamage, point, normal);
        OnHealthChanged?.Invoke(_currentHealth, MaxHP);
        Debug.Log(OnHealthChanged.Method.Name);
    }
}
