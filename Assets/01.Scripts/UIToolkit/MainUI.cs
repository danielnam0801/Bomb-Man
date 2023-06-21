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
    private PlayerHealthBar _playerHealthBar;
    private EnemyHealth _subscribedEnemy = null;
    private AgentHealth _subscribedPlayer = null;

    private void Awake()
    {
        document = GetComponent<UIDocument>();
    }

    private void OnEnable()
    {
        VisualElement root = document.rootVisualElement;
        FightUI(root);
        PopupSetting(root);
    }

    public void Subscribe(EnemyHealth health, AgentHealth playerHealth)
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

        _subscribedPlayer = playerHealth;
        _subscribedPlayer.OnHealthChanged += UpdatePlayerHPData;
        _playerHealthBar.MaxHP = playerHealth.MaxHP;
    }

    public void FightUI(VisualElement root)
    {
        VisualElement _enemyHpBarRoot = root.Q("BarRect");
        VisualElement _playerHpBarRoot = root.Q("BarRectPlayer");
        _enemyBar = new EnemyHPBar(_enemyHpBarRoot);
        _playerHealthBar = new PlayerHealthBar(_playerHpBarRoot);
    }

    public void BossDieEvent()
    {
        UIManager.Instance.GoIntro();
    }

    public void PopupSetting(VisualElement root)
    {
        Button popupBtn = root.Q<Button>("myPopupBtn");

        VisualElement popup = root.Q<VisualElement>("popupWindow");

        popupBtn.RegisterCallback<ClickEvent>(e =>
        {
            Time.timeScale = 0;
            popup.AddToClassList("on"); //클래스에 on을 붙여준다.
        });

        popup.RegisterCallback<MouseOutEvent>(e =>
        {
            Time.timeScale = 1;
            popup.RemoveFromClassList("on");
        });
    }

    public void PlayerDieEvent()
    {
        VisualElement root = document.rootVisualElement;

        VisualElement deadUI = root.Q<VisualElement>("DeadUI");
        deadUI.AddToClassList("on");

        VisualElement introBtn , exitBtn;
        VisualElement upPanel, downPanel;

        introBtn = root.Q<VisualElement>("IntroBtn");
        exitBtn = root.Q<VisualElement>("ExitBtn");

        upPanel = root.Q<VisualElement>("DeadTopPanel");
        downPanel = root.Q<VisualElement>("DeadBottomPanel");

        upPanel.AddToClassList("on");
        downPanel.AddToClassList("on");

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
        Debug.Log("EnemyCurrent : " + current);
        _enemyBar.HP = current;
        //_enemyBar.MaxHP = max;
    }

    private void UpdatePlayerHPData(int current, int max)
    {
        Debug.Log("PlayerCurrent : " + current);
        _playerHealthBar.HP = current;
        //_playerHealthBar.MaxHP = max;
    }


    void VisualElementInit()
    {
        VisualTreeAsset visualTreeAsset = Instantiate(this.visualTreeAsset);
        document.visualTreeAsset = visualTreeAsset;
    }



}
