using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotBossPhaseData : MonoBehaviour
{
    public bool IsDashing;
    public bool IsShooting;
    public bool IsJumping;

    public int currentPhase;

    public bool CanAttack => IsDashing == false && IsShooting == false && IsJumping == false;

    private void OnEnable()
    {
        currentPhase = 0;
        IsDashing = false;
        IsShooting = false;
        IsJumping = false;
    }
}
