using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShareMaterialApply : MonoBehaviour
{
    Renderer m_render;
    ShareMaterial _shareMaterial;
    private void Awake()
    {
        m_render = GetComponent<Renderer>();
        _shareMaterial = transform.parent.GetComponent<ShareMaterial>();
    }

    private void Start()
    {
        _shareMaterial.sharedColor = m_render.material.GetColor("_TintColor");
    }

    private void Update()
    {
        m_render.material.SetColor("_TintColor",_shareMaterial.sharedColor);
    }
}
