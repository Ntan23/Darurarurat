using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHaveTemporaryWrapping
{
    bool IsOpen{ get; }
    void OpenWrapping();

}
