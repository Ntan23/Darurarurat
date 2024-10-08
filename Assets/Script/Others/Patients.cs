using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    [SerializeField] private Vector3[] savedPosition;
    //private Animator playerAnimator;
    private bool canMove;
    private bool canBeTreated;
    private PatientsQueueManager pqm;

    void Awake()
    {
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        pqm = PatientsQueueManager.instance;

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        woundIndicator.SetActive(false);

        if(SceneManager.GetActiveScene().name == "PatientReception" && savedPosition != null) MoveBack();
    }

    void Update()
    {
        if(canMove)
        {
            goalpos = positions[nextLocation];

            if(Vector2.Distance(_object.transform.position, goalpos.position) > 0) MovingToNextPos();
            if(Vector2.Distance(_object.transform.position, goalpos.position) <= 0) 
            {
                if(Vector2.Distance(_object.transform.position,goalpos.position) == 0) canBeTreated = true;
                
                if(nextLocation == 1) LeanTween.scale(woundIndicator, new Vector3(0.5f, 0.5f, 0.5f), 2.0f).setEaseSpring().setOnComplete(() => canMove = false);
                
                if(nextLocation < 1) 
                {
                    LeanTween.scale(_object.gameObject, new Vector3(2.0f, 2.0f, 2.0f), 2.0f);
                    nextLocation++;
                }
            }
        } 
    }

    public void TreatWound()
    {
        if(canBeTreated) ScenesManager.instance.GoToTargetScene("Level " + (_wound.woundIndex + 1).ToString());
    }

    void MovingToNextPos() => _object.position = Vector2.MoveTowards(_object.position, goalpos.position, Time.deltaTime * speed); 
    
    private void MoveBack()
    {
        LeanTween.scale(_object.gameObject, Vector3.one, 2.0f);
        LeanTween.move(_object.gameObject, savedPosition[0], 2.0f).setOnComplete(() => LeanTween.move(_object.gameObject, savedPosition[1], 2.0f).setOnComplete(() => Destroy(this.gameObject)));
    }

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
