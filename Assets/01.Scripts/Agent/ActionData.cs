using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionData : MonoBehaviour
{
    public Vector3 DirectionToFly;
    public bool IsGround;
    public bool IsJumping;
    public bool JumpCall;
    public int JumpCnt;
    public bool CanCheckAttack = true;

    public Vector3 StartPos;
    public Vector3 EndPos;
    public Vector3 cp1;
    public Vector3 cp2;
    public int PointCnt;

    public Vector3 DynaBombPoint;
}
