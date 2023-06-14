using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotBossPhaseData : MonoBehaviour
{
    public bool isDashing;
    public bool isShooting;
    public bool isJumping;

    public bool CanAttack => isDashing == false && isShooting == false && isJumping == false;
}
