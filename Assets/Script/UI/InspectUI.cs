using UnityEngine;
using TMPro;

public class InspectUI : MonoBehaviour
{
    private TextMeshProUGUI inspectText;

    void Start() 
    {
        inspectText = GetComponentInChildren<TextMeshProUGUI>();   

        //gameObject.SetActive(false); 
    }
    
    public void FadeIn() => LeanTween.value(gameObject, UpdateAlpha, 0.0f, 1.0f, 0.5f); 
    public void MoveIn() => LeanTween.moveLocalY(gameObject, 200.0f, 0.8f).setEaseSpring();
    public void FadeOut() => LeanTween.value(gameObject, UpdateAlpha, 1.0f, 0.0f, 0.5f); 
    public void MoveOut() => LeanTween.moveLocalY(gameObject, -200.0f, 0.8f).setEaseSpring();
    private void UpdateAlpha(float alpha) => GetComponent<CanvasGroup>().alpha = alpha;

    public void ChangeUIText(string sentence) => inspectText.text = sentence;   
}
