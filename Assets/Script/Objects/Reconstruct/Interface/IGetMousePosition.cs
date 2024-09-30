using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGetMousePosition
{
    bool CanAnimate{ get; }
    //private Vector3 mousePosition;
    //private float mouseDistance;
    void CheckMousePos_Start();
    void CheckMousePos_End();
}
