using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedbackPlayer : MonoBehaviour
{
    List<Feedback> _feedbackList;

    private void Awake()
    {
        _feedbackList = new List<Feedback>();
        GetComponents<Feedback>(_feedbackList);
    }

    public void PlayFeedback()
    {
        FinishFeedback();
        foreach (Feedback f in _feedbackList)
        {
            f.CreateFeedBack();
        }
    }

    public void FinishFeedback()
    {
        foreach (Feedback f in _feedbackList)
        {
            f.FinishFeedBack();
        }
    }
}
