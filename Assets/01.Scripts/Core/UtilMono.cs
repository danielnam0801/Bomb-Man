using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilMono : MonoBehaviour
{
    public static UtilMono Instance;

    private void Awake()
    {
        if(Instance != null)
        {
            Debug.LogError("Multiple UtilMono is running");
        }
        Instance = this;
    }

    public void AddDelayCoroutine(Action Callback, float time)
    {
        StartCoroutine(DelayCoroutine(Callback, time));
    }

    private IEnumerator DelayCoroutine(Action Callback, float time)
    {
        yield return new WaitForSeconds(time);
        Callback?.Invoke();
    }
}
