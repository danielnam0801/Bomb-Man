using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Feedback : MonoBehaviour
{
    public abstract void CreateFeedBack();
    public abstract void FinishFeedBack();

    protected virtual void OnDestroy()
    {
        FinishFeedBack();
    }

    protected virtual void OnDisable()
    {
        FinishFeedBack();
    }
}