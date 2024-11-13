using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Patients : MonoBehaviour
{
    private PatientWoundSO _wound;
    [SerializeField] private SpriteRenderer woundSprite;
    [SerializeField] private GameObject woundIndicator;


    [Header("For Movement")]
    [SerializeField] private float speed = 2;
    private Transform[] positions = new Transform[2];
    [SerializeField] private SpriteRenderer patientSpriteRenderer;
    private int nextLocation;
    private Transform goalpos;
    [SerializeField] private Vector3[] savedPosition;
    //private Animator playerAnimator;
    private bool canMove;
    private bool canBeTreated;
    private bool isTreated;
    private PatientsQueueManager pqm;
    [SerializeField] private bool isSpecial;
    [Header("For Special Patients Only")]
    [SerializeField] private int specialID;
    private Animator doorAnimator;

    void Awake() => DontDestroyOnLoad(this);
    
    void Start()
    {
        pqm = PatientsQueueManager.instance;

        patientSpriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();

        doorAnimator = GameObject.FindGameObjectWithTag("Door").GetComponent<Animator>();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        woundIndicator.SetActive(false);

        if(!isSpecial && SceneManager.GetActiveScene().name == "PatientReception" && savedPosition != null) 
        {
            MoveBack();
        }
    }

    void Update()
    {
        if(canMove)
        {
            goalpos = positions[nextLocation];
            
            if(goalpos != null)
            {
                if(Vector2.Distance(transform.position, goalpos.position) > 0) MovingToNextPos();
                if(Vector2.Distance(transform.position, goalpos.position) <= 0) 
                {
                    if(Vector2.Distance(transform.position,goalpos.position) == 0) canBeTreated = true;
                    
                    if(nextLocation == 1) LeanTween.scale(woundIndicator, new Vector3(0.5f, 0.5f, 0.5f), 2.0f).setEaseSpring().setOnComplete(() => canMove = false);
                    
                    if(nextLocation < 1) StartCoroutine(PatientFadeAndDoorAnimation());
                }
            }
        } 
    }

    void UpdateSpriteAlpa(float alpha) => patientSpriteRenderer.color = new Color(1.0f, 1.0f, 1.0f, alpha);

    IEnumerator PatientFadeAndDoorAnimation()
    {
        canMove = false;
        patientSpriteRenderer.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        doorAnimator.Play("Door_OpenClose");
        transform.position = positions[1].position;
        transform.localScale =  new Vector3(1.8f, 1.8f, 1.8f);
        yield return new WaitForSeconds(0.2f);
        patientSpriteRenderer.sortingOrder = 1;
        LeanTween.value(patientSpriteRenderer.gameObject, UpdateSpriteAlpa, 0.0f, 1.0f, 1.0f).setOnComplete(() => 
        {
            canMove = true;

            //LeanTween.scale(gameObject, new Vector3(1.8f, 1.8f, 1.8f), 2.0f);
        });

        nextLocation++;
    }
    void OnMouseDown() => TreatWound();

    public void TreatWound()
    {
        if(canBeTreated) 
        {
            canBeTreated = false;
            
            if(!isSpecial) ScenesManager.instance.GoToTargetScene("Level " + (_wound.woundIndex + 1).ToString());

            if(isSpecial) 
            {
                PlayerPrefs.SetInt("SpecialSpawned", 0);
                ScenesManager.instance.GoToTargetScene("Special " + (specialID + 1).ToString());
            }
        }
    }
    void MovingToNextPos() => transform.position = Vector2.MoveTowards(transform.position, goalpos.position, Time.deltaTime * speed); 
    
    private void MoveBack()
    {
        // LeanTween.scale(gameObject, Vector3.one, 2.0f);
        // LeanTween.move(gameObject, savedPosition[0], 2.0f).setOnComplete(() => LeanTween.move(gameObject, savedPosition[1], 2.0f).setOnComplete(() => Destroy(this.gameObject)));
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(1.0f);
        //pqm.UpdateProgress(isTreated);
        LeanTween.value(patientSpriteRenderer.gameObject, UpdateSpriteAlpa, 1.0f, 0.0f, 1.0f).setOnComplete(() => Destroy(this.gameObject));
    }

    public void SetWound(PatientWoundSO wound) => _wound = wound;
    public void SetWoundSprite(PatientWoundSO wound) => woundSprite.sprite = wound.woundSprite;  
    public void SetTargetPositions(Transform[] pos) 
    {
        for(int i = 0; i < pos.Length; i++) positions[i] = pos[i];

        StartCoroutine(Delay());
    }

    public void SetTreatValue(bool value) => isTreated = value;
    
    IEnumerator Delay()
    {
        yield return new WaitForSeconds(1.0f);
        canMove = true;
    }

    public int GetSpecialID()
    {
        return specialID;
    }
}
