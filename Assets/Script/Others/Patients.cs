using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patients : MonoBehaviour
{
    private PatientWoundSO _wound;
    private Sprite woundSprite;
    [SerializeField] private GameObject woundIndicator;


    [Header("For Movement")]
    [SerializeField] private float speed = 2;
    private Transform[] positions = new Transform[2];
    [SerializeField] private Transform _object;
    private int nextLocation;
    private Transform goalpos;
    //private Animator playerAnimator;
    private bool canMove;

    void Update()
    {
        if(canMove)
        {
            goalpos = positions[nextLocation];

            if(Vector2.Distance(_object.transform.position,goalpos.position) > 0) MovingToNextPos();
            if(Vector2.Distance(_object.transform.position,goalpos.position) <= 0) 
            {
                if(nextLocation == 1) LeanTween.scale(woundIndicator, new Vector3(0.5f, 0.5f, 0.5f), 2.0f).setEaseSpring();
                if(nextLocation < 1) 
                {
                    LeanTween.scale(_object.gameObject, new Vector3(2.0f, 2.0f, 2.0f), 2.0f);
                    nextLocation++;
                }
            }
        } 
    }

    void MovingToNextPos() => _object.position = Vector2.MoveTowards(_object.position, goalpos.position, Time.deltaTime * speed); 
    

    public void SetWound(PatientWoundSO wound) => _wound = wound;
    public void SetWoundSprite(PatientWoundSO wound) => woundSprite = wound.woundSprite;  
    public void SetTargetPositions(Transform[] pos) 
    {
        for(int i = 0; i < pos.Length; i++) positions[i] = pos[i];

        StartCoroutine(Delay());
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(1.0f);
        canMove = true;
    }

}
