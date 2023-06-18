using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMatColor : MonoBehaviour
{
    [SerializeField]
    Material mat;

    Color currentColor;

    private void Start()
    {
        currentColor = mat.color;
    }

    private void Update()
    {
        Color color = mat.color;
        mat.SetColor();
    }
}
