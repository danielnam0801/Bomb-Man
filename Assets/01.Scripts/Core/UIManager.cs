using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    Dictionary<string, GameObject> uis;

    private bool useIntroUi = true;
    private bool useFightUI = false;

    public GameObject CurrentUI { get; private set; }
    public bool UseIntroUI {
        get { return useIntroUi; }
        set
        {
            if (value)
            {
                useIntroUi = true;
                string intro = "IntroUI";
                uis[intro].SetActive(true);
                CurrentUI = uis[intro];
            }
            else
            {
                useIntroUi = false;
                string intro = "IntroUI";
                uis[intro].SetActive(false);
            }
        } 
    }

    public bool UseFightUI
    {
        get { return useFightUI; }
        set
        {
            if (value)
            {
                useFightUI = true;
                string fight = "MainUI";
                uis[fight].SetActive(true);
                CurrentUI = uis[fight];
            }
            else
            {
                useFightUI = false;
                string fight = "MainUI";
                uis[fight].SetActive(false);
            }
        }
    }

    public void GoIntro()
    {
        GameManager.Instance.IsIntroScene = true;
        GameManager.Instance.IsFightScene = false;
        UseFightUI = false;
        UseIntroUI = true;
        GameManager.Instance.LoadScene("IntroScene");
    }

    private void Awake()
    {
        uis = new Dictionary<string, GameObject>();

        for(int i = 0; i < transform.childCount; i++)
        {
            uis.Add(transform.GetChild(i).name , transform.GetChild(i).gameObject);
        }
    }
}
