using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombManager : MonoBehaviour
{
    public static BombManager Instance;

    [SerializeField] Dynamite[] dynamites;

    private void OnEnable()
    {
        if (dynamites == null)
            dynamites = Resources.LoadAll<Dynamite>("UsingBomb");
    }

    public string SelectBomb()
    {
        return dynamites[Random.Range(0, dynamites.Length)].gameObject.name;
    }
    
}
