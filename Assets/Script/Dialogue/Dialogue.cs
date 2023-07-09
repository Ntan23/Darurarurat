using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public string actorName;
    public int actorIndex;
    public Sprite actorSprite;
    public string audioActionName;

	[TextArea(3, 10)]
	public string sentences;
}
