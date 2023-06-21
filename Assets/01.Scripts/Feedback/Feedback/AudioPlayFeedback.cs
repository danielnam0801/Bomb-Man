using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayFeedback : Feedback
{
    AudioSource audioSource;
    public bool useRandomPitch = false;
    [SerializeField] float minRandomPitchValue = -0.25f;
    [SerializeField] float maxRandomPitchValue = 0.25f;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();    
    }

    public override void CreateFeedBack()
    {
        if (useRandomPitch)
            audioSource.pitch = audioSource.pitch + UnityEngine.Random.Range(minRandomPitchValue, maxRandomPitchValue);
        audioSource.PlayOneShot(audioSource.clip);
    }

    public override void FinishFeedBack()
    {
        audioSource.Stop();
    }
}
