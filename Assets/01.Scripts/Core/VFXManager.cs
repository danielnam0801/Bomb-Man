using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    public static VFXManager Instance;

    public void SpawningEffect(EffectPlayer effect, Vector3 pos, Quaternion rot)
    {
        EffectPlayer thisEffect = PoolManager.Instance.Pop(effect.gameObject.name) as EffectPlayer;
        thisEffect.SetPositionAndRotation(pos, rot);
        thisEffect.StartPlay(5f);
    }
}
