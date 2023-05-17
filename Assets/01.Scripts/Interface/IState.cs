using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    public void OnEnterState();
    public void OnUpdateState();
    public void OnExitState();
    public void SetUp(Transform root);
}
