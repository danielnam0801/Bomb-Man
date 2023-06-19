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
        List<VisualElement> btnList = new List<VisualElement>();

        _startBtn = _root.Q<VisualElement>("StartBtn");
        _creditBtn = _root.Q<VisualElement>("CreditBtn");
        _exitBtn = _root.Q<VisualElement>("ExitBtn");
        fadePanel = _root.Q<VisualElement>("FadePanel");      
        clickFadePanel = _root.Q<VisualElement>("ClickFadePanel");
      

        btnList.Add(_startBtn);
        btnList.Add(_creditBtn);
        btnList.Add(_exitBtn);


        bool isClick = false;
        _startBtn.RegisterCallback<ClickEvent>((t) =>
        {
            Debug.Log("CLicked");
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

    private void ChangeSceneInit()
    {
        GameManager.Instance.FightSceneInit();

        UIManager.Instance.UseFightUI = true;
        GameManager.Instance.IsFightScene = true;
        GameManager.Instance.IsIntroScene = false;

        GameManager.Instance.LoadScene("BossScene");
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

    IEnumerator BtnClickEvent(List<VisualElement> btnList, VisualElement clickPanel)
    {
        foreach (var btn in btnList)
        {
            btn.AddToClassList("BtnClick");
            yield return new WaitForSeconds(0.3f);
        }
        yield return new WaitForSeconds(1f);
        clickPanel.AddToClassList("start");
        clickPanel.RegisterCallback<TransitionEndEvent>((t) =>
        {
            ChangeSceneInit();
            //UtilMono.Instance.AddDelayCoroutine(ChangeSceneInit, 0.2f);
        });
        yield return new WaitForSeconds(2f);
        clickPanel.AddToClassList("end");
        clickPanel.RegisterCallback<TransitionEndEvent>((t) =>
        {
            UIManager.Instance.UseIntroUI = false;
            VisualElementInit();
        });
    }

}
