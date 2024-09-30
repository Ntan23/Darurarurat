using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHavePermanentCap
{
    bool IsOpen{ get; }
    Collider ObjectCollider {get;}   
    Collider CapCollider {get;}
    void OpenCap();
    void CloseCap();
}
