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

    [SerializeField] private bool isIntro;
    [SerializeField] private Dialogue[] dialogues;
    [SerializeField] private Image[] actorImage;
    [SerializeField] private Color inactiveColor;
    [SerializeField] private Color activeColor;
	[SerializeField] private TextMeshProUGUI actorNameText;
	[SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private GameObject dialogueBoard;
    [SerializeField] private GameObject dialogueContainer;
    [SerializeField] private Button nextButton;
    private TextMeshProUGUI nextButtonText;
    [SerializeField] private int cutDialogueIndex;
    private int dialogueIndex;
    private Queue<Dialogue> dialogueQueue = new Queue<Dialogue>
    ();

    private ScenesManager sm;

    void Start() 
    {
        sm = ScenesManager.instance;

        nextButtonText = nextButton.GetComponent<TextMeshProUGUI>();
        nextButtonText.color = inactiveColor;

        foreach(Image img in actorImage) img.color = inactiveColor;

        StartCoroutine(StartDialogueAnimation());
    }

	public void StartDialogue()
	{
        for(int i = 0; i < dialogues.Length; i++) dialogueQueue.Enqueue(dialogues[i]);
        
		DisplayNextDialogue();
	}

	public void DisplayNextDialogue()
	{
        nextButton.interactable = false;
        nextButtonText.color = inactiveColor;

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
        if(isIntro && dialogueIndex == 1)
        {
            actorNameText.text = null;
            dialogueText.text = null;
        }
        
        Dialogue dialogue = dialogueQueue.Dequeue();

        if(isIntro && dialogueIndex == 0) StartCoroutine(TypeSentence(dialogue.sentences));

        if(isIntro && dialogueIndex > 0) dialogueText.transform.localPosition = new Vector3(dialogueText.transform.localPosition.x, -23.0f, transform.localPosition.z);
        
        if(isIntro && dialogueIndex == 1)
        {
            LeanTween.scale(actorImage[0].gameObject, Vector3.one, 0.5f).setEaseSpring();
            LeanTween.scale(actorImage[1].gameObject, Vector3.one, 0.5f).setEaseSpring().setDelay(0.5f).setOnComplete(() => SettingDialogue(dialogue));
        }

        if(!isIntro || (isIntro && dialogueIndex > 1)) SettingDialogue(dialogue);

        dialogueIndex++;
	}

    void SettingDialogue(Dialogue dialogue)
    {
        actorNameText.text = dialogue.actorName;

        actorImage[dialogue.actorIndex].sprite = dialogue.actorSprite;
        actorImage[dialogue.actorIndex].color = activeColor;
        LeanTween.scale(actorImage[dialogue.actorIndex].gameObject, new Vector3(1.2f, 1.2f, 1.2f), 0.5f).setEaseSpring();

        if(dialogue.actorIndex == 0) 
        {
            actorImage[dialogue.actorIndex + 1].color = inactiveColor;
            LeanTween.scale(actorImage[dialogue.actorIndex + 1].gameObject, Vector3.one, 0.5f).setEaseSpring();
        }
        if(dialogue.actorIndex == 1) 
        {
            actorImage[dialogue.actorIndex - 1].color = inactiveColor;
            LeanTween.scale(actorImage[dialogue.actorIndex - 1].gameObject, Vector3.one, 0.5f).setEaseSpring();
        }

        StartCoroutine(TypeSentence(dialogue.sentences));
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
        nextButtonText.color = Color.black;
	}

    public void ShowEndDialogue() => StartCoroutine(ShowEndDialogueAnimation());

    public void MoveStartBoard() => StartCoroutine(StartDialogueAnimation());

	void EndDialogue() => LeanTween.moveLocalX(dialogueBoard, 1924.0f, 0.8f).setEaseSpring().setOnComplete(() => CloseDialogueUI());
    
    void CloseDialogueUI()
    {
        actorNameText.text = null;
        dialogueText.text = null;
        dialogueContainer.transform.localScale = Vector3.zero;
        
        foreach(Image img in actorImage) 
        {
            img.gameObject.transform.localScale = Vector3.zero;
            img.color = inactiveColor;
        }
    }

    IEnumerator ShowEndDialogueAnimation()
    {
        LeanTween.moveLocalX(dialogueBoard, 0.0f, 0.8f).setEaseSpring();
        yield return new WaitForSeconds(0.6f);
        LeanTween.scale(dialogueContainer, Vector3.one, 0.5f).setEaseSpring();
        LeanTween.scale(actorImage[0].gameObject, Vector3.one, 0.5f).setEaseSpring().setDelay(0.5f);
        LeanTween.scale(actorImage[1].gameObject, Vector3.one, 0.5f).setEaseSpring().setDelay(1.0f).setOnComplete(() => DisplayNextDialogue());
    }

    IEnumerator StartDialogueAnimation()
    {
        yield return new WaitForSeconds(0.6f);
        if(isIntro) 
        {
            dialogueText.transform.localPosition = new Vector3(dialogueText.transform.localPosition.x, 43.0f, transform.localPosition.z);
            LeanTween.scale(dialogueContainer, Vector3.one, 0.5f).setEaseSpring().setOnComplete(() => StartDialogue());
        }
        else if(!isIntro)
        {
            LeanTween.scale(dialogueContainer, Vector3.one, 0.5f).setEaseSpring();
            LeanTween.scale(actorImage[0].gameObject, Vector3.one, 0.5f).setEaseSpring().setDelay(0.5f);
            LeanTween.scale(actorImage[1].gameObject, Vector3.one, 0.5f).setEaseSpring().setDelay(1.0f).setOnComplete(() => StartDialogue());
        }
    }
}
