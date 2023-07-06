using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryUITest : MonoBehaviour
{
    [SerializeField] private GameObject ui;


    // Start is called before the first frame update
    void Start()
    {
        LeanTween.scale(ui, Vector3.one, 0.8f).setEaseInElastic();
        LeanTween.scale(ui, new Vector3(1.1f, 1.1f, 1.1f), 0.8f).setDelay(1.0f).setEaseOutElastic().setLoopPingPong();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
