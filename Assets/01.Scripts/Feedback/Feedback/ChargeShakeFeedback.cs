using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeShakeFeedback : MonoBehaviour
{
    private CinemachineBasicMultiChannelPerlin _noise;

    private void OnEnable()
    {
        if (Core.DefineEtc.VCam == null) Debug.LogError("ShakeFeedback¿¡ Vcam¾øÀ½");
        _noise = Core.DefineEtc.VCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void SetShake(float _amplitude, float _intensity)
    {
        _noise.m_AmplitudeGain = _amplitude;
        _noise.m_FrequencyGain = _intensity;
    }

    public void ResetShake()
    {
        _noise.m_AmplitudeGain = 0;
    }
}
