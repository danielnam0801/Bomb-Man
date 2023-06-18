using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RobotBossPhaseData : MonoBehaviour
{
    public bool IsDashing;
    public bool IsSingleShooting;
    public bool IsBurstShooting;
    public bool IsAutoShooting;
    public bool IsJumping;

    private int currentPhase;

    public UnityEvent PhaseChangeEvent;

    public int CurrentPhase
    {
        get { return currentPhase; }
        set
        {
            if(value != currentPhase)
            {
                PhaseChangeEvent?.Invoke();
                currentPhase = value;
            }
        }
    }

    public bool CanAttack => IsDashing == false && IsSingleShooting == false && IsJumping == false && IsBurstShooting == false && IsAutoShooting == false;

    private void OnEnable()
    {
        currentPhase = 1;
        IsDashing = false;
        IsSingleShooting = false;
        IsBurstShooting = false;
        IsAutoShooting = false;
        IsJumping = false;
    }
}
