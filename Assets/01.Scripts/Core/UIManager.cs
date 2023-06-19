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
    public bool UseIntroUI {
        get { return useIntroUi; }
        set
        {
            if (value)
            {
                useIntroUi = true;
                string intro = "IntroUI";
                uis[intro].SetActive(true);
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
            }
            else
            {
                useFightUI = false;
                string fight = "MainUI";
                uis[fight].SetActive(false);
            }
        }
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
