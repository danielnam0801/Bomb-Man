using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMaterial : MonoBehaviour
{   
    Renderer m_renderer;
    MaterialPropertyBlock m_material;

    // Start is called before the first frame update
    void Start()
    {
        m_renderer = GetComponent<Renderer>();
        m_renderer.GetPropertyBlock(m_material);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
