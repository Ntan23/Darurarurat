using UnityEngine;

public class Test : MonoBehaviour
{
    public RectTransform atas;
    public RectTransform bawah;
    public RectTransform bg;
    public float startYPointTop, startYPointBot;
    public float nextYPoint, nextYPointTop, nextYPointBot;
    public float moveDuration = 0.2f;
    public bool show, hide;
    private void Start() 
    {
        startYPointTop = atas.localPosition.y;
        startYPointBot = bawah.localPosition.y;
        HideAwake();
        nextYPointTop = startYPointTop - nextYPoint;
        nextYPointBot = startYPointBot + nextYPoint;
    }
    private void Update() {
        if(show)
        {
            show = false;
            Show();
        }

        if(hide)
        {
            hide = false;
            MoveOutBG();
        }
    }

    public void HideAwake()
    {
        LeanTween.moveLocalY(atas.gameObject, startYPointTop, 0);
        LeanTween.moveLocalY(bawah.gameObject, startYPointBot, 0);
        LeanTween.alpha(bg, 0, 0);
        atas.gameObject.SetActive(false);
        bawah.gameObject.SetActive(false);
        bg.gameObject.SetActive(false);
    }
    public void Show()
    {
        atas.gameObject.SetActive(true);
        bawah.gameObject.SetActive(true);
        bg.gameObject.SetActive(true);
        MoveInBG();
    }
    public void MoveInBG()
    {
        LeanTween.moveLocalY(atas.gameObject, nextYPointTop, moveDuration);
        LeanTween.moveLocalY(bawah.gameObject, nextYPointBot, moveDuration);
        LeanTween.alpha(bg, 0.5f, moveDuration);
    }
    public void MoveOutBG()
    {
        LeanTween.moveLocalY(atas.gameObject, startYPointTop, moveDuration);
        LeanTween.moveLocalY(bawah.gameObject, startYPointBot, moveDuration);
        LeanTween.alpha(bg, 0, moveDuration).setOnComplete(
            ()=>
            {
                atas.gameObject.SetActive(false);
                bawah.gameObject.SetActive(false);
                bg.gameObject.SetActive(false);
            }
        );
    }
}
