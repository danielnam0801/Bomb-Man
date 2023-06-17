using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotBossPhaseData : MonoBehaviour
{
    public bool IsDashing;
    public bool IsSingleShooting;
    public bool IsBurstShooting;
    public bool IsAutoShooting;
    public bool IsJumping;

    public int currentPhase;

    public bool CanAttack => IsDashing == false && IsSingleShooting == false && IsJumping == false && IsBurstShooting == false && IsAutoShooting == false;

    private void OnEnable()
    {
        currentPhase = 0;
        IsDashing = false;
        IsSingleShooting = false;
        IsBurstShooting = false;
        IsAutoShooting = false;
        IsJumping = false;
    }
}
