using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class IntroUI : MonoBehaviour
{
    UIDocument document;
    [SerializeField]
    VisualTreeAsset visualTreeAsset;

    private void Awake()
    {
        document = GetComponent<UIDocument>();
    }
    private void OnEnable()
    {
        VisualElement root = document.rootVisualElement;
        Intro(root);
    }

    private void Intro(VisualElement _root)
    {
        VisualElement _startBtn, _creditBtn, _exitBtn, fadePanel, clickFadePanel;
        VisualElement creditPanel, _creditExitBtn; 
        List<VisualElement> btnList = new List<VisualElement>();

        _startBtn = _root.Q<VisualElement>("StartBtn");
        _exitBtn = _root.Q<VisualElement>("ExitBtn");
        _creditBtn = _root.Q<VisualElement>("CreditBtn");
      
        fadePanel = _root.Q<VisualElement>("FadePanel");      
        clickFadePanel = _root.Q<VisualElement>("ClickFadePanel");

        _creditExitBtn = _root.Q<VisualElement>("CreditEndBtn");
        creditPanel = _root.Q<VisualElement>("CreditPanel");
        
        btnList.Add(_startBtn);
        btnList.Add(_creditBtn);
        btnList.Add(_exitBtn);


        bool isClick = false;
        _startBtn.RegisterCallback<ClickEvent>((t) =>
        {
            Debug.Log("CLicked");
            StartCoroutine(BtnClickEvent(btnList, clickFadePanel, fadePanel, _root));
            isClick = true;
        });

        _creditBtn.RegisterCallback<ClickEvent>((t) =>
        {
            creditPanel.AddToClassList("on");
            isClick = true;

            _creditExitBtn.RegisterCallback<ClickEvent>((t) =>
            {
                creditPanel.RemoveFromClassList("on");
                fadePanel.RemoveFromClassList("on");
                isClick = false;
            });
        });

        _exitBtn.RegisterCallback<ClickEvent>((t) =>
        {
            GameManager.Instance.ExitGame();
            isClick = true;
        });

        #region fadePanel
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
        #endregion
    }

    void VisualElementInit()
    {
        VisualTreeAsset visualTreeAsset = Instantiate(this.visualTreeAsset);
        document.visualTreeAsset = visualTreeAsset;
    }

    private void ChangeToBossSceneInit()
    {
        GameManager.Instance.FightSceneInit();

        UIManager.Instance.UseFightUI = true;
        GameManager.Instance.IsFightScene = true;
        GameManager.Instance.IsIntroScene = false;

        GameManager.Instance.LoadScene("BossScene");
    }

    IEnumerator BtnClickEvent(List<VisualElement> btnList, VisualElement clickPanel, VisualElement fadePanel, VisualElement root)
    {
       
        foreach (var btn in btnList)
        {
            btn.AddToClassList("BtnClick");
            yield return new WaitForSeconds(0.3f);
        }
        yield return new WaitForSeconds(1f);

        bool isStart = true;
        clickPanel.AddToClassList("start");
        clickPanel.RegisterCallback<TransitionEndEvent>((t) =>
        {
            if (isStart)
            {
                isStart = false;
                Debug.Log("TransitionEndEventMultiple!!");
                VisualElement leftPanel = root.Q<VisualElement>("LeftPanel");
                leftPanel.AddToClassList("LeftPanelEnd");
                ChangeToBossSceneInit();
            }
        });
        yield return new WaitForSeconds(2f);
        fadePanel.RemoveFromClassList("on");
        bool panelEnd = false;
        clickPanel.AddToClassList("end");
        clickPanel.RegisterCallback<TransitionEndEvent>((t) =>
        {
            if (!isStart)
            {
                VisualElement upPanel = root.Q<VisualElement>("UpPanel");
                VisualElement downPanel = root.Q<VisualElement>("DownPanel");
                panelEnd = true;
                upPanel.AddToClassList("PanelEnd");
                downPanel.AddToClassList("PanelEnd2");
                isStart = true;
            }
        });
        yield return new WaitUntil(()=> panelEnd);
        yield return new WaitForSeconds(1f);
        UIManager.Instance.UseIntroUI = false;
        VisualElementInit();
    }
}
