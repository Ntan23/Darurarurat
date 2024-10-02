using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Get the mouse pos and get its distance between its start pos and end pos
/// </summary>
public interface IGetMousePositionDistance
{
    // bool CanAnimate{ get; }
    //private Vector3 mousePosition;
    //private float mouseDistance;
    void CheckMousePos();
    void CheckMouseDistance_StartToEnd();
}
