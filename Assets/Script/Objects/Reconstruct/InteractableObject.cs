using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableObject : MonoBehaviour
{
    private GameManager gm;
    private Animator animator;
    [SerializeField]private ObjectControl objControl;
}

