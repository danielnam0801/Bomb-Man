using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MainUI : MonoBehaviour
{
    private UIDocument _uiDocument;
    private EnemyHPBar _enemyBar;
    private EnemyHealth _subscribedEnemy = null;

    private void Awake()
    {
        _uiDocument = GetComponent<UIDocument>();
    }

    private void OnEnable()
    {
       
    }

    public void Subscribe(EnemyHealth health)
    {
        _enemyBar.ShowBar(true);

        if (_subscribedEnemy != health)
        {
            if (_subscribedEnemy != null)
            {
                _subscribedEnemy.OnHealthChanged -= UpdateEnemyHPData;
            }

            _subscribedEnemy = health;
            _subscribedEnemy.OnHealthChanged += UpdateEnemyHPData;

            _enemyBar.EnemyName = health.gameObject.name;
            _enemyBar.MaxHP = _subscribedEnemy.MaxHP;
        }
    }

    private void FightUI(VisualElement root)
    {
        VisualElement _hpBarRoot = root.Q<VisualElement>("BarRect");
        _enemyBar = new EnemyHPBar(_hpBarRoot);
    }

    private void UpdateEnemyHPData(int current, int max)
    {
        _enemyBar.HP = current;
    }
}
