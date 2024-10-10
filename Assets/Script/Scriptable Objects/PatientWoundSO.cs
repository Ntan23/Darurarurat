using UnityEngine;

[CreateAssetMenu()]
public class PatientWoundSO : ScriptableObject
{
    public string woundName;
    public int woundIndex;
    public Sprite woundSprite;
    public float treatPrice;
    public float failPrice;
    public int[] objectNeededToTreatIndex;
}
