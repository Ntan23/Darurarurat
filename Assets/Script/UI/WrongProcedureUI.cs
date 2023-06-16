using UnityEngine;
using TMPro;
using System.Collections;

public class WrongProcedureUI : MonoBehaviour
{
    private TextMeshProUGUI wrongText;
    private RectTransform rectTransform;

    void Start() 
    {
        wrongText = GetComponentInChildren<TextMeshProUGUI>();    
        rectTransform = GetComponent<RectTransform>();
    }

    public void FadeIn() 
    {
        LeanTween.value(gameObject, UpdateAlpha, 0.0f, 1.0f, 1.5f); 
        LeanTween.moveLocalY(gameObject, 420.0f, 0.5f).setEaseInBack();
    }
    
    public void FadeOut() 
    {
        LeanTween.value(gameObject, UpdateAlpha, 1.0f, 0.0f, 1.5f); 
        StartCoroutine(Wait());
    }

    private void UpdateAlpha(float alpha) => GetComponent<CanvasGroup>().alpha = alpha;

    public void UpdateText(string sentence) => wrongText.text = sentence;

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.5f);
        LeanTween.moveLocalY(gameObject, 664.0f, 0.5f).setEaseOutBack();
    }
}
