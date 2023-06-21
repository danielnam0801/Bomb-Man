using Core;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopupText : PoolableMono
{
    private TextMeshPro _tmpText;

    private void Awake()
    {
        _tmpText = GetComponent<TextMeshPro>();
    }

    public void StartPopup(string text, Vector3 pos, int fontSize,
                                Color color, float time = 1f, float yDelta = 2f)
    {
        _tmpText.SetText(text);
        _tmpText.color = color;
        _tmpText.fontSize = fontSize;
        transform.position = pos;
        StartCoroutine(ShowRoutine(time, yDelta));
    }

    private IEnumerator ShowRoutine(float time, float yDelta)
    {
        float currentTime = 0f;
        Vector3 firstPos = transform.position;
        float percent = 0;
        while (percent < 1)
        {
            currentTime += Time.deltaTime;
            percent = currentTime / time;
            float nextY = yDelta * EaseOutBounce(percent);
            float nextOpacity = Mathf.Lerp(1, 0, percent);

            transform.position = firstPos + new Vector3(0, nextY, 0);
            _tmpText.alpha = nextOpacity;

            Vector3 camDir = (transform.position - Define.MainCam.transform.position).normalized;

            transform.rotation = Quaternion.LookRotation(camDir);
            yield return null;
        }

        PoolManager.Instance.Push(this);
    }

    public override void Init()
    {
        _tmpText.alpha = 1;
    }

    private float EaseOutBounce(float x)
    {
        const float n1 = 7.5625f;
        const float d1 = 2.75f;

        if (x < 1 / d1)
        {
            return n1 * x * x;
        }
        else if (x < 2 / d1)
        {
            return n1 * (x -= 1.5f / d1) * x + 0.75f;
        }
        else if (x < 2.5 / d1)
        {
            return n1 * (x -= 2.25f / d1) * x + 0.9375f;
        }
        else
        {
            return n1 * (x -= 2.625f / d1) * x + 0.984375f;
        }
    }
}
