using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeShakeFeedback : MonoBehaviour
{
    private CinemachineBasicMultiChannelPerlin _noise;

    public void SetShake(float _amplitude, float _intensity)
    {

        if (Core.DefineEtc.VCam == null) Debug.LogError("ShakeFeedback¿¡ Vcam¾øÀ½");
        if(_noise == null)
            _noise = Core.DefineEtc.VCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        _noise.m_AmplitudeGain = _amplitude;
        _noise.m_FrequencyGain = _intensity;
    }

    public void ResetShake()
    {
        _noise.m_AmplitudeGain = 0;
    }
}
