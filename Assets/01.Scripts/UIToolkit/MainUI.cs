using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MainUI : MonoBehaviour
{
    private UIDocument document;
    [SerializeField]
    private VisualTreeAsset visualTreeAsset;
    private EnemyHPBar _enemyBar;
    private EnemyHealth _subscribedEnemy = null;

    private void Awake()
    {
        document = GetComponent<UIDocument>();
    }

    private void OnEnable()
    {
        VisualElement root = document.rootVisualElement;
        FightUI(root);
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

    public void FightUI(VisualElement root)
    {
        VisualElement _hpBarRoot = root.Q<VisualElement>("BarRect");
        _enemyBar = new EnemyHPBar(_hpBarRoot);
    }

    public void BossDieEvent()
    {
        UIManager.Instance.GoIntro();
    }

    public void PlayerDieEvent()
    {
        VisualElement root = document.rootVisualElement;

        VisualElement deadUI = root.Q<VisualElement>("DeadUI");
        deadUI.AddToClassList("on");

        VisualElement introBtn , exitBtn;

        introBtn = root.Q<VisualElement>("IntroBtn");
        exitBtn = root.Q<VisualElement>("ExitBtn");

        introBtn.RegisterCallback<ClickEvent>((t) =>
        {
            UIManager.Instance.GoIntro();
            VisualElementInit();
        });
        exitBtn.RegisterCallback<ClickEvent>((t) =>
        {
            GameManager.Instance.ExitGame();
        });
    }

    private void UpdateEnemyHPData(int current, int max)
    {
        _enemyBar.HP = current;
    }

    void VisualElementInit()
    {
        VisualTreeAsset visualTreeAsset = Instantiate(this.visualTreeAsset);
        document.visualTreeAsset = visualTreeAsset;
    }



}
