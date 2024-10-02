using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interaction with Other Item, example: Patient
/// </summary>
public abstract class ObjectToOthersInteraction : MonoBehaviour, IInteractWithPatient
{
    public abstract void InteractionWithPatient();

}
