using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIDecision : MonoBehaviour
{
    protected AIActionData _aIActionData;
    protected EnemyController _enemyController;

    public bool IsReverse = false;

    public virtual void SetUp(Transform parentRoot)
    {
        _enemyController = parentRoot.GetComponent<EnemyController>();
        _aIActionData = parentRoot.Find("AI").GetComponent<AIActionData>();
    }

    public abstract bool MakeDecision();
}
