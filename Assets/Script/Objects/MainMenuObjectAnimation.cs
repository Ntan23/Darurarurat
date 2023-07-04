using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuObjectAnimation : MonoBehaviour
{
    [SerializeField] private GameObject[] objects;
    [SerializeField] private float[] targetZPosition;

    void Start() => StartCoroutine(StartAnimation());

    public void MoveBack() => StartCoroutine(MoveBackAnimation());

    IEnumerator StartAnimation()
    {
        yield return new WaitForSeconds(1.0f);
        LeanTween.moveZ(objects[0], targetZPosition[0], 0.8f).setEaseSpring();
        yield return new WaitForSeconds(0.5f);
        LeanTween.moveZ(objects[1], targetZPosition[1], 0.8f).setEaseSpring();
    }

    IEnumerator MoveBackAnimation()
    {
        LeanTween.moveZ(objects[0], 15.0f, 0.8f).setEaseSpring();
        yield return new WaitForSeconds(0.25f);
        LeanTween.moveZ(objects[1], 15.0f, 0.8f).setEaseSpring();
    }
}
