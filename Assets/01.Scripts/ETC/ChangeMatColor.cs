using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMatColor : MonoBehaviour
{
    Renderer m_Renderer;
    Color currentColor;

    void Start()
    {
        m_Renderer = GetComponent<Renderer>();
        Color color = m_Renderer.material.GetColor("_TintColor");
        currentColor = color;
    }

    [SerializeField] float colorChangeCool = 5f;
    [SerializeField] float lerpTime = 2f;
    bool canChangeColor = true;
    float t = 0f;
    void Update()
    {
        if (t > colorChangeCool && canChangeColor)
        {
            t = 0;
            canChangeColor = false;
            Color color = new Color(Random.value, Random.value, Random.value, currentColor.r);
            StartCoroutine(ChangeCor(color));
        }

        t+=Time.deltaTime;
    }
    IEnumerator ChangeCor(Color color)
    {
        float time = 0;
        while (time < lerpTime)
        {
            currentColor = Color.Lerp(currentColor, color, time / lerpTime);
            m_Renderer.material.SetColor("_TintColor", currentColor);
            time += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        canChangeColor = true;
    }
}

