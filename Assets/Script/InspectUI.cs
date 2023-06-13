using UnityEngine;
using TMPro;

public class InspectUI : MonoBehaviour
{
    private TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start() => text = GetComponentInChildren<TextMeshProUGUI>();    
    
    public void ChangeUIText(string sentence) => text.text = sentence;   
}
