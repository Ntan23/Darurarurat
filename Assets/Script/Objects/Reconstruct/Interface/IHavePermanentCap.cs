using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHavePermanentCap
{
    // Collider ObjectCollider {get;}   
    // Collider CapCollider {get;}
    void OpenCap();
    void CloseCap();
}
