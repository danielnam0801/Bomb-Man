using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    private UIDocument _document;
    //private UIDocument _document;

    private VisualElement _root;
    private EnemyHPBar _enemyBar;
    private EnemyHealth _subscribedEnemy = null;

    private void Awake()
    {
        _document = GetComponent<UIDocument>();
        if (_document == null) Debug.LogError("Document가 존재하지않음");
    }

    private void OnEnable()
    {
        UIEvents();
    }

    private void UIEvents()
    {
        _root = _document.rootVisualElement;
        if (GameManager.Instance.IsIntroScene)
        {
            Debug.Log("Intor");
            //ChangeDocument(GameManager.Instance._intro);
            List<VisualElement> btnList = new List<VisualElement>();
            VisualElement _startBtn = _root.Q<VisualElement>("StartBtn");
            VisualElement _creditBtn = _root.Q<VisualElement>("CreditBtn");
            VisualElement _exitBtn = _root.Q<VisualElement>("ExitBtn");

            btnList.Add(_startBtn);
            btnList.Add(_creditBtn);
            btnList.Add(_exitBtn);

            VisualElement fadePanel = _root.Q<VisualElement>("FadePanel");
            VisualElement clickFadePanel = _root.Q<VisualElement>("ClickFadePanel");

            bool isClick = false;

            _startBtn.RegisterCallback<ClickEvent>((t) =>
            {
                StartCoroutine(BtnClickEvent(btnList, clickFadePanel));
                isClick = true;
            });

            _creditBtn.RegisterCallback<ClickEvent>((t) =>
            {
                ShowCreditScreen();
            });

            _exitBtn.RegisterCallback<ClickEvent>((t) =>
            {
                ExitGame();
            });


            _startBtn.RegisterCallback<MouseOverEvent>((t) =>
            {
                fadePanel.AddToClassList("on");
            });
            _creditBtn.RegisterCallback<MouseOverEvent>((t) =>
            {
                fadePanel.AddToClassList("on");
            });
            _exitBtn.RegisterCallback<MouseOverEvent>((t) =>
            {
                fadePanel.AddToClassList("on");
            });

            _startBtn.RegisterCallback<MouseOutEvent>((t) =>
            {
                if (!isClick)
                    fadePanel.RemoveFromClassList("on");
            });
            _creditBtn.RegisterCallback<MouseOutEvent>((t) =>
            {
                if (!isClick)
                    fadePanel.RemoveFromClassList("on");
            });
            _exitBtn.RegisterCallback<MouseOutEvent>((t) =>
            {
                if (!isClick)
                    fadePanel.RemoveFromClassList("on");
            });

        }
        if (GameManager.Instance.IsLoadingScene)
        {
            ChangeDocument(GameManager.Instance._intro);
            List<VisualElement> btnList = new List<VisualElement>();
            VisualElement _startBtn = _root.Q<VisualElement>("StartBtn");
            VisualElement _creditBtn = _root.Q<VisualElement>("CreditBtn");
            VisualElement _exitBtn = _root.Q<VisualElement>("ExitBtn");

            btnList.Add(_startBtn);
            btnList.Add(_creditBtn);
            btnList.Add(_exitBtn);

            VisualElement fadePanel = _root.Q<VisualElement>("FadePanel");
            VisualElement clickFadePanel = _root.Q<VisualElement>("ClickFadePanel");

            bool isClick = false;

            _startBtn.RegisterCallback<ClickEvent>((t) =>
            {
                StartCoroutine(BtnClickEvent(btnList, clickFadePanel));
                isClick = true;
            });

            _creditBtn.RegisterCallback<ClickEvent>((t) =>
            {
                ShowCreditScreen();
            });

            _exitBtn.RegisterCallback<ClickEvent>((t) =>
            {
                ExitGame();
            });


            _startBtn.RegisterCallback<MouseOverEvent>((t) =>
            {
                fadePanel.AddToClassList("on");
            });
            _creditBtn.RegisterCallback<MouseOverEvent>((t) =>
            {
                fadePanel.AddToClassList("on");
            });
            _exitBtn.RegisterCallback<MouseOverEvent>((t) =>
            {
                fadePanel.AddToClassList("on");
            });

            _startBtn.RegisterCallback<MouseOutEvent>((t) =>
            {
                if (!isClick)
                    fadePanel.RemoveFromClassList("on");
            });
            _creditBtn.RegisterCallback<MouseOutEvent>((t) =>
            {
                if (!isClick)
                    fadePanel.RemoveFromClassList("on");
            });
            _exitBtn.RegisterCallback<MouseOutEvent>((t) =>
            {
                if (!isClick)
                    fadePanel.RemoveFromClassList("on");
            });
        }
        if (GameManager.Instance.IsFightScene)
        {
            ChangeDocument(GameManager.Instance._fight);
            VisualElement _hpBarRoot = _root.Q<VisualElement>("BarRect");
            _enemyBar = new EnemyHPBar(_hpBarRoot);
        }
    }

    public void ChangeDocument(VisualTreeAsset sourceAsset)
    {
        _document.visualTreeAsset = null;
        
        _document.visualTreeAsset = sourceAsset;
    }

    private void StartInit()
    {
        GameManager.Instance.LoadScene("loading");
        GameManager.Instance.IsIntroScene = false;
        GameManager.Instance.IsLoadingScene = true;
        //ChangeDocument(GameManager.Instance._loading);
    }

    private void ShowCreditScreen()
    {
        
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif
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

    private void UpdateEnemyHPData(int current, int max)
    {
        _enemyBar.HP = current;
    }

    IEnumerator BtnClickEvent(List<VisualElement> btnList, VisualElement clickPanel)
    {
        foreach(var btn in btnList)
        {
            btn.AddToClassList("BtnClick");
            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitForSeconds(1f);
        clickPanel.AddToClassList("start");
        clickPanel.RegisterCallback<TransitionEndEvent>((t) =>
        {
            UtilMono.Instance.AddDelayCoroutine(StartInit, 0.2f);
        });
        yield return new WaitForSeconds(2f);
        clickPanel.AddToClassList("end");
        clickPanel.RegisterCallback<TransitionEndEvent>((t) =>
        {
            UIEvents();
            ChangeDocument(GameManager.Instance._fight);
        });
        yield return new WaitForSeconds(2f);
        ChangeDocument(GameManager.Instance._intro);


    }
}
