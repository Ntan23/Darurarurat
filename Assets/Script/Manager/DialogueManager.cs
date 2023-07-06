using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour 
{
    #region Singleton
    public static DialogueManager instance;

    void Awake()
    {
        if(instance == null) instance = this;
    }
    #endregion

    [SerializeField] private Dialogue[] dialogues;
    [SerializeField] private Image[] actorImage;
    [SerializeField] private Color inactiveColor;
    [SerializeField] private Color activeColor;
	[SerializeField] private TextMeshProUGUI actorNameText;
	[SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private GameObject dialogueBoard;
    [SerializeField] private GameObject startBoard;
    [SerializeField] private Button nextButton;
    [SerializeField] private int cutDialogueIndex;
    private int dialogueIndex;
    private Queue<Dialogue> dialogueQueue = new Queue<Dialogue>
    ();

    private ScenesManager sm;

    void Start() => sm = ScenesManager.instance;

	public void StartDialogue (Dialogue dialogue)
	{
		actorNameText.text = dialogue.actorName;

		// dialogueQueue.Clear();

        for(int i = 0; i < dialogues.Length; i++) dialogueQueue.Enqueue(dialogues[i]);
        
		DisplayNextSentence();
	}

	public void DisplayNextSentence()
	{
        nextButton.interactable = false;
		if (dialogueQueue.Count == 0)
		{
			EndDialogue();
            sm.GoToNextScene();
			return;
		}
        if(dialogueIndex == cutDialogueIndex)
        {
            EndDialogue();
            dialogueIndex++;
            return;
        }

		Dialogue dialogue = dialogueQueue.Dequeue();

        actorNameText.text = dialogue.actorName;

		StopAllCoroutines();

        actorImage[dialogue.actorIndex].sprite = dialogue.actorSprite;
        actorImage[dialogue.actorIndex].color = activeColor;

        if(dialogue.actorIndex == 0) actorImage[dialogue.actorIndex + 1].color = inactiveColor;
        if(dialogue.actorIndex == 1) actorImage[dialogue.actorIndex - 1].color = inactiveColor;
        
		StartCoroutine(TypeSentence(dialogue.sentences));
        dialogueIndex++;
	}

	IEnumerator TypeSentence (string sentence)
	{
		dialogueText.text = "";
		foreach (char letter in sentence.ToCharArray())
		{
			dialogueText.text += letter;
			yield return new WaitForSeconds(0.02f);
		}

        nextButton.interactable = true;
	}

    public void ShowEndDialogue()
    {
        // LeanTween.moveLocalX(dialogueBoard, 0.0f, 0.8f).setEaseSpring();
        // DisplayNextSentence();
        StartCoroutine(ShowEndDialogueAnimation());
    }

    public void MoveStartBoard()
    {
        // if(startBoard != null) LeanTween.moveLocalX(startBoard, 1924.0f, 0.8f).setEaseSpring();
        // StartDialogue(dialogues[0]);
        StartCoroutine(StartDialogueAnimation());
    }

	void EndDialogue() => LeanTween.moveLocalX(dialogueBoard, 1924.0f, 0.8f).setEaseSpring();
	
    IEnumerator ShowEndDialogueAnimation()
    {
        LeanTween.moveLocalX(dialogueBoard, 0.0f, 0.8f).setEaseSpring();
        yield return new WaitForSeconds(0.2f);
        DisplayNextSentence();
    }

    IEnumerator StartDialogueAnimation()
    {
        if(startBoard != null) LeanTween.moveLocalX(startBoard, 1924.0f, 0.8f).setEaseSpring();
        yield return new WaitForSeconds(0.1f);
        StartDialogue(dialogues[0]);
    }
}
