using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    private GameManager gm;
    private Animator animator;
    [SerializeField] private ObjectControl targetObjectControl;
    private bool canInteract;
    [SerializeField] private GameObject button;
    [SerializeField] private ParticleSystem particleFX;
    
    void Start() 
    {
        gm = GameManager.instance;

        animator = GetComponent<Animator>();

        button.SetActive(false);
    } 

    void OnMouseEnter()
    {
        if(canInteract && gm.IsPlaying() && !gm.GetPauseMenuIsAnimating()) button.SetActive(true);
    }

    void OnMouseExit()
    {
        if(gm.IsPlaying() && !gm.GetPauseMenuIsAnimating()) button.SetActive(false);
    }

    public void ApplyVaseline()
    {
        button.SetActive(false);
        ChangeCanInteract();
        animator.Play("Apply Vaseline");
        StartCoroutine(ApplyEffect());
    }

    IEnumerator ApplyEffect()
    {
        yield return new WaitForSeconds(0.4f);
        particleFX.Play();
        yield return new WaitForSeconds(1.0f);
        particleFX.Pause();
        yield return new WaitForSeconds(0.2f);
        targetObjectControl.CheckWinCondition(); //Keknya jgn di sini ||Want Change||
    }

    public void ChangeCanInteract() => canInteract = !canInteract;

}
