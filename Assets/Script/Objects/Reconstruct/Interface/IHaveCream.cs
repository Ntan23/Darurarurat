using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public interface IHaveCream
{
    // void GrabCream();
    event EventHandler OnGettingCream, OnCreamReady;
}
