using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMatColor : MonoBehaviour
{
    bool shareMaterialTrue = false;
    Renderer m_Renderer;
    Color currentColor;
    ShareMaterial shareMaterialManager;
    public Material shareMaterial;

    private void Awake()
    {
        TryGetComponent<Renderer>(out m_Renderer);
        if(TryGetComponent<ShareMaterial>(out shareMaterialManager))
        {
            shareMaterialTrue = true;
        };
    }

    void Start()
    {
        currentColor = shareMaterial.GetColor("_TintColor");
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
            Color color = new Color(Random.value, Random.value, Random.value, currentColor.a);
            StartCoroutine(ChangeCor(color));
        }

        if (!shareMaterialTrue)
            m_Renderer.material.SetColor("_TintColor", currentColor);
        t +=Time.deltaTime;
    }
    IEnumerator ChangeCor(Color color)
    {
        float time = 0;
        while (time < lerpTime)
        {
            currentColor = Color.Lerp(currentColor, color, time / lerpTime);
            if (shareMaterialTrue)
            {
                shareMaterialManager.sharedColor = currentColor;
            }
            time += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        canChangeColor = true;
    }
}

