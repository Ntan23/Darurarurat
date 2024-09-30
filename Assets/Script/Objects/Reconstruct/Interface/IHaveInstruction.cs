using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHaveInstruction
{
    GameObject[] instructionArrows {get;}// Sama
    GameObject instructionArrowParent {get;}// Sama
    void DoShowInstruction();
    void ShowInstruction();
}
